using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPAsyncServer.model
{
    public class MessageModel
    {
        /// <summary>
        /// 描述 如：开始 结束
        /// </summary>
        public string videoMessageType { get; set; }
        public string memberNo { get; set; }
        /// <summary>
        /// 弹出框提示信息
        /// </summary>
        public string prompt { get; set; }
        public VideoPlayModel videoData { get; set; }
        public List<VideoPlayModel> videoDataList { get; set; }
        public bool collectStat { get; set; }
        public string data { get; set; }
        public string memberMessageType { get; set; }
        //public bool isExigency { get; set; }
        public WatchSubordinateLiveMessageBean watchSubordinateLiveMessageBean { get; set; }

    }

    /// <summary>
    /// 播放实体类
    /// </summary>
    public class VideoPlayModel
    {
        /// <summary>
        /// 组编号
        /// </summary>
        public int groupNo { get; set; }
        /// <summary>
        /// 组名称
        /// </summary>
        public string groupName { get; set; }

        /// <summary>
        /// 是否铺满
        /// </summary>
        public Boolean? covered { get; set; }

        /// <summary>
        /// 是否显示组呼按钮
        /// </summary>
        public Boolean showGroupCall { get; set; }

        /// <summary>
        /// 是否显示加载动画
        /// </summary>
        public bool showLoading { get; set; }
        public bool isCamera { get; set; }
        /// <summary>
        /// 是否显示云台控制
        /// </summary>
        public bool showPtz { get; set; }
        /// <summary>
        /// 自动点名序号
        /// </summary>
        public int sort { get; set; }
        /// <summary>
        /// 视频框标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 是否覆盖焦点框
        /// </summary>
        public bool coverageFocus { get; set; }
        /// <summary>
        /// 上报人姓名
        /// </summary>
        public string liveMemberName { get; set; }
        /// <summary>
        /// 上报人账号
        /// </summary>
        public string liveMemberNo { get; set; }
        /// <summary>
        /// 上报账号设备标识
        /// </summary>
        public string liveMemberUniqueNo { get; set; }
        public string resultCode { get; set; }
        public string resultDesc { get; set; }
        public string rtspUrl { get; set; }
        public string theme { get; set; }
        public string callId { get; set; }
        public string terminalMemberType { get; set; }
        public string streamType { get; set; }

        /// <summary>
        /// osd叠层信息
        /// </summary>
        public OSD osd { get; set; }
    }

    public class WatchSubordinateLiveMessageBean
    {
        public bool isThisPlatform { get; set; }
        public string liveName { get; set; }
        public string liverPlatform { get; set; }
        public int no { get; set; }
        public string videoUrl { get; set; }
    }

    /// <summary>
    /// osd叠层
    /// </summary>
    public class OSD
    {
        /// <summary>
        /// 是否显示osd叠层
        /// </summary>
        public bool show { get; set; } = true;

        /// <summary>
        /// osd叠层文字
        /// </summary>
        public String stOSD { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// OSD 基于图像右上角显示位置
        /// </summary>
        public Position position { get; set; }

        /// <summary>
        /// osd叠层宽度
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// osd叠层高度
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// osd叠层透明度，0~255
        /// </summary>
        public int alpha { get; set; }

        /// <summary>
        /// osd叠层颜色R
        /// </summary>
        public int red { get; set; }

        /// <summary>
        /// osd叠层颜色G
        /// </summary>
        public int green { get; set; }

        /// <summary>
        /// osd叠层颜色B
        /// </summary>
        public int blue { get; set; }

        /// <summary>
        /// OSD显示背景颜色 全为0背景透明
        /// </summary>
        public int shadowcolor { get; set; }

    }

    /// <summary>
    /// 按照右上左下的优先级
    /// left与right、top与bottom必须有一个有值
    /// </summary>
    public class Position
    {
        /// <summary>
        /// 右边距
        /// </summary>
        public int? right { get; set; }

        /// <summary>
        /// 上边距
        /// </summary>
        public int? top { get; set; }

        /// <summary>
        /// 左边距
        /// </summary>
        public int? left { get; set; }

        /// <summary>
        /// 下边距
        /// </summary>
        public int? bottom { get; set; }
    }

}
