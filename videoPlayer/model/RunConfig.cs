using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using videoPlayer.eunm;

namespace videoPlayer.model
{
    /// <summary>
    /// 启动时需要的参数
    /// </summary>
    public class RunConfig
    {
        /// <summary>
        /// 访问pc的url
        /// </summary>
        public string pcUrl { get; set; }
        /// <summary>
        /// 点名的url
        /// </summary>
        public string callUrl { get; set; }
        /// <summary>
        /// 下个点名的url
        /// </summary>
        public string nextCallUrl { get; set; }
        /// <summary>
        /// URL序号，0，1，2，按字段顺序访问URL
        /// </summary>
        public int urlIndex { get; set; } = 0;

        /// <summary>
        /// 窗口模式
        /// </summary>
        public WinPatternEnums winPatternEnums { get; set; }

        /// <summary>
        /// 分屏数量
        /// </summary>
        public int splitCount { get; set; } = 4;

        /// <summary>
        /// 是否显示视频列表
        /// </summary>
        public bool isShowPlayList { get; set; }
        /// <summary>
        /// 是否显示历史视频列表
        /// </summary>
        public bool isShowHistoryList { get; set; } = true;

        /// <summary>
        /// 启动的本地tcp端口
        /// </summary>
        public int localPort { get; set; }

        /// <summary>
        /// 进程编号
        /// </summary>
        public int processId { get; set; }
        /// <summary>
        /// 主窗体高
        /// </summary>
        public int mainHeight { get; set; }
        /// <summary>
        /// 主窗体宽
        /// </summary>
        public int mainWidth { get; set; }

        public override string ToString()
        {
            return "pcUrl=" + pcUrl + "\t" 
                + "winPatternEnums=" + winPatternEnums + "\t" 
                + "splitCount=" + splitCount + "\t" 
                + "isShowPlayList=" + isShowPlayList + "\t" 
                + "localPort=" + localPort + "\t" 
                + "processId=" + processId;
        }
    }
}
