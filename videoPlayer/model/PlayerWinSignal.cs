using EasyPlayerNetSDK;
using log4net;
using Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TCPAsyncServer.model;
using videoPlayer.eunm;

namespace videoPlayer.model
{
    /// <summary>
    /// 单个的播放器窗口
    /// </summary>
    public class PlayerWinSignal
    {
        private static ILog logger = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int _index { get; set; }

        public string key { get; set; }

        /// <summary>
        /// 播放后的通道号
        /// </summary>
        public int channelId { get; set; }

        public bool selectStatus { get; set; }

        public bool maxStatus { get; set; }

        public bool videoPlayStatus { get; set; }

        /// <summary>
        /// 是否加载中
        /// </summary>
        public bool loading { get; set; }

        public bool audioPlayedStatus { get; set; }

        public MessageModel messageModel { get; set; }

        private frmMain mainForm { get; set; }

        public Panel onePlayerPanel { get; set; }
        private Panel titlePanel;
        private Panel rtspPanel;

        private Label loadingLabel;//加载动画

        private Panel ptzPanel;

        private Panel comBtnPanel;

        private Button cameraSwitch;
        private Button volumeSwitch;
        private Label tittleLibel;

        /// <summary>
        /// 保存所有的组件名，用来查找
        /// </summary>
        private List<string> assemblyNameList = new List<string>();

        /// <summary>
        /// 保存组件名对应的tips
        /// </summary>
        private Dictionary<string, string> tipMap = new Dictionary<string, string>();

        /// <summary>
        /// 云台命令
        /// </summary>
        private Dictionary<string, string> ptzMap = new Dictionary<string, string>();

        /// <summary>
        /// 保存图标
        /// </summary>
        private Dictionary<string, Image> ptzIconMap = new Dictionary<string, Image>();
        private Dictionary<string, Image> ptzIconFocusMap = new Dictionary<string, Image>();

        private Action<Bitmap, Label> createImageAnimatorEvent;

        public PlayerWinSignal(int index, frmMain mainForm) {


            selectStatus = false;
            maxStatus = false;
            videoPlayStatus = false;
            audioPlayedStatus = false;
            _index = index;
            key = "";
            this.mainForm = mainForm;

            createImageAnimatorEvent = mainForm.imageAnimator;

            initWin(index);
        }

        public void changeLoadingStatus(bool isShow)
        {
            lock (this)
            {
                if (loadingLabel.Visible != isShow)
                {
                    loadingLabel.Visible = isShow;
                }
            }
            
        }

        /// <summary>
        /// 隐藏加载动画
        /// </summary>
        public void hideLoading() {
            if (loadingLabel.Visible)
            {
                loadingLabel.Hide();
            }
        }

        /// <summary>
        /// 显示加载动画
        /// </summary>
        public void showLoading() {
            if (!loadingLabel.Visible)
            {
                loadingLabel.Show();
            }
        }

        /// <summary>
        /// 加载动画的显示状态
        /// </summary>
        public bool loadingVisible() {
            return loadingLabel.Visible;
        }

        private void imageAnimator(Bitmap animatedGif, Label panel)
        {
            lock (panel)
            {

                Graphics g = panel.CreateGraphics();

                // A Gif image's frame delays are contained in a byte array

                // in the image's PropertyTagFrameDelay Property Item's

                // value property.

                // Retrieve the byte array...

                int PropertyTagFrameDelay = 0x5100;

                PropertyItem propItem = animatedGif.GetPropertyItem(PropertyTagFrameDelay);

                byte[] bytes = propItem.Value;

                // Get the frame count for the Gif...

                FrameDimension frameDimension = new FrameDimension(animatedGif.FrameDimensionsList[0]);

                int frameCount = animatedGif.GetFrameCount(FrameDimension.Time);

                // Create an array of integers to contain the delays,

                // in hundredths of a second, between each frame in the Gif image.

                int[] delays = new int[frameCount + 1];

                int i = 0;
                for (i = 0; i <= frameCount - 1; i++)

                {

                    delays[i] = BitConverter.ToInt32(bytes, i * 4);

                }

                // Play the Gif one time...

                while (true)

                {

                    for (i = 0; i <= animatedGif.GetFrameCount(frameDimension) - 1; i++)

                    {

                        animatedGif.SelectActiveFrame(frameDimension, i);

                        g.DrawImage(animatedGif, new Point(0, 0));

                        Application.DoEvents();

                        Thread.Sleep(delays[i] * 10);

                    }

                }
            }

        }

