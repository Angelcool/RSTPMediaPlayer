using Newtonsoft.Json;
using System.Collections.Generic;

namespace VideoPlayerForWpf.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RootVideo
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "videos")]
        public IList<VideoEntity> Videos { get; set; }
    }

    /// <summary>
    /// 视频实体
    /// </summary>
    public class VideoEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "callId")]
        public int CallId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "imgUrl")]
        public string ImgUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "callIdStr")]
        public string CallIdStr { get; set; }
        /// <summary>
        /// 郑子琪
        /// </summary>
        [JsonProperty(PropertyName = "sendMemberName")]
        public string SendMemberName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "messageUrl")]
        public string MessageUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "terminalMemberType")]
        public string TerminalMemberType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "sendUniqueNo")]
        public int SendUniqueNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "sendMemberId")]
        public int SendMemberId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "allMessageVersion")]
        public int AllMessageVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "sendUniqueNoStr")]
        public string SendUniqueNoStr { get; set; }
    }
}
