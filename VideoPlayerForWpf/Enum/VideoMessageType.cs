using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerForWpf.Enum
{
    /// <summary>
    /// 
    /// </summary>
    public enum VideoMessageType
    {
        /// <summary>
        /// 播放视频
        /// </summary>
        START = 1,//("播放视频",1,"start"),

        /// <summary>
        /// 退出观看
        /// </summary>
        STOP = 2,//("退出观看/停止播放",2,"stop"),

        /// <summary>
        /// 推送
        /// </summary>
        OPENPUSH = 3,//("推送",3,"openPush"),

        /// <summary>
        /// 选中窗口
        /// </summary>
        SELECT = 4,//("选中窗口",4,"select"),

        /// <summary>
        /// 多路视频推送
        /// </summary>
        PUSHMANY = 5,//("多路视频推送",5,"pushMany"),
        //PUSH2SPLIT,//("预览窗口推送到九分屏",6,"push2Split"),

        /// <summary>
        /// 云台控制
        /// </summary>
        PTZCONTROL = 7,//("云台控制",7,"ptzControl"),

        /// <summary>
        /// 显示窗体
        /// </summary>
        SHOW = 8,//("显示窗体",8,"show"),

        /// <summary>
        /// 显示提示
        /// </summary>
        PROMPT = 9,//("显示提示",9,"prompt"),
        //HEART,//("心跳",10,"heart"),

        /// <summary>
        /// 点名状态变更
        /// </summary>
        ROLLCALLSTATUS = 11//("点名状态变更",11,"rollcallstatus");
    }
}