        public PlayerWinSignal findByName(string name)
        {
            if (assemblyNameList.Contains(name)) {
                return this;
            }

            return null;
        }

        public void closeRtsp(bool isLeave) {
            logger.Info("将要关闭窗口[" + key + "]上的流");

            try
            {

                VideoPlayModel v = messageModel.videoData;

                if (isLeave)
                {
                    if (v.isCamera && !messageModel.watchSubordinateLiveMessageBean.isThisPlatform)
                    {
                        mainForm.tcpServerService.notifyHangupVideoChild(v.liveMemberNo, v.isCamera, messageModel.watchSubordinateLiveMessageBean);
                    }
                    else if (v.streamType == "RTSP")
                    {
                        mainForm.tcpServerService.notifyHangupVideo(v);
                    }
                }

                //关闭视频
                int ret = PlayerSdk.EasyPlayer_CloseStream(channelId);
                logger.Info("关闭视频是否成功：" + ret);
                if (ret == 0)
                {
                    //关闭音频
                    int audioStatus = PlayerSdk.EasyPlayer_PlaySound(channelId);
                    logger.Info("关闭音频是否成功：" + audioStatus);
                    channelId = 0;

                    videoPlayStatus = false;
                    audioPlayedStatus = false;

                    //关闭后隐藏
                    onePlayerPanel.Visible = false;
                    titlePanel.Visible = false;
                    comBtnPanel.Visible = false;
                    ptzPanel.Visible = false;

                    //清空保存的播放参数
                    this.messageModel = null;
                    this.key = "";

                    //全屏播放窗口关闭后，还原窗口
                    if (mainForm.checkFullScreenWinByKey(key))
                    {
                        mainForm.changeFullScreen(null);
                    }
                }
            }
            catch (Exception e)
            {

                logger.Error("",e);
            }
        }

        /// <summary>
        /// 重置窗口的位置
        /// </summary>
        /// <param name="info"></param>
        public void changeLocation(WinInfo info) {

            logger.Info("需要设置当前窗口显示信息：" + info.ToString());

            showBtn(false);

            if (maxStatus && onePlayerPanel.Visible)  //当前窗口在最大化状态
            {
                onePlayerPanel.Width = mainForm.getVideoPlayerParentPanel().Width;
                onePlayerPanel.Height = mainForm.getVideoPlayerParentPanel().Height;
                onePlayerPanel.Location = new Point(0, 0);
            }
            else
            {
                onePlayerPanel.Width = info.width;
                onePlayerPanel.Height = info.height;
                onePlayerPanel.Location = new Point(info.x, info.y);
            }
            loadingLabel.Location = new Point((rtspPanel.Width - loadingLabel.Width) / 2, (rtspPanel.Height - loadingLabel.Height) / 2);
            /*if (!videoPlayStatus && null != this.messageModel && null != this.messageModel.videoData && this.messageModel.videoData.showLoading)
            {
                changeLoadingStatus(true);
            }
            else
            {
                changeLoadingStatus(false);
            }*/
            if (null != this.messageModel && null != this.messageModel.videoData && null != messageModel.videoData && null != messageModel.videoData.osd && messageModel.videoData.osd.show)
            {
                //添加水印
                //showOSD(messageModel.videoData.osd);
            }
        }


        public int showOSD(OSD messageOsd)
        {
            PlayerSdk.EASY_PALYER_OSD osd = messageOsd2Osd(messageOsd);
            return PlayerSdk.EasyPlayer_ShowOSD(channelId, 1, osd);
        }


