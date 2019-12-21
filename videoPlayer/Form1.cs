using EasyPlayerNetSDK;
using log4net;
using Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using TCPAsyncServer;
using TCPAsyncServer.model;
using videoPlayer.eunm;
using videoPlayer.model;
using videoPlayer.Util;

namespace videoPlayer
{
    /// ctrl+M+o 展开
    public partial class frmMain : Form
    {
        private static ILog logger = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AsyncTCPServerService tcpServerService;

        ///////////////////跨线程//////////////////////////
        delegate void changePanel(MessageModel messageModel);
        delegate void test01(string text);
        delegate void endPlayVideo(string memberNo);
        delegate void collectStat(string memberNo, bool stat);
        delegate void killMyself();
        delegate void autoCloseVideoWin(int key);//自动关闭异常断开的播放窗口
        delegate void queryMember(string memberNo, string data, string memberMessageType);
        delegate void showMainForm(MessageModel messageModel);
        delegate void showPrompt(string prompt);
        delegate void changeSysStatIcon(float cpuNum,float memNum, decimal netUsage);
        ///////////////////////////////////////////////////
        public PlayerSdk.MediaSourceCallBack callBack = null;
        private bool isInit = false;//Player状态
        private bool isFullScreen = false;//是否全屏
        private string fullScreenName = null;//全屏窗体名字

        //主窗口位置信息
        private WinInfo mainWinInfo;

        public bool isHardEncode = false;

        /// <summary>
        /// 窗口布局
        /// </summary>
        private LayoutMng layout;

        public RunConfig _cfg;

        string _straightClickFlag = "2019-4-2 16:56:19";

        private static Object changLocation = new object();

        /// <summary>
        /// 历史图像窗口
        /// </summary>
        private Panel historyListPanel;

        /// <summary>
        /// 历史图像浏览器
        /// </summary>
        private AxSHDocVw.AxWebBrowser historyListBrowser;

        private int rightListBoxWidth;

        /// <summary>
        /// 四分屏是否是激活状态
        /// </summary>
        private bool isScreenFourActive = true;

        /// <summary>
        /// 六分屏是否是激活状态
        /// </summary>
        private bool isScreenSixActive;

        /// <summary>
        /// 九分屏是否是激活状态
        /// </summary>
        private bool isScreenNineActive;

        /// <summary>
        /// 十六分屏是否是激活状态
        /// </summary>
        private bool isScreenSixteenActive;

        /// <summary>
        /// 所有的播放窗口
        /// </summary>
        private Dictionary<int, PlayerWinSignal> winMap = new Dictionary<int, PlayerWinSignal>();

        /// <summary>
        /// 当前正在播放的窗口索引
        /// </summary>
        private int playIndex = 1;

        public frmMain(RunConfig cfg)
        {

            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;


            InitializeComponent();
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;

            cfg.processId = Process.GetCurrentProcess().Id;

            this._cfg = cfg;

            rightListBoxWidth = 278;

            cfg.localPort = SysInfo.getPortByPattern(cfg.winPatternEnums);

            logger.Info("启动信息是：" + cfg);

            tcpServerService = new AsyncTCPServerService();
            tcpServerService.serverStart(cfg.localPort);                                    //启动TCP服务端
            tcpServerService.startPlayVideoEvent += TCPServerService_startPlayVideoEvent;   // 开始播放事件
            tcpServerService.stopPlayVideoEvent += TCPServerService_stopPlayVideoEvent;     //PC通知停止播放
            tcpServerService.hasdisConnectEvent += TCPServerService_hasdisConnectEvent;     //客户端断开
            tcpServerService.killMyselfEvent += TCPServerService_killMyselfEvent;           //pc通知关闭程序
            tcpServerService.queryMemberEvent += TCPServerService_queryMemberEvent;         //查询终端是否存在
            tcpServerService.showMainFormEvent += TCPServerService_showMainFormEvent;         //显示窗体
            tcpServerService.showPromptEvent += TCPServerService_showPromptEvent;         //显示窗体

            //向pc报告启动结果
            HttpUtils.HttpGetByThread(cfg);

            _cfg.mainWidth = this.Width;
            _cfg.mainHeight = this.Height;

            //设置不同模式下窗口属性
            if (WinPatternEnums.SPLIT.Equals(cfg.winPatternEnums)) {
                cfg.splitCount = 4;
                screen_four.Visible = true;
                screen_six.Visible = true;
                screen_nine.Visible = true;
                screen_sixteen.Visible = true;
                playList.Visible = true;
            }

            if (WinPatternEnums.CALL.Equals(cfg.winPatternEnums)) {
                cfg.splitCount = 6;
                //forword.Visible = false;
                playList.Visible = true;
                this.playList.BackgroundImage = global::videoPlayer.Properties.Resources.institutionList;
            }

            if (WinPatternEnums.CRUISE.Equals(cfg.winPatternEnums))
            {
                _cfg.mainWidth = 400;
                _cfg.mainHeight = 300;
                this.Width = _cfg.mainWidth;
                this.Height = _cfg.mainHeight;
                Rectangle rec = Screen.GetWorkingArea(this);
                this.Location = new Point(0, rec.Height - this.Height);
                forword.Visible = false;
                winMax.Visible = false;
                //winMin.Visible = false;
            }

            if (WinPatternEnums.PREVIEW.Equals(cfg.winPatternEnums))
            {
                _cfg.mainWidth = 400;
                _cfg.mainHeight = 300;
                this.Width = _cfg.mainWidth;
                this.Height = _cfg.mainHeight;
                forword.Visible = false;
                to_split.Visible = true;
                //Thread th = new Thread(new ThreadStart(previewCfg)); //创建线程                     
                //th.Start(); //启动线程

            }

            if (WinPatternEnums.PREVIEW_VERTICAL.Equals(cfg.winPatternEnums))
            {
                _cfg.mainWidth = 400;
                _cfg.mainHeight = 533;
                this.Width = _cfg.mainWidth;
                this.Height = _cfg.mainHeight;
                forword.Visible = false;
                to_split.Visible = true;
                //Thread th = new Thread(new ThreadStart(previewCfg)); //创建线程                     
                //th.Start(); //启动线程

            }

            //去掉标题栏高度
            layout = new LayoutMng(_cfg.mainWidth, _cfg.mainHeight - 46);

            mainWinInfo = new WinInfo();
            mainWinInfo.x = 180;
            mainWinInfo.y = 180;
            mainWinInfo.width = this.Width;
            mainWinInfo.height = this.Height;


            playerWinInit();
        }

