using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerForWpf.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VideoPlayEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public int GroupNo { get; set; }

        public string GroupName { get; set; }

        /**
         * 是否显示加载动画
         */
        public bool ShowLoading { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IsCamera { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string LiveMemberName { get; set; }

        /// <summary>
        /// 直播设备的编号
        /// </summary>
        public string LiveMemberNo { get; set; }

        /// <summary>
        /// 直播设备唯一码
        /// </summary>

        public string LiveMemberUniqueNo { get; set; }

        public string ResultCode { get; set; }

        public string ResultDesc { get; set; }

        /// <summary>
        /// 视频流地址
        /// </summary>
        public string RtspUrl { get; set; }

        public string Theme { get; set; }

        /// <summary>
        /// 视频流唯一编码
        /// </summary>
        public string CallId { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string TerminalMemberType { get; set; }

        /// <summary>
        /// 视频流类型
        /// </summary>
        public string StreamType { get; set; }

        /**
         * osd叠层信息，没有可以不传
         */
        //public OSD osd { get; set; }


        /**
         * 视频点名 排序
         */
        public int Sort { get; set; }

        /**
         * 是否铺满
         */
        public bool Covered { get; set; }

        /**
         * 是否显示云台控制
         */
        public bool ShowPtz { get; set; }

        /// <summary>
        /// 视频标题
        /// </summary>
        public string Title { get; set; }
        /**
         * 是否显示组呼按钮
         */
        public bool ShowGroupCall { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CoverageFocus { get; set; } = true;
    }
}