        /// <summary>
        /// 把消息体中的osd信息转换成EasyPlayer的osd结构体
        /// </summary>
        /// <param name="videoOsd"></param>
        /// <returns></returns>
        public PlayerSdk.EASY_PALYER_OSD messageOsd2Osd(OSD videoOsd)
        {
            Position position = videoOsd.position;
            int x;
            int y;
            if (null != position.right)
            {
                x = (int)position.right;
            }
            else
            {
                x = rtspPanel.Width - (int)position.left;
            }
            if (null != position.top)
            {
                y = (int)position.top;
            }
            else
            {
                y = rtspPanel.Height - (int)position.bottom;
            }
            PlayerSdk.tagRECT rect = new PlayerSdk.tagRECT { left = x, bottom = videoOsd.height, right = videoOsd.width, top = y };
            PlayerSdk.EASY_PALYER_OSD osd = new PlayerSdk.EASY_PALYER_OSD { rect = rect, stOSD = videoOsd.stOSD, alpha = (uint)videoOsd.alpha, size = videoOsd.size, shadowcolor = (uint)videoOsd.shadowcolor };
            osd.color = (uint)(videoOsd.blue << 16 | videoOsd.green << 8 | videoOsd.red);
            return osd;
        }

        /// <summary>
        /// 选中视频框与列表   FOR 多次点击
        /// </summary>
        /// <param name="key"></param>
        public void changeSelect(bool selectStatus)
        {
            //没有播放图像的窗口，不能选中
            //todo 暂时所有窗口都能选中
            //if (null == messageModel)
            //{
            //    return;
            //}

            if (this.selectStatus != selectStatus)
            {
                logger.Info("修改窗口[" + _index + "]上的选中状态为：" + selectStatus);
            }

            this.selectStatus = selectStatus;

            if (selectStatus)
            {
                onePlayerPanel.BackColor = Color.FromArgb(0, 120, 215); ;//改变边框颜色
            }
            else
            {
                onePlayerPanel.BackColor = Color.FromArgb(21, 21, 21); ;//改变边框颜色
            }
        }

        /// <summary>
        /// 显示按键栏
        /// </summary>
        /// <param name="key"></param>
        public void showBtn(bool status)
        {
            //没有播放图像的窗口，不显示按钮栏
            if (null == messageModel) {
                return;
            }
            titlePanel.Visible = status;
            comBtnPanel.Visible = status;
            if (messageModel.videoData.showPtz && status)
            {
                ptzPanel.Visible = true;

            }
            else {
                ptzPanel.Visible = false;
            }
        }

        //重置音频窗口的图标
        public void changeVolumeStatus(bool volumeStatus)
        {
            try
            {
                if (this.audioPlayedStatus != volumeStatus)
                {
                    logger.Info("修改窗口[" + _index + "]上的音频状态为：" + volumeStatus);
                }

                this.audioPlayedStatus = volumeStatus;

                if (volumeStatus)
                {
                    //PlayerSdk.EasyPlayer_StopSound();
                    //Thread.Sleep(500);
                    logger.Info("播放音频：" + channelId);
                    int ret = PlayerSdk.EasyPlayer_PlaySound(channelId);
                    logger.Info("播放音频状态：" + ret);
                    volumeSwitch.BackgroundImage = Properties.Resources.redioOpen;
                }
                else
                {
                    PlayerSdk.EasyPlayer_StopSound();
                    volumeSwitch.BackgroundImage = Properties.Resources.redioClose;
                }
            }
            catch (Exception e)
            {

                logger.Error("",e);
            }
        }