        private void previewCfg() {
            forword.Location = new System.Drawing.Point(280, 11);
        }

        private void cruiseCfg()
        {
            winMax.Location = new System.Drawing.Point(400, 11);
        }

        private void TCPServerService_queryMemberEvent(string memberNo, string data, string memberMessageType)
        {
            this.BeginInvoke(new queryMember(queryMemberVideoWindow), memberNo,data, memberMessageType);
        }

        private void TCPServerService_showMainFormEvent(MessageModel messageModel)
        {
            this.BeginInvoke(new showMainForm(showMyWindow),messageModel);
        }

        private void TCPServerService_showPromptEvent(string prompt)
        {
            this.BeginInvoke(new showPrompt(showMessageBox),prompt);
        }

        private void TCPServerService_killMyselfEvent()
        {
            this.BeginInvoke(new killMyself(closeMywindow));
        }

        private void closeMywindow()
        {
            this.Close();
        }

        private void showMyWindow(MessageModel messageModel)
        {

            lock (this)
            {
                if (messageModel.videoData.showLoading)
                {
                    PlayerWinSignal playerWin = getCanPlayPlayerWin(messageModel);
                    if (null == playerWin)
                    {
                        return;
                    }
                    if (playerWin.videoPlayStatus)
                    {
                        //playerWin.closeRtsp();
                        this.Show();
                        return;
                    }
                    try
                    {
                        #region 初始化播放框
                        playerWin.loading = true;
                        if (null == playerWin.messageModel)
                        {
                            playerWin.setMessageModel(messageModel);
                        }
                        else if(null == playerWin.messageModel.videoData)
                        {
                            playerWin.messageModel.videoData = messageModel.videoData;
                        }
                        else
                        {
                            playerWin.messageModel.videoData.showLoading = messageModel.videoData.showLoading;
                        }
                        playerWin.initInfo();
                        changeLocation();
                        #endregion
                        /*if (_minButtonClick == 0)
                        {
                            this.WindowState = FormWindowState.Normal;
                        }
                        if (_minButtonClick == 2)
                        {
                            this.WindowState = FormWindowState.Maximized;
                        }*/
                        //playerWin.changeLoadingStatus(true);
                    }
                    catch (Exception e)
                    {
                        logger.Error("", e);
                    }
                    playerWin.loading = false;
                }
                this.Show();
            }
            

        }

        private void showMessageBox(string message)
        {
            lock (this)
            {
                /*ConfirmDialog confirmDialog = new ConfirmDialog();
                confirmDialog.setPrompt(message);
                //confirmDialog.ShowDialog();
                confirmDialog.Show();
                //confirmDialog.set
                //MessageBox.Show(message);*/
                this.promptBox.Text = message;
                this.promptBox.Visible = true;
            }
        }

        private void TCPServerService_hasdisConnectEvent()
        {
            //logger.Info("客户端断开,自杀吧！");
            logger.Info("客户端断开连接！");
            //this.BeginInvoke(new killMyself(closeMywindow));
        }

        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        public const Int32 AW_CENTER = 0x00000010;
        public const Int32 AW_HIDE = 0x00010000;
        public const Int32 AW_ACTIVATE = 0x00020000;
        public const Int32 AW_SLIDE = 0x00040000;
        public const Int32 AW_BLEND = 0x00080000;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        /// <summary>
        /// PC端通知结束观看
        /// </summary>
        /// <param name="memberNo"></param>
        private void TCPServerService_stopPlayVideoEvent(string memberNo)
        {
            this.BeginInvoke(new endPlayVideo(endPlayVideoImplement), memberNo);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();

            frm2.MdiParent = this;
            frm2.Show();

            var boolHardEncode = ConfigurationManager.AppSettings["HardEncode"];
            if (boolHardEncode == "1")
            {
                isHardEncode = true;
            }
            if (PlayerSdk.EasyPlayer_Init() == 0)   //初始化EasyPlayer
                isInit = true;
            callBack = new PlayerSdk.MediaSourceCallBack(MediaCallback);

            timer1.Enabled = true;  //检测鼠标键盘

            //this.BackColor = Color.Black;
            //this.TransparencyKey = Color.Black;
        }
        
        private void TCPServerService_startPlayVideoEvent(MessageModel messageModel)
        {
            this.BeginInvoke(new changePanel(createVideoPanel), messageModel);
        }

        public Panel getVideoPlayerParentPanel() {
            return videoPlayerParentPanel;
        }

        /// <summary>
        /// 初始化播放窗口面板
        /// </summary>
        private void playerWinInit() {
            logger.Info("初始化播放窗口");
            for (int i=0; i<16; i++) {
                PlayerWinSignal win = new PlayerWinSignal(i + 1, this);
                winMap[i + 1] = win;
                videoPlayerParentPanel.Controls.Add(win.onePlayerPanel);
            }
        }

        /// <summary>
        /// 获取播放中的窗口数量
        /// </summary>
        /// <returns></returns>
        private int getPlayCount() {
            int count = 0;
            foreach (var item in winMap)
            {
                //有播放参数，就是占用了一个播放窗口
                if (null != item.Value.messageModel) {
                    count++;
                }
            }

            return count;
        }

        private PlayerWinSignal getPlayerWinSignal(int key)
        {
            foreach (var item in winMap)
            {
                if (item.Value.channelId == key)
                {
                    return item.Value;
                }
            }

            return null;
        }

        private PlayerWinSignal getPlayerWinSignalByNo(string no)
        {
            foreach (var item in winMap)
            {
                if (item.Value.messageModel.memberNo == no)
                {
                    return item.Value;
                }
            }

            return null;
        }

        private PlayerWinSignal getPlayerWinSignal(string key)
        {
            foreach (var item in winMap)
            {
                if (item.Value.key == key)
                {
                    return item.Value;
                }
            }

            return null;
        }

        private PlayerWinSignal getMinSortPlayerWinSignal() {

            int sort = -1;

            PlayerWinSignal playerWin = null;

            foreach (var item in winMap)
            {
                if (item.Value._index == 1 || null == item.Value.messageModel) {
                    continue;
                }
                if (sort == -1 || sort > item.Value.messageModel.videoData.sort) {
                    sort = item.Value.messageModel.videoData.sort;
                    playerWin = item.Value;
                }
            }

            return playerWin;
        }

        private PlayerWinSignal getPlayerWinSignalByName(string name)
        {
            foreach (var item in winMap)
            {
                if (null != item.Value.findByName(name))
                {
                    return item.Value;
                }
            }

            return null;
        }

