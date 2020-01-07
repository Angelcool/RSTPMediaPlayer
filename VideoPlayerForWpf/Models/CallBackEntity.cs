namespace VideoPlayerForWpf.Models
{
    /// <summary>
    /// PPT呼叫回调实体类
    /// </summary>
    public class CallBackEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string CallId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// 0：切组成功，1：开始呼叫，2：停止呼叫
        /// </summary>
        public int Type { get; set; }
    }
}