        /// <summary>
        /// 开始播放rtsp
        /// </summary>
        public void startPlay()
        {
            try
            {
                //todo 要去掉
                //Thread.Sleep(100);
                logger.Info("将要播放编号[" + messageModel.memberNo + "]的图像，rtsp地址是：" + messageModel.videoData.rtspUrl);

                if (!"start".Equals(messageModel.videoMessageType))
                {
                    return;
                }

                string rtspPath = messageModel.videoData.rtspUrl;
                if (null == rtspPath || "".Equals(rtspPath))
                {
                    logger.Info("将要播放编号[" + messageModel.memberNo + "]的图像，但是没有rtsp地址");
                    return;
                }

                channelId = PlayerSdk.EasyPlayer_OpenStream(rtspPath, rtspPanel.Handle, PlayerSdk.RENDER_FORMAT.DISPLAY_FORMAT_RGB24_GDI, 1, "", "", mainForm.callBack, IntPtr.Zero, mainForm.isHardEncode);
                logger.Info("播放的图像的channelID：" + channelId);
                if (channelId > 0)
                {
                    videoPlayStatus = true;

                    PlayerSdk.EasyPlayer_SetFrameCache(channelId, 3);
                    int ret;
                    bool covered = null == messageModel.videoData.covered ? true : (bool)messageModel.videoData.covered;
                    if (covered)
                    {
                        ret = PlayerSdk.EasyPlayer_SetShownToScale(channelId, 0);
                    }
                    else
                    {
                        ret = PlayerSdk.EasyPlayer_SetShownToScale(channelId, 1);
                    }
                    logger.Info("关闭其它窗口音频");
                    mainForm.resetAllVolumeStatus();
                    logger.Info("打开当前窗口音频图标");
                    changeVolumeStatus(true);
                    //int retStartAudio = PlayerSdk.EasyPlayer_PlaySound(channelId);//播放音频
                    //logger.Info("播放音频状态：" + retStartAudio);
                    //if (retStartAudio == 0)
                    //{
                    //    //关闭其它窗口音频
                    //    logger.Info("关闭其它窗口音频");
                    //    mainForm.resetAllVolumeStatus();
                    //    logger.Info("打开当前窗口音频图标");
                    //    changeVolumeStatus(true);
                    //}
                }
                else //拉流失败
                {
                    logger.Info("拉流失败");
                    channelId = 0;
                    mainForm.tcpServerService.notifyErrMsg(messageModel.memberNo, "播放错误！");
                }
            }
            catch (Exception e)
            {

                logger.Error("",e);
            }
        }

        /// <summary>
        /// 初始化窗口信息
        /// </summary>
        public void initInfo() {
            logger.Info("初始化窗口[" + _index + "]信息");
            if (!onePlayerPanel.Visible) {
                onePlayerPanel.Visible = true;
            }

            VideoPlayModel v = messageModel.videoData;

            tittleLibel.Text = v.theme;   //视频主题
            if (!v.isCamera)
            {
                tittleLibel.Text = messageModel.videoData.title == null || "".Equals(messageModel.videoData.title) ? messageModel.videoData.liveMemberName : messageModel.videoData.title;
            }

            if (v.isCamera && messageModel.watchSubordinateLiveMessageBean.isThisPlatform
                    && v.terminalMemberType == "TERMINAL_PAD_VEHICULAR")
            {
                cameraSwitch.Visible = true;
            }
            else {
                cameraSwitch.Visible = false;
            }
        }

        public void setMessageModel(MessageModel messageModel) {
            this.messageModel = messageModel;

            VideoPlayModel videoPlayModel = messageModel.videoData;
            string callId = messageModel.videoData.callId;
            if (videoPlayModel.isCamera)
            {
                string platName = messageModel.watchSubordinateLiveMessageBean.liverPlatform;
                this.key = messageModel.memberNo + "_" + callId + "_" + platName;
            }
            else
            {
                this.key = messageModel.memberNo + "_" + callId;
            }
        }

        /// <summary>
        /// 根据索引号初始化窗口
        /// </summary>
        /// <param name="index"></param>
        public void initWin(int key) {
            //默认的初始化参数（后边会重置）
            int width = 20;
            int height = 20;


            onePlayerPanel = new Panel();
            Panel panel2 = new Panel();
            titlePanel = new Panel();
            rtspPanel = new Panel();

            loadingLabel = new Label();  //加载动画

            Button currentWinSwitch = new Button(); //窗口大小
            tittleLibel = new Label();        //tittle 文本

            onePlayerPanel.Name = "P1" + key;
            assemblyNameList.Add(onePlayerPanel.Name);
            onePlayerPanel.Size = new Size(width, width);
            onePlayerPanel.Margin = new Padding(2, 0, 0, 2);
            onePlayerPanel.BackColor = Color.FromArgb(21, 21, 21);
            onePlayerPanel.Location = new Point(0, 0);
            //默认隐藏
            onePlayerPanel.Visible = false;

            panel2.Name = "P2" + key;
            assemblyNameList.Add(panel2.Name);
            panel2.Width = width - 4;
            panel2.Height = height - 4;
            panel2.Location = new Point(2, 2);
            panel2.BackColor = Color.FromArgb(34, 34, 34);
            panel2.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            titlePanel.Name = "P3" + key;
            assemblyNameList.Add(titlePanel.Name);
            titlePanel.Size = new Size(10, 35);
            titlePanel.Dock = DockStyle.Top;
            titlePanel.BackColor = Color.FromArgb(34, 34, 34);
            titlePanel.Visible = false;

            //视频流面板
            rtspPanel.Name = "P4" + key;
            assemblyNameList.Add(rtspPanel.Name);
            rtspPanel.Size = new Size(panel2.Width, panel2.Height);
            rtspPanel.Location = new Point(0, 0);
            rtspPanel.BackColor = Color.FromArgb(34, 34, 34);
            rtspPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            rtspPanel.Click += mainForm.RtspPanel_Click;
            rtspPanel.MouseEnter += mainForm.RtspPanel_MouseEnter;
            rtspPanel.MouseDoubleClick += mainForm.RtspPanel_MouseDoubleClick;
            //rtspPanel.Paint += new PaintEventHandler(rtspPanel_Paint);

            loadingLabel.Size = new Size(global::videoPlayer.Properties.Resources.liveLoading.Width, global::videoPlayer.Properties.Resources.liveLoading.Height);
            loadingLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            loadingLabel.Anchor = AnchorStyles.None;
            loadingLabel.BackColor = rtspPanel.BackColor;
            loadingLabel.BackgroundImage = global::videoPlayer.Properties.Resources.liveLoading;
            //loadingLabel.Location = new Point((rtspPanel.Width - loadingLabel.Width) / 2, (rtspPanel.Height - loadingLabel.Height) / 2);
            //loadingLabel.Location = new Point(0, 0);
            //loadingLabel.Paint += new PaintEventHandler(loadingLabel_Paint);
            loadingLabel.Visible = false;

            //关闭按钮
            Button closeBtn = createButton(
                "close" + key,
                new Size(19, 21),
                new Point(titlePanel.Width - 20, 7),
                mainForm.hangupbutton_Click,
                Properties.Resources.close,
                "关闭当前图像"
                );

            //三防Pad切换摄像头
            cameraSwitch = createButton(
                "camera" + key,
                new Size(24, 22),
                new Point(titlePanel.Width - 90, 6),
                mainForm.CameraSwitch_Click,
                Properties.Resources.cameraSwitching,
                "摄像机"
                );
            //摄像机图标默认不显示
            cameraSwitch.Visible = false;

            //标题栏文本
            tittleLibel.AutoSize = false;
            tittleLibel.Name = "tittle" + key;
            assemblyNameList.Add(tittleLibel.Name);
            tittleLibel.Size = new Size(titlePanel.Width - 180, 23);
            tittleLibel.Location = new Point(60, 5);
            tittleLibel.Font = new Font(new FontFamily("微软雅黑"), 14, FontStyle.Regular);

            tittleLibel.TextAlign = ContentAlignment.MiddleCenter;
            tittleLibel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            //titlePanel.Controls.Add(currentWinSwitch);
            if(WinPatternEnums.PREVIEW.Equals(mainForm._cfg.winPatternEnums) || WinPatternEnums.PREVIEW_VERTICAL.Equals(mainForm._cfg.winPatternEnums))
            {
                closeBtn.Visible = false;
            }
            titlePanel.Controls.Add(closeBtn);

            //下边操作栏
            comBtnPanel = new Panel();
            comBtnPanel.Name = "commonButtonPanel" + key;
            assemblyNameList.Add(comBtnPanel.Name);
            comBtnPanel.Size = new Size(10, 35);
            comBtnPanel.Location = new Point(0, panel2.Height - comBtnPanel.Height);
            comBtnPanel.Dock = DockStyle.Bottom;
            comBtnPanel.BackColor = Color.FromArgb(34, 34, 34);
            //默认隐藏
            comBtnPanel.Visible = false;

            //选中按钮
            Button selectbutton = createButton(
                "selectbutton" + key,
                new Size(34, 34),
                new Point(comBtnPanel.Width - 150, 3),
                mainForm.selectbutton_Click,
                Properties.Resources.select,
                "选中"
                );

            //推送按钮
            Button pushbutton = createButton(
                "pushbutton" + key,
                new Size(34, 34),
                new Point(comBtnPanel.Width - 105, 3),
                Pushbutton_Click,
                Properties.Resources.videoPcPush,
                "推送"
                );

            //音量开关
            volumeSwitch = createButton(
                "volume" + key,
                new Size(25, 30),
                new Point(comBtnPanel.Width - 65, 3),
                mainForm.VolumeSwitch_Click,
                Properties.Resources.videoPlay,
                "声音"
                );

            //最大化
            Button maxBtn = createButton(
                "maxBtn" + key,
                new Size(34, 34),
                new Point(comBtnPanel.Width - 35, 3),
                mainForm.MaxBtn_Click,
                Properties.Resources.videoPcMax,
                "最大化"
                );

            //云台面板
            ptzPanel = new Panel();
            ptzPanel.Name = "ptzPanel" + key;
            assemblyNameList.Add(ptzPanel.Name);
            ptzPanel.Size = new Size(150, 110);
            ptzPanel.Location = new Point(width - 160, height - 170);

            //ptzPanel.BackColor = Color.FromArgb(34, 134, 134);
            ptzPanel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            ptzPanel.Visible = false;

            //云台相关---------------------------------------
            Button ptzLeftBtn = createButton(
                "ptzLeftBtn" + key,
                new Size(29, 54),
                new Point(ptzPanel.Width - 140, 25),
                null,
                Properties.Resources.ptzLeft,
                "向左"
                );
            ptzLeftBtn.MouseDown += Btn_MouseDown;
            ptzLeftBtn.MouseUp += Btn_MouseUp;
            ptzMap.Add(ptzLeftBtn.Name, "left");
            ptzIconMap.Add(ptzLeftBtn.Name, Properties.Resources.ptzLeft);
            ptzIconFocusMap.Add(ptzLeftBtn.Name, Properties.Resources.ptzLeft2);

            Button ptzUpBtn = createButton(
                "ptzUpBtn" + key,
                new Size(54, 29),
                new Point(ptzPanel.Width - 120, 3),
                null,
                Properties.Resources.ptzTop,
                "向上"
                );
            ptzUpBtn.MouseDown += Btn_MouseDown;
            ptzUpBtn.MouseUp += Btn_MouseUp;
            ptzMap.Add(ptzUpBtn.Name, "up");
            ptzIconMap.Add(ptzUpBtn.Name, Properties.Resources.ptzTop);
            ptzIconFocusMap.Add(ptzUpBtn.Name, Properties.Resources.ptzTop2);

            Button ptzRightBtn = createButton(
                "ptzRightBtn" + key,
                new Size(29, 54),
                new Point(ptzPanel.Width - 75, 25),
                null,
                Properties.Resources.ptzRight,
                "向右"
                );
            ptzRightBtn.MouseDown += Btn_MouseDown;
            ptzRightBtn.MouseUp += Btn_MouseUp;
            ptzMap.Add(ptzRightBtn.Name, "right");
            ptzIconMap.Add(ptzRightBtn.Name, Properties.Resources.ptzRight);
            ptzIconFocusMap.Add(ptzRightBtn.Name, Properties.Resources.ptzRight2);

            Button ptzDownBtn = createButton(
                "ptzDownBtn" + key,
                new Size(54, 29),
                new Point(ptzPanel.Width - 120, 70),
                null,
                Properties.Resources.ptzDown,
                "向下"
                );
            ptzDownBtn.MouseDown += Btn_MouseDown;
            ptzDownBtn.MouseUp += Btn_MouseUp;
            ptzMap.Add(ptzDownBtn.Name, "down");
            ptzIconMap.Add(ptzDownBtn.Name, Properties.Resources.ptzDown);
            ptzIconFocusMap.Add(ptzDownBtn.Name, Properties.Resources.ptzDown2);

            Button ptzZoomInBtn = createButton(
                "ptzZoomInBtn" + key,
                new Size(34, 34),
                new Point(ptzPanel.Width - 40, 5),
                null,
                Properties.Resources.zoomIn,
                "拉近"
                );
            ptzZoomInBtn.MouseDown += Btn_MouseDown;
            ptzZoomInBtn.MouseUp += Btn_MouseUp;
            ptzMap.Add(ptzZoomInBtn.Name, "zoomin");
            ptzIconMap.Add(ptzZoomInBtn.Name, Properties.Resources.zoomIn);
            ptzIconFocusMap.Add(ptzZoomInBtn.Name, Properties.Resources.zoomIn2);

            Button ptzZoomOutBtn = createButton(
                "ptzZoomOutBtn" + key,
                new Size(34, 34),
                new Point(ptzPanel.Width - 40, 65),
                null,
                Properties.Resources.zoomOut,
                "拉远"
                );
            ptzZoomOutBtn.MouseDown += Btn_MouseDown;
            ptzZoomOutBtn.MouseUp += Btn_MouseUp;
            ptzMap.Add(ptzZoomOutBtn.Name, "zoomout");
            ptzIconMap.Add(ptzZoomOutBtn.Name, Properties.Resources.zoomOut);
            ptzIconFocusMap.Add(ptzZoomOutBtn.Name, Properties.Resources.zoomOut2);

            //云台
            ptzPanel.Controls.Add(ptzLeftBtn);
            ptzPanel.Controls.Add(ptzUpBtn);
            ptzPanel.Controls.Add(ptzRightBtn);
            ptzPanel.Controls.Add(ptzDownBtn);

            ptzPanel.Controls.Add(ptzZoomInBtn);
            ptzPanel.Controls.Add(ptzZoomOutBtn);

            //下边操作按钮
            comBtnPanel.Controls.Add(volumeSwitch);
            comBtnPanel.Controls.Add(selectbutton);
            comBtnPanel.Controls.Add(pushbutton);
            comBtnPanel.Controls.Add(maxBtn);

            //if (videoPlayModel.isCamera == 0 && messageModel.watchSubordinateLiveMessageBean.isThisPlatform
            //        && videoPlayModel.terminalMemberType == "TERMINAL_PAD_VEHICULAR")
            //{
            //    titlePanel.Controls.Add(cameraSwitch);
            //}

            titlePanel.Controls.Add(tittleLibel);
            rtspPanel.Controls.Add(ptzPanel);
            rtspPanel.Controls.Add(loadingLabel);

            panel2.Controls.Add(titlePanel);
            panel2.Controls.Add(comBtnPanel);
            panel2.Controls.Add(rtspPanel);

            onePlayerPanel.Controls.Add(panel2);

            
        }