        public bool checkFullScreenWinByKey(string key)
        {
            foreach (var item in winMap)
            {
                if (null != item.Value.findByName(fullScreenName))
                {
                    return item.Value.key.Equals(key);
                }
            }

            return false;
        }



        private PlayerWinSignal getSelectPlayerWinSignal()
        {
            foreach (var item in winMap)
            {
                if (item.Value.selectStatus)
                {
                    return item.Value;
                }
            }

            return null;
        }

        public void resetAllVolumeStatus()
        {
            foreach (var item in winMap)
            {
                if (item.Value.audioPlayedStatus)
                {
                    item.Value.changeVolumeStatus(false);
                }
            }
        }

        Dictionary<int, string> _callbackERROR = new Dictionary<int, string>();
        /// <summary>
        /// 数据流回调
        /// </summary>
        /// <param name="_channelId">The _channel identifier.</param>
        /// <param name="_channelPtr">The _channel PTR.</param>
        /// <param name="_frameType">Type of the _frame.</param>
        /// <param name="pBuf">The p buf.</param>
        /// <param name="_frameInfo">The _frame information.</param>
        /// <returns>System.Int32.</returns>
        public int MediaCallback(int channelId, IntPtr _channelPtr, int _frameType, IntPtr pBuf, ref PlayerSdk.EASY_FRAME_INFO _frameInfo)
        {
            try
            {
                if (_frameType == 4)
                {
                    if (_frameInfo.codec == 1667592818)
                    {
                        string memberNoKey = "";
                        PlayerWinSignal playerWin = getPlayerWinSignal(1000 + channelId);
                        if (playerWin == null) {
                            logger.Info("没有找到playerWin,channelId:" + channelId);
                            return 0;
                        }
                        logger.Debug("返回错误memberNo===" + playerWin.key + "channelId" + channelId);
                        if (!_callbackERROR.Keys.Contains(channelId))//第一次错误码过来
                        {
                            this.BeginInvoke(new showPrompt(showMessageBox), "已断开");
                            _callbackERROR[channelId] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else//第二次以上错误码过来
                        {
                            DateTime lasterrortime = Convert.ToDateTime(_callbackERROR[channelId]);
                            if (lasterrortime.AddSeconds(120) < DateTime.Now)
                            {
                                this.BeginInvoke(new showPrompt(showMessageBox), "");
                                logger.Info("停掉channel:" + channelId);
                                _callbackERROR.Remove(channelId);
                                this.BeginInvoke(new autoCloseVideoWin(notifyPCKill), channelId + 1000);

                            }
                        }
                    }
                }
                else//正确回调过来
                {
                    if (_frameType == 1 || _frameType == 2)
                    {
                        /*PlayerWinSignal playerWin = getPlayerWinSignal(1000 + channelId);
                        if (playerWin == null)
                        {
                            logger.Info("没有找到playerWin,channelId:" + channelId);
                        }
                        else
                        {
                            playerwin.changeloadingstatus(false);
                        }*/
                        if (_callbackERROR.Keys.Contains(channelId))
                        {
                            logger.Info("在字典中删除channelId" + channelId);
                            this.BeginInvoke(new showPrompt(showMessageBox), "已连接");
                            _callbackERROR.Remove(channelId);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error("",ex);
            }
            return 0;
        }

        private void notifyPCKill(int key)
        {
            PlayerWinSignal playerWin = getPlayerWinSignal(key);

            //没有在播放
            if (playerWin.videoPlayStatus == false) {
                return;
            }

            //关闭视频流
            playerWin.closeRtsp(true);

            if (WinPatternEnums.PREVIEW.Equals(_cfg.winPatternEnums) || WinPatternEnums.PREVIEW_VERTICAL.Equals(_cfg.winPatternEnums)) {
                playIndex = 1;
            }

            /////////////////
            //var c = videoPlayerParentPanel.Controls.Find("P1" + memberNoKey, true).FirstOrDefault();

            ////////////移除控件///////////////////
            //videoPlayerParentPanel.Controls.Remove(c);
            //videoPanelList.Remove(c as Panel);

            //var videoPanelCount = videoPanelList.Count;
            changeLocation();

            //if (maximization)
            //{
            //    maximization = false;
            //    foreach (Panel p in videoPanelList)
            //    {
            //        p.Visible = true;
            //    }
            //}
            /////////////////
        }

        #region 窗口拖动
        //首先﹐.定义鼠標左鍵按下時的Message标识﹔其次﹐在Form1_MouseDown方法﹐讓操作系統誤以為是按下标题栏。
        //1.定义鼠標左鍵按下時的Message标识
        private const int WM_NCLBUTTONDOWN = 0XA1;   //.定义鼠標左鍵按下
        private const int HTCAPTION = 2;

        //2.讓操作系統誤以為是按下标题栏
        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //為當前的應用程序釋放鼠標鋪獲
            ReleaseCapture();
            //發送消息﹐讓系統誤以為在标题栏上按下鼠標
            SendMessage((int)this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        //3.申明程序中所Windows的API函數
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern int ReleaseCapture();

        #endregion
        #region 获取键盘和鼠标没有操作的时间
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            // 设置结构体块容量 
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            // 捕获的时间 
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        //获取键盘和鼠标没有操作的时间
        private static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            // 捕获时间 
            if (!GetLastInputInfo(ref vLastInputInfo))
                return 0;
            else
                return Environment.TickCount - (long)vLastInputInfo.dwTime;
        }
        #endregion

        /// <summary>
        /// 计数 FOR memmber
        /// </summary>
        List<string> _memberNoList = new List<string>();

        /// <summary>
        /// 重新定位
        /// </summary>
        private void changeLocation()
        {

            lock (changLocation)
            {
                //正在播放的数量
                //int playerCount = getPlayCount();
                //分屏的数量
                int playerCount = _cfg.splitCount;
                if (WinPatternEnums.PREVIEW.Equals(_cfg.winPatternEnums) || WinPatternEnums.PREVIEW_VERTICAL.Equals(_cfg.winPatternEnums))
                {
                    playerCount = 1;
                    if (_cfg.isShowHistoryList)
                    {
                        List<WinInfo> playerWinInfos = layout.getLayoutByCount(playerCount);
                        //先扩宽窗口,显示历史图像列表
                        if (this.Width <= playerWinInfos[0].width)
                        {
                            this.Width = _cfg.mainWidth + rightListBoxWidth;
                        }
                        createHistoryList();
                    }
                }
                //logger.Info("正在播放的数量是：" + playerCount + "，playIndex是：" + playIndex);
                logger.Info("分屏的数量是：" + playerCount + "，playIndex是：" + playIndex);

                //if (0 == playerCount) {
                //    logger.Info("没有正在播放视频的窗口");
                //    return;
                //}

                //处理被关掉的窗口还在占用窗口
                //if ((playIndex - 1) > playerCount)
                //{
                //    playerCount = playIndex;
                //}

                if (!WinPatternEnums.CRUISE.Equals(_cfg.winPatternEnums))
                {
                    List<WinInfo> playerWinInfos = layout.getLayoutByCount(playerCount);

                    int i = 0;
                    foreach (var item in winMap)
                    {
                        //显示固定模式的全部窗口
                        //if (i < playerWinInfos.Count)
                        if (i < playerCount)
                        {
                            item.Value.onePlayerPanel.Visible = true;
                            item.Value.changeLocation(playerWinInfos[i++]);
                        }
                    }
                }

                if (WinPatternEnums.CRUISE.Equals(_cfg.winPatternEnums))
                {
                    List<WinInfo> playerWinInfos = layout.getCruiseLayout();

                    //先扩宽窗口
                    if (this.Width < playerWinInfos[0].width * playerCount)
                    {
                        this.Width = playerWinInfos[0].width * playerCount;
                    }

                    int i = 0;
                    foreach (var item in winMap)
                    {
                        //显示固定模式的全部窗口
                        if (i < playerWinInfos.Count)
                        {
                            item.Value.onePlayerPanel.Visible = true;
                            item.Value.changeLocation(playerWinInfos[i++]);
                        }
                    }
                }
            }
            
        }

        private void createHistoryList() {

            if(null == historyListPanel)
            {
                historyListPanel = new Panel();
            }

            if(null == historyListBrowser)
            {
                historyListBrowser = new AxSHDocVw.AxWebBrowser();
            }

            historyListPanel.Name = "historyListPanel";
            historyListPanel.Width = rightListBoxWidth;
            historyListPanel.Height = this.Height -46;
            historyListPanel.Location = new Point(this.Width - rightListBoxWidth, 46);
            historyListPanel.BackColor = Color.FromArgb(34, 34, 34);
            historyListPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            //historyListPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.historyListPanel_Paint);


            historyListBrowser.Enabled = true;
            historyListBrowser.Location = new Point(1, 1);
            historyListBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("historyListBrowser.OcxState")));
            historyListBrowser.Size = new System.Drawing.Size(rightListBoxWidth - 2, this.Height - 46 - 2);

            this.historyListPanel.Controls.Add(this.historyListBrowser);
            this.Controls.Add(this.historyListPanel);

            /*if (isFullScreen)
            {
                historyListPanel.Hide();
            }
            else
            {
                historyListPanel.Show();
            }*/

        }

        private void openHistoryVideoList(string memberNo, string memberUniqueNo) {
            if (this.historyListPanel == null) {
                createHistoryList();
            }
            string url = "http://127.0.0.1:8099/html/videoVcrList.html?terminalNo=" + memberNo + "&terminalUniqueNo=" + memberUniqueNo + "&t=" + DateTime.Now.Ticks;
            openBrowser(this.historyListBrowser, url);
        }

        /// <summary>
        /// 浏览器中打开URL
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="url"></param>
        private void openBrowser(AxSHDocVw.AxWebBrowser webBrowser, string url) {
            logger.Info("打开浏览器");
            Object EmptyString = System.Reflection.Missing.Value;
            Object Zero = 0;
            webBrowser.Silent = true;  //屏蔽脚本错误
            logger.Info("url:" + url);
            webBrowser.Navigate(url, ref Zero, ref Zero, ref Zero, ref Zero);
            logger.Info("打开浏览器成功");
        }

        private void historyListPanel_Paint(object sender, PaintEventArgs e)
        {
            string url = "http://127.0.0.1:8099/html/videoVcrList.html?t=" + DateTime.Now.Ticks;
            openBrowser(this.historyListBrowser, url);
        }

        /// <summary>
        /// 播放框点击选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RtspPanel_Click(object sender, EventArgs e)
        {
            Panel rtspPanel = sender as Panel;
            string name = rtspPanel.Name;
            logger.Info("窗口[" + name + "]被选中了");
            changeSelect(name);
            PName.Focus();
        }
        public void selectbutton_Click(object sender, EventArgs e)
        {
            Button selectbutton = sender as Button;
            string name = "P4" + selectbutton.Name.Substring(12);
            logger.Info("窗口[" + name + "]被选中了");
            select(name);
            PName.Focus();
        }

        private void select(string name)
        {
            PlayerWinSignal playerWin = getPlayerWinSignalByName(name);

            MessageModel message = playerWin.messageModel;

            if (null == message && !WinPatternEnums.CALL.Equals(_cfg.winPatternEnums))
            {
                return;
            }

            foreach (var item in winMap)
            {
                //todo 把key换成_index试试能不能用
                if (playerWin._index.Equals(item.Value._index))
                {
                    item.Value.changeSelect(true);
                }
                else
                {
                    item.Value.changeSelect(false);
                }
            }
            if (null == message)
            {
                return;
            }
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("memberNo", message.memberNo);
            parameter.Add("uniqueNo", message.videoData.liveMemberUniqueNo);
            parameter.Add("videoMessageType", "select");
            parameter.Add("videoData", message.videoData);
            string content = JsonConvert.SerializeObject(parameter);
            tcpServerService.sendMessage(content);
        }

        private void changeSelect(string name)
        {
            PlayerWinSignal playerWin = getPlayerWinSignalByName(name);

            MessageModel message = playerWin.messageModel;

            if (null == message && !WinPatternEnums.CALL.Equals(_cfg.winPatternEnums)) {
                return;
            }

            foreach (var item in winMap)
            {
                //todo 把key换成_index试试能不能用
                if (playerWin._index.Equals(item.Value._index))
                {
                    item.Value.changeSelect(!item.Value.selectStatus);
                }
                else
                {
                    item.Value.changeSelect(false);
                }
            }
            if (null == message)
            {
                return;
            }
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("memberNo", message.memberNo);
            parameter.Add("uniqueNo", message.videoData.liveMemberUniqueNo);
            parameter.Add("videoMessageType", "select");
            parameter.Add("videoData", message.videoData);
            string content = JsonConvert.SerializeObject(parameter);
            tcpServerService.sendMessage(content);
        }

        /// <summary>
        /// 音频开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void VolumeSwitch_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            PlayerWinSignal playerWin = getPlayerWinSignalByName(button.Name);

            logger.Info("当前音频播放状态：" + playerWin.audioPlayedStatus);

            //if (playerWin.audioPlayedStatus)
            //{
            //    return;
            //}

            foreach (var item in winMap)
            {
                if (playerWin.key.Equals(item.Value.key)) {
                    continue;
                }
                if (item.Value.audioPlayedStatus)
                {
                    item.Value.changeVolumeStatus(false);
                }
                //if (null != item.Value.messageModel && item.Value.videoPlayStatus) {
                //    item.Value.changeVolumeStatus(!item.Value.audioPlayedStatus);
                //}
                //if (!playerWin.key.Equals(item.Value.key))
                //{
                //    item.Value.changeVolumeStatus(!item.Value.audioPlayedStatus);
                //}
                //else {
                //    if (item.Value.audioPlayedStatus) {
                //        item.Value.changeVolumeStatus(false);
                //    }
                //}
                //else
                //{
                //    item.Value.changeVolumeStatus(false);
                //}
            }
            playerWin.changeVolumeStatus(!playerWin.audioPlayedStatus);

            PName.Focus();
        }

        public void imageAnimator(Bitmap animatedGif,Label panel) {

            lock (panel)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(delegate { imageAnimator(animatedGif, panel); }));
                    return;
                }

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
                    if (!panel.Visible)
                    {
                        return;
                    }

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

