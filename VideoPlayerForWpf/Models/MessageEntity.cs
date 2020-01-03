using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayerForWpf.Enum;

namespace VideoPlayerForWpf.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageEntity
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public VideoMessageType VideoMessageType { get; set; }

        public string MemberNo { get; set; }

        /**
         * 提示信息
         */
        public string Prompt { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string UniqueNo { get; set; }

        public VideoPlayEntity VideoData { get; set; }

        public List<VideoPlayEntity> VideoDataList { get; set; }

        public bool CollectStat { get; set; }

        public string Data { get; set; }

        public string MemberMessageType { get; set; }

        //public WatchSubordinateLiveMessageBean watchSubordinateLiveMessageBean { get; set; }

        public Dictionary<string, object> VideoMap { get; set; }
    }
}