        private void rtspPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = rtspPanel.CreateGraphics(); //需要在上面写字的控件
            PointF rotatePoint = new PointF(this.rtspPanel.Height / 2, this.rtspPanel.Width / 2); //设定旋转的中心点
            Matrix myMatrix = new Matrix();
            myMatrix.RotateAt(270, rotatePoint, MatrixOrder.Append); //旋转270度
            g.Transform = myMatrix;
            g.FillRectangle(SystemBrushes.ActiveCaption, e.ClipRectangle);
        }

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll ")]
        private static extern bool BitBlt(
                                        IntPtr hdcDest,   //   handle   to   destination   DC 
                                        int nXDest,   //   x-coord   of   destination   upper-left   corner 
                                        int nYDest,   //   y-coord   of   destination   upper-left   corner 
                                        int nWidth,   //   width   of   destination   rectangle 
                                        int nHeight,   //   height   of   destination   rectangle 
                                        IntPtr hdcSrc,   //   handle   to   source   DC 
                                        int nXSrc,   //   x-coordinate   of   source   upper-left   corner 
                                        int nYSrc,   //   y-coordinate   of   source   upper-left   corner 
                                        System.Int32 dwRop   //   raster   operation   code 
                         );

        private void loadingLabel_Paint(object sender, PaintEventArgs e) {
            lock (this.loadingLabel)
            {
                Bitmap image = global::videoPlayer.Properties.Resources.liveLoading;
                /*logger.Info("开始委托代理");
                createImageAnimatorEvent.BeginInvoke(image, (Label)sender, null, null);
                logger.Info("委托代理成功");*/
                imageAnimator(image, this.loadingLabel);
            }
        }