        /// 创建播放窗口模板
        /// </summary>
        /// <param name="text"></param>
        private void createVideoPanel(MessageModel messageModel)
        {
            try
            {
                VideoPlayModel videoPlayModel = messageModel.videoData;
                string memberNo = messageModel.memberNo;
                string key;
                string callId = videoPlayModel.callId;
                if((WinPatternEnums.PREVIEW.Equals(_cfg.winPatternEnums) || WinPatternEnums.PREVIEW_VERTICAL.Equals(_cfg.winPatternEnums)) && _cfg.isShowHistoryList)
                {
                    string liveMemberNo = videoPlayModel.liveMemberNo;
                    string liveMemberUniqueNo = videoPlayModel.liveMemberUniqueNo;
                    this.openHistoryVideoList(liveMemberNo, liveMemberUniqueNo);
                }
                if (videoPlayModel.isCamera)
                {
                    string platName = messageModel.watchSubordinateLiveMessageBean.liverPlatform;
                    key = memberNo + "_" + callId + "_" + platName;
                }
                else
                {
                    key = memberNo + "_" + callId;
                }

                //查找是否有相同的窗口
                PlayerWinSignal playerWin = getPlayerWinSignal(key);

                if (null != playerWin)
                {
                    logger.Info("查找到相同的窗口,key=" + key);
                    playerWin.setMessageModel(messageModel);
                    logger.Info("窗口上是否在播放图像：" + playerWin.videoPlayStatus);
                    logger.Info("playIndex:" + playIndex);
                    //窗口上没有在播放图像
                    if (!playerWin.videoPlayStatus)
                    {
                        playerWin.startPlay();
                    }
                    //playerWin.changeLoadingStatus(false);

                    changeSelect(playerWin.onePlayerPanel.Name);
                    return;
                }

                //有选中的窗口
                if (messageModel.videoData.coverageFocus)
                {
                    playerWin = getSelectPlayerWinSignal();
                }

                //todo 选中就能播放
                //if (null != playerWin && null != playerWin.messageModel) {
                if (null != playerWin)
                {
                    //先关闭正在播放的流
                    playerWin.closeRtsp(true);
                    playerWin.setMessageModel(messageModel);
                    playerWin.initInfo();
                    playerWin.startPlay();
                    //playerWin.changeLoadingStatus(false);
                    return;
                }

                //正在播放的窗口数量
                int playCount = getPlayCount();

                //if (playIndex > 16 || (1 == playCount && WinPatternEnums.PREVIEW.Equals(_cfg.winPatternEnums)))
                if (WinPatternEnums.PREVIEW.Equals(_cfg.winPatternEnums) || WinPatternEnums.PREVIEW_VERTICAL.Equals(_cfg.winPatternEnums))
                {
                    playIndex = 1;
                }
                else if (playIndex > 4 && WinPatternEnums.CRUISE.Equals(_cfg.winPatternEnums))
                {
                    playIndex = 1;
                }
                else if (WinPatternEnums.SPLIT.Equals(_cfg.winPatternEnums))
                {
                    playerWin = getNotPlayerWin();
                    if (playerWin == null && playIndex > _cfg.splitCount)
                    {
                        playIndex = 1;
                    }
                }
                else if (WinPatternEnums.CALL.Equals(_cfg.winPatternEnums))
                {
                    playerWin = getNotPlayerWin();
                    if (playerWin == null)
                    {
                        playIndex = 1;
                    }
                }

                if (null == playerWin)
                {
                    playerWin = winMap[playIndex++];
                }

                /*if (playerWin.loading)
                {
                    Thread.Sleep(100);
                    this.BeginInvoke(new changePanel(createVideoPanel), messageModel);
                    return;
                }*/

                //先关闭正在播放的流
                if (playerWin.videoPlayStatus)
                {
                    playerWin.closeRtsp(true);
                }

                playerWin.setMessageModel(messageModel);
                playerWin.initInfo();
                playerWin.startPlay();

                
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            finally
            {
                #region 初始化播放框
                changeLocation();
                logger.Info("当前播放窗口的数量" + getPlayCount());
                if (getPlayCount() >= 1)//有播放窗口就显示
                {
                    this.Show();
                }
                #endregion
                //if (getPlayCount() == 1)//上一次的状态是hide
                if (_minButtonClick == 0)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                if (_minButtonClick == 2)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
        }


        private PlayerWinSignal getCanPlayPlayerWin(MessageModel messageModel) {

            try
            {
                VideoPlayModel videoPlayModel = messageModel.videoData;
                string memberNo = messageModel.memberNo;
                string key;
                string callId = messageModel.videoData.callId;
                if (videoPlayModel.isCamera)
                {
                    string platName = messageModel.watchSubordinateLiveMessageBean.liverPlatform;
                    key = memberNo + "_" + callId + "_" + platName;
                }
                else
                {
                    key = memberNo + "_" + callId;
                }

                //查找是否有相同的窗口
                PlayerWinSignal playerWin = getPlayerWinSignal(key);

                if (null != playerWin)
                {
                    logger.Info("查找到相同的窗口,key=" + key);
                    playerWin.setMessageModel(messageModel);
                    logger.Info("窗口上是否在播放图像：" + playerWin.videoPlayStatus);
                    logger.Info("playIndex:" + playIndex);
                    //窗口上没有在播放图像
                    if (!playerWin.videoPlayStatus)
                    {
                        playerWin.startPlay();
                    }

                    changeSelect(playerWin.onePlayerPanel.Name);
                    return null;
                }

                //有选中的窗口
                if (messageModel.videoData.coverageFocus)
                {
                    playerWin = getSelectPlayerWinSignal();
                }

                //todo 选中就能播放
                //if (null != playerWin && null != playerWin.messageModel) {
                if (null != playerWin)
                {
                    //先关闭正在播放的流
                    /*playerWin.closeRtsp();

                    playerWin.setMessageModel(messageModel);
                    playerWin.initInfo();
                    playerWin.startPlay();*/

                    return playerWin;
                }

                //正在播放的窗口数量
                int playCount = getPlayCount();

                //if (playIndex > 16 || (1 == playCount && WinPatternEnums.PREVIEW.Equals(_cfg.winPatternEnums)))
                if (WinPatternEnums.PREVIEW.Equals(_cfg.winPatternEnums) || WinPatternEnums.PREVIEW_VERTICAL.Equals(_cfg.winPatternEnums))
                {
                    playIndex = 1;
                }
                else if (playIndex > 4 && WinPatternEnums.CRUISE.Equals(_cfg.winPatternEnums))
                {
                    playIndex = 1;
                }
                else if (WinPatternEnums.SPLIT.Equals(_cfg.winPatternEnums))
                {
                    playerWin = getNotPlayerWin();
                    if (playerWin == null && playIndex > _cfg.splitCount)
                    {
                        playIndex = 1;
                    }
                }
                else if (WinPatternEnums.CALL.Equals(_cfg.winPatternEnums))
                {
                    playerWin = getNotPlayerWin();
                    if (playerWin == null)
                    {
                        playIndex = 1;
                    }
                }

                if (null == playerWin)
                {
                    playerWin = winMap[playIndex++];
                }

                
                return playerWin;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        private PlayerWinSignal getNotPlayerWin() {

            for (var i = 1; i <= _cfg.splitCount; i++) {
                var win = winMap[i];
                if (!win.videoPlayStatus)
                {
                    playIndex = 1;
                    return win;
                }
            }

            return null;
        }

        public void MaxBtn_Click(object sender, EventArgs e)
        {
            logger.Info("点击了最大化。。。");
            Button btn = sender as Button;
            changeFullScreen(btn.Name);
        }

        public void RtspPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            logger.Info("鼠标双击了。。。");
            Panel panel = sender as Panel;
            changeFullScreen(panel.Name);
        }

        public void changeFullScreen(string name)
        {
            if (isFullScreen)
            {
                this.Location = new Point(mainWinInfo.x, mainWinInfo.y);
                this.Width = mainWinInfo.width;
                this.Height = mainWinInfo.height;
                isFullScreen = false;
                fullScreenName = null;

                foreach (var item in winMap)
                {
                    item.Value.onePlayerPanel.Visible = true;
                }
                changeLocation();
            }
            else if(null != name)
            {
                Rectangle rec = Screen.GetWorkingArea(this);
                PlayerWinSignal playerWin = getPlayerWinSignalByName(name);

                this.Location = new Point(0, -45);
                this.Width = rec.Width;
                this.Height = rec.Height + 45;

                //右边固定框如果是显示的，则全屏缩短宽度，显示右边固定框
                if (null != historyListPanel && historyListPanel.Visible)
                {
                    playerWin.onePlayerPanel.Width = rec.Width - rightListBoxWidth;
                }else
                {
                    playerWin.onePlayerPanel.Width = rec.Width;
                }

                playerWin.onePlayerPanel.Height = rec.Height;
                playerWin.onePlayerPanel.Location = new Point(0, 0);
                if (null != playerWin.messageModel && null != playerWin.messageModel.videoData && null != playerWin.messageModel.videoData && null != playerWin.messageModel.videoData.osd && playerWin.messageModel.videoData.osd.show)
                {
                    //添加水印
                    //playerWin.showOSD(playerWin.messageModel.videoData.osd);
                }
                isFullScreen = true;
                fullScreenName = name;

                //注意，设置显示要在修改窗口之后，否则changLocation方法中会设置其它窗口显示
                foreach (var item in winMap)
                {
                    if (item.Value._index != playerWin._index)
                    {
                        item.Value.onePlayerPanel.Visible = false;
                    }
                }

                //全屏时隐藏右边固定框
                //this.historyListPanel.Hide();
            }
        }

        public void CameraSwitch_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            DateTime lastClickTime = Convert.ToDateTime(_straightClickFlag);
            if (lastClickTime.AddSeconds(1) > DateTime.Now)
            {
                return;
            }

            PlayerWinSignal playerWin = getPlayerWinSignalByName(button.Name);

            string memberNo = playerWin.messageModel.memberNo;

            tcpServerService.notifyCameraSwich(memberNo);
            _straightClickFlag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        

        /// <summary>
        /// 鼠标进入播放框 按钮重置 状态重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RtspPanel_MouseEnter(object sender, EventArgs e)
        {
            var p = sender as Panel;
            PlayerWinSignal playerWin = getPlayerWinSignalByName(p.Name);

            foreach (var item in winMap)
            {
                if (playerWin.key.Equals(item.Value.key))
                {
                    item.Value.showBtn(true);
                }
                else
                {
                    item.Value.showBtn(false);
                }
            }
        }

        /// <summary>
        /// pc通知关闭播放窗口
        /// </summary>
        /// <param name="memberNo"></param>
        private void endPlayVideoImplement(string memberNo)
        {
            PlayerWinSignal playerWin = getPlayerWinSignalByNo(memberNo);
            if (null == playerWin || playerWin.videoPlayStatus == false) {
                tcpServerService.notifyErrMsg(memberNo, memberNo + "上报已结束！");
                return;
            }

            playerWin.closeRtsp(true);
            if (WinPatternEnums.PREVIEW.Equals(_cfg.winPatternEnums) || WinPatternEnums.PREVIEW_VERTICAL.Equals(_cfg.winPatternEnums)) {
                playIndex = 1;
            }
            changeLocation();
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            closeForm(true);
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void closeForm( bool isLeave) {
            int playCount = getPlayCount();
            if (playCount < 1)
            {
                this.playIndex = 1;
                //this.WindowState = FormWindowState.Minimized;
                this.Hide();
                return;
            }
            foreach (PlayerWinSignal playerWinSignal in this.winMap.Values)
            {
                if (playerWinSignal.messageModel != null)
                {
                    playerWinSignal.closeRtsp(isLeave);
                }
            }
            this.playIndex = 1;
            //this.WindowState = FormWindowState.Minimized;
            this.Hide();


            //ConfirmDialog confirmDialog = new ConfirmDialog();
            //if (confirmDialog.ShowDialog() == DialogResult.OK)//关闭所有
            //{
            //    foreach (PlayerWinSignal playerWinSignal in this.winMap.Values)
            //    {
            //        if (playerWinSignal.messageModel != null)
            //        {
            //            playerWinSignal.closeRtsp();
            //        }
            //    }
            //    this.playIndex = 1;
            //    //this.WindowState = FormWindowState.Minimized;
            //    this.Hide();
            //}
            //else
            //{
            //    return;
            //}
        }

        /// <summary>
        /// 记录最小化前窗口的状态，0为normal，1为maximized
        /// </summary>
        int _minButtonClick = 0;

        /// <summary>
        /// 窗口最大化及还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (this.WindowState == FormWindowState.Normal)
            {
                _minButtonClick = 2;
                this.WindowState = FormWindowState.Maximized;
                button.BackgroundImage = Properties.Resources.restore;
            }
            else
            {
                _minButtonClick = 0;
                this.WindowState = FormWindowState.Normal;
                button.BackgroundImage = Properties.Resources.max;
            }
            PName.Focus();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void forword_Click(object sender, EventArgs e)
        {

            if (WinPatternEnums.CALL.Equals(_cfg.winPatternEnums))
            {
                _cfg.urlIndex = 1;
                HttpUtils.HttpGetByThread(_cfg);
                return;
            }

            List<VideoPlayModel> videoPlays = new List<VideoPlayModel>();
            PlayerWinSignal winSignal = null;
            foreach (PlayerWinSignal playerWinSignal in this.winMap.Values)
            {
                if (playerWinSignal.videoPlayStatus)
                {
                    videoPlays.Add(playerWinSignal.messageModel.videoData);
                    winSignal = playerWinSignal;
                }
            }
            if (videoPlays.Count > 0)
            {
                if (WinPatternEnums.SPLIT.Equals(_cfg.winPatternEnums)){
                    tcpServerService.notifyPush(videoPlays);
                }else if (WinPatternEnums.PREVIEW.Equals(_cfg.winPatternEnums) || WinPatternEnums.PREVIEW_VERTICAL.Equals(_cfg.winPatternEnums))
                {
                    this.closeForm(false);
                    tcpServerService.notifyPush2Split(videoPlays[0]);
                }
            }
        }
        #region 窗口拖动事件
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            winMax.BackgroundImage = Properties.Resources.max;
            //為當前的應用程序釋放鼠標鋪獲
            ReleaseCapture();
            //發送消息﹐讓系統誤以為在标题栏上按下鼠標
            SendMessage((int)this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        #endregion

        /// <summary>
        /// 挂断事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void hangupbutton_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Info("关闭小窗口");
                var b = sender as Button;
                var name = b.Parent.Parent.Name;

                PlayerWinSignal playerWin = getPlayerWinSignalByName(name);
                
                //MessageModel messageModel = playerWin.messageModel;
                //VideoPlayModel v = messageModel.videoData;

                if (WinPatternEnums.CALL.Equals(_cfg.winPatternEnums) && playerWin._index == 1) {

                    PlayerWinSignal minSortPlayerWin = getMinSortPlayerWinSignal();
                    if(minSortPlayerWin != null)
                    {
                        MessageModel messageModel = minSortPlayerWin.messageModel;
                        playerWin.closeRtsp(true);
                        playIndex = 1;
                        minSortPlayerWin.closeRtsp(true);
                        createVideoPanel(messageModel);
                        //点名下一个视频
                        _cfg.urlIndex = 2;
                        HttpUtils.HttpGetByThread(_cfg);
                        return;
                    }

                }

                playerWin.closeRtsp(true);
                //changeLocation();
                //设置播放索引为关闭的窗口
                playIndex = playerWin._index;

                //if (v.isCamera && !messageModel.watchSubordinateLiveMessageBean.isThisPlatform)
                //{
                //    tcpServerService.notifyHangupVideoChild(v.liveMemberNo, v.isCamera, messageModel.watchSubordinateLiveMessageBean);
                //}
                //else
                //{
                //    tcpServerService.notifyHangupVideo(v);
                //}
                
                PName.Focus();
                if (WinPatternEnums.CALL.Equals(_cfg.winPatternEnums))
                {
                    //点名下一个视频
                    _cfg.urlIndex = 2;
                    HttpUtils.HttpGetByThread(_cfg);
                }

            }
            catch(Exception ex)
            {
                logger.Error(ex);
                PName.Focus();
            }
        }

        /// <summary>
        /// 鼠标检测定时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            var lastTime = GetLastInputTime();
            TimeSpan spanTime = new TimeSpan(lastTime * 10000);
            
            if (spanTime.Seconds < 3)
            {
                return;
            }

            foreach (var item in winMap)
            {
                item.Value.showBtn(false);
            }
        }

        /// <summary>
        /// 推送点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pushbutton_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            var name = b.Parent.Parent.Name;

            PlayerWinSignal playerWin = getPlayerWinSignalByName(name);
            
        }

        /// <summary>
        /// 关闭PlayerSdk开启的线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            logger.Info("关闭PlayerSdk开启的线程");
            notifyIcon1.Visible = false;
            notifyIcon1.Dispose();
            
            if (isInit)
                PlayerSdk.EasyPlayer_Release();
        }

