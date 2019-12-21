using log4net;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using videoPlayer.model;

namespace videoPlayer.Util
{
    public class LayoutMng
    {
        private static ILog logger = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 窗口模式
        /// key n*n中的n
        /// </summary>
        private Dictionary<int, List<WinInfo>> basePanelMap = new Dictionary<int, List<WinInfo>>();

        /// <summary>
        /// 保存播放窗口的位置和大小信息
        /// key: 正在播放的数量
        /// </summary>
        private Dictionary<int, List<WinInfo>> videoPanelMap = new Dictionary<int, List<WinInfo>>();

        /// <summary>
        /// 巡航模板
        /// </summary>
        private List<WinInfo> cruisePanelList = new List<WinInfo>();

        private int _baseWidth;
        private int _baseHeight;

        public LayoutMng(int baseWidth, int baseHeight) {
            this._baseWidth = baseWidth;
            this._baseHeight = baseHeight;

            initBaseWin();
            initExtWin();
            //巡航窗口默认数量是4
            initCruiseWin(4);
        }

        public List<WinInfo> getLayoutByCount(int count) {
            try
            {
                //初始化时还没有播放窗口，此时可能需要先计算大小
                if (0 == count) {
                    count = 1;
                }
                return videoPanelMap[count];
            }
            catch (Exception e) {
                logger.Warn("没有这个窗口数量的模板：" + count);
                return null;
            }
        }

        public List<WinInfo> getCruiseLayout()
        {
            return cruisePanelList;
        }

        /// <summary>
        /// 初始化基本布局的窗口信息（1*1，2*2，3*3，4*4分屏模式）
        /// </summary>
        private void initBaseWin() {
        initBaseWin(1);
        initBaseWin(2);
        initBaseWin(3);
        initBaseWin(4);
        }

        private void initBaseWin(int count) {
            int width = this._baseWidth / count;
            int height = this._baseHeight / count;

            List<WinInfo> playerWinInfos = new List<WinInfo>();

            int index = 1;
            for (int y = 0; y < count; y++) {

                for (int x = 0; x < count; x++)
                {
                    //设置位置和宽高
                    WinInfo info = new WinInfo();
                    info.index = index++;

                    info.x = width * x;
                    info.width = width;
                    
                    info.y = height * y;
                    info.height = height;

                    playerWinInfos.Add(info);
                }
            }

            basePanelMap[count] = playerWinInfos;
        }

        private void initCruiseWin(int count)
        {
            int index = 1;
            for (int i = 0; i < count; i++)
            {
                //设置位置和宽高
                WinInfo info = new WinInfo();
                info.index = index++;
                info.width = this._baseWidth;
                info.height = this._baseHeight;
                info.x = this._baseWidth * i;
                info.y = 0;

                cruisePanelList.Add(info);
            }
        }

        /// <summary>
        /// 初始化扩展布局的窗口信息
        /// 对应的窗口数量是：
        /// 1->1*1
        /// 2,3,4->2*2
        /// 5,6,9->3*3
        /// 7,8,10~16->4*4
        /// </summary>
        private void initExtWin()
        {
            videoPanelMap[1] = basePanelMap[1];

            videoPanelMap[2] = basePanelMap[2];
            videoPanelMap[3] = basePanelMap[2];
            videoPanelMap[4] = basePanelMap[2];

            videoPanelMap[9] = basePanelMap[3];

            videoPanelMap[10] = basePanelMap[4];
            videoPanelMap[11] = basePanelMap[4];
            videoPanelMap[12] = basePanelMap[4];
            videoPanelMap[13] = basePanelMap[4];
            videoPanelMap[14] = basePanelMap[4];
            videoPanelMap[15] = basePanelMap[4];
            videoPanelMap[16] = basePanelMap[4];

            //初始化窗口是5，6个的模板
            List<WinInfo> playerWinInfos = new List<WinInfo>();
            List<WinInfo> tempPlayerWinInfos = basePanelMap[3];
            //1号窗口
            WinInfo info = infoCp(tempPlayerWinInfos[0]); 
            info.width = info.width * 2;
            info.height = info.height * 2;
            playerWinInfos.Add(info);

            playerWinInfos.Add(tempPlayerWinInfos[2]);
            playerWinInfos.Add(tempPlayerWinInfos[5]);
            playerWinInfos.Add(tempPlayerWinInfos[6]);
            playerWinInfos.Add(tempPlayerWinInfos[7]);
            playerWinInfos.Add(tempPlayerWinInfos[8]);
            resetIndex(playerWinInfos);

            videoPanelMap[5] = playerWinInfos;
            videoPanelMap[6] = playerWinInfos;

            //初始化窗口是7，8个的模板
            playerWinInfos = new List<WinInfo>();
            tempPlayerWinInfos = basePanelMap[4];

            //1号窗口
            info = infoCp(tempPlayerWinInfos[0]);
            info.width = info.width * 3;
            info.height = info.height * 3;
            playerWinInfos.Add(info);

            playerWinInfos.Add(tempPlayerWinInfos[3]);
            playerWinInfos.Add(tempPlayerWinInfos[7]);
            playerWinInfos.Add(tempPlayerWinInfos[11]);
            playerWinInfos.Add(tempPlayerWinInfos[12]);
            playerWinInfos.Add(tempPlayerWinInfos[13]);
            playerWinInfos.Add(tempPlayerWinInfos[14]);
            playerWinInfos.Add(tempPlayerWinInfos[15]);
            resetIndex(playerWinInfos);

            videoPanelMap[7] = playerWinInfos;
            videoPanelMap[8] = playerWinInfos;

            logger.Info("初始化窗口模板完成。。。");
        }

        private WinInfo infoCp(WinInfo temp) {
            WinInfo info = new WinInfo();
            info.x = temp.x;
            info.y = temp.y;
            info.width = temp.width;
            info.height = temp.height;

            return info;
        }

        private void resetIndex(List<WinInfo> playerWinInfos) {
            int i = 1;
            foreach (WinInfo info in playerWinInfos)
            {
                info.index = i++;
            }
        }
    }
}