        private Button createButton(string btnName, Size size, Point location, EventHandler click, Image icon, string tips)
        {
            Button btn = new Button();
            btn.Name = btnName;

            //保存组件名
            assemblyNameList.Add(btnName);
            //保存按钮上的提示文字
            tipMap.Add(btnName, tips);

            btn.Size = size;
            btn.Location = location;
            btn.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            btn.BackgroundImage = icon;
            btn.BackgroundImageLayout = ImageLayout.Center;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.FlatStyle = FlatStyle.Flat;
            btn.UseVisualStyleBackColor = true;
            btn.Cursor = Cursors.Hand;
            btn.TabStop = false;
            if (null != click)
            {
                btn.Click += click;
            }
            btn.MouseEnter += Btn_MouseEnter;

            return btn;
        }

        /// <summary>
        /// 云台控制按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            //根据控件名称获取提示文字
            Button button = sender as Button;
            string ptzCmd = ptzMap[button.Name];
            
            button.BackgroundImage = ptzIconFocusMap[button.Name];

            //发送云台控制命令
            messageModel.videoMessageType = "ptzControl";
            messageModel.data = ptzCmd;
            mainForm.tcpServerService.sendMessage(JsonConvert.SerializeObject(messageModel));
        }

        /// <summary>
        /// 云台控制按钮抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_MouseUp(object sender, MouseEventArgs e)
        {
            //根据控件名称获取提示文字
            Button button = sender as Button;
            string ptzCmd = ptzMap[button.Name];

            button.BackgroundImage = ptzIconMap[button.Name];

            //点击事件后，马上发stop
            messageModel.data = "stop";
            mainForm.tcpServerService.sendMessage(JsonConvert.SerializeObject(messageModel));
        }

        private void Pushbutton_Click(object sender, EventArgs e)
        {
            VideoPlayModel v = messageModel.videoData;

            if (!v.isCamera)
            {
                //mainForm.tcpServerService.notifyPush(v.liveMemberNo, v.isCamera, v.rtspUrl, v.callId,v.streamType, null);
                mainForm.tcpServerService.notifyPush(v);
            }
            else
            {
                //mainForm.tcpServerService.notifyPush(v.liveMemberNo, v.isCamera, null, null, v.streamType,messageModel.watchSubordinateLiveMessageBean);
                mainForm.tcpServerService.notifyPush(v);
            }
        }

        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            //根据控件名称获取提示文字
            Button button = sender as Button;
            string tips = tipMap[button.Name];

            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            p.SetToolTip(sender as Button, tips);
        }
    }
}