        bool statusBar = true; //托盘点击状态
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (statusBar)
            {
                this.Show();
                if(_minButtonClick == 0)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                if(_minButtonClick == 2)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                statusBar = false;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                statusBar = true;
            }
        }

        /// <summary>
        /// 第一次渲染 立即隐藏 避免首次启动 渲染问题 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Shown(object sender, EventArgs e)
        {
           // this.Hide();
        }
        
        /// <summary>
        /// 任务栏关闭触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeForm(true);
            e.Cancel = true;
            //logger.Info("程序关闭");
            //notifyIcon1.Dispose();
            //tcpServerService.serverDispose();
        }

        private void videoPlayerParentPanel_SizeChanged(object sender, EventArgs e)
        {
            if (!WinPatternEnums.CRUISE.Equals(_cfg.winPatternEnums))
            {
                int width;
                int height;
                if (this.WindowState == FormWindowState.Maximized)
                {
                    height = this.Height - 46;
                    if (null != historyListPanel && historyListPanel.Visible)
                    {
                        width = this.Width - rightListBoxWidth;
                    }
                    else
                    {
                        width = this.Width;
                    }
                }
                else
                {
                    width = _cfg.mainWidth;
                    height = _cfg.mainHeight - 46;
                }
                layout = new LayoutMng(width, height);
                changeLocation();
            }
        }

        /// <summary>
        /// 查询终端是否在播放
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="data"></param>
        /// <param name="memberMessageType"></param>
        private void queryMemberVideoWindow(string memberNo, string data, string memberMessageType)
        {
            PlayerWinSignal playerWin = getPlayerWinSignal(memberNo);
            //返回播放状态
            tcpServerService.notifyPlayResult(data, memberMessageType, playerWin.videoPlayStatus);
        }

        private void Screen_four_Click(object sender, EventArgs e)
        {
            if (isScreenFourActive || _cfg.splitCount == 4)
            {
                return;
            }
            changeSplitScreenStatus(screen_four);
        }

        private void Screen_six_Click(object sender, EventArgs e)
        {
            if (isScreenSixActive || _cfg.splitCount == 6)
            {
                return;
            }
            changeSplitScreenStatus(screen_six);

        }

        private void Screen_nine_Click(object sender, EventArgs e)
        {
            if (isScreenNineActive || _cfg.splitCount == 9) {
                return;
            }
            changeSplitScreenStatus(screen_nine);

        }

        private void reloadVideoPlayers() {
            int splitCount = _cfg.splitCount;
            List<MessageModel> messages = new List<MessageModel>(splitCount);
            foreach (PlayerWinSignal playerWinSignal in this.winMap.Values)
            {
                if (messages.Count >= splitCount)
                {
                    playerWinSignal.closeRtsp(true);
                    continue;
                }
                if (playerWinSignal.messageModel != null)
                {
                    messages.Add(playerWinSignal.messageModel);
                }
            }

            foreach (MessageModel message in messages) {
                createVideoPanel(message);
            }

        }

        private void PlayList_Click(object sender, EventArgs e)
        {
            logger.Info("视频列表的显示状态是：" + _cfg.isShowPlayList);
            _cfg.isShowPlayList = !_cfg.isShowPlayList;
            this.play_list_panel.Visible = _cfg.isShowPlayList;
            logger.Info("修改视频列表的显示状态为：" + _cfg.isShowPlayList);
        }

        private void changePlayListShowStatus() {
            logger.Info("视频列表的显示状态是：" + _cfg.isShowPlayList);
            _cfg.isShowPlayList = !_cfg.isShowPlayList;
            PlayListBrowser.Visible = _cfg.isShowPlayList;
            logger.Info("修改视频列表的显示状态为：" + _cfg.isShowPlayList);
        }

        private void PlayListBrowser_Enter(object sender, EventArgs e)
        {
            //logger.Info("打开浏览器");
            //Object EmptyString = System.Reflection.Missing.Value;
            //Object Zero = 0;
            //this.PlayListBrowser.Silent = true;  //屏蔽脚本错误
            //string url = "http://127.0.0.1:8099/html/videoList.html";
            //this.PlayListBrowser.Navigate(url, ref Zero, ref Zero, ref Zero, ref Zero);
        }

        private void Play_list_panel_Paint(object sender, PaintEventArgs e)
        {
            string url;
            if (WinPatternEnums.CALL.Equals(_cfg.winPatternEnums)) {
                url = "http://127.0.0.1:8099/html/rollCallList.html?t=" + DateTime.Now.Ticks;
            } else {
                url = "http://127.0.0.1:8099/html/videoList.html?t=" + DateTime.Now.Ticks;
            }
            openBrowser(this.PlayListBrowser, url);
        }

        private void To_split_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 键盘监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListKey(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                changeFullScreen(null);
            }
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            int WM_KEYDOWN = 256;

            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)

            {

                switch (keyData)

                {

                    case Keys.Escape:

                        changeFullScreen(null);
                        //if (isFullScreen)
                        //{
                        //    this.Location = new Point(mainWinInfo.x, mainWinInfo.y);
                        //    this.Width = mainWinInfo.width;
                        //    this.Height = mainWinInfo.height;
                        //    isFullScreen = false;

                        //    foreach (var item in winMap)
                        //    {
                        //        item.Value.onePlayerPanel.Visible = true;
                        //    }
                        //    changeLocation();
                        //}

                        break;

                }



            }

            return false;

        }

        private void Screen_sixteen_Click(object sender, EventArgs e)
        {
            if (isScreenSixteenActive || _cfg.splitCount == 16)
            {
                return;
            }
            changeSplitScreenStatus(screen_sixteen);
        }

        private void changeSplitScreenStatus(Button screen_button)
        {
            lock (this)
            {
                isScreenFourActive = this.screen_four == screen_button;
                isScreenSixActive = this.screen_six == screen_button;
                isScreenNineActive = this.screen_nine == screen_button;
                isScreenSixteenActive = this.screen_sixteen == screen_button;
                _cfg.splitCount = isScreenFourActive ? 4 : isScreenSixActive ? 6 : isScreenNineActive ? 9 : isScreenSixteenActive ? 16 : 4;
                isScreenFourActive = _cfg.splitCount == 4;
                screen_four.BackgroundImage = isScreenFourActive ? global::videoPlayer.Properties.Resources.screen_four_active : global::videoPlayer.Properties.Resources.screen_four;
                screen_six.BackgroundImage = isScreenSixActive ? global::videoPlayer.Properties.Resources.screen_six_active : global::videoPlayer.Properties.Resources.screen_six;
                screen_nine.BackgroundImage = isScreenNineActive ? global::videoPlayer.Properties.Resources.screen_nine_active : global::videoPlayer.Properties.Resources.screen_nine;
                screen_sixteen.BackgroundImage = isScreenSixteenActive ? global::videoPlayer.Properties.Resources.screen_sixteen_active : global::videoPlayer.Properties.Resources.screen_sixteen;
                changeLocation();
            }
        }
    }
}