using System;
using System.Web.Http;
using VideoPlayerForWpf.Models;

namespace VideoPlayerForWpf.Controller
{
    /// <summary>
    /// HomeController
    /// </summary>
    public class HomeController : ApiController
    {
        /// <summary>
        /// //定义在 PlayController 中的一个事件，参数是MessageArgs对象
        /// </summary>
        public static event EventHandler<MessageEntity> VideoPlayEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public object Index(MessageEntity model)
        {
            try
            {
                VideoPlayEvent?.Invoke(null, model);

                return new { success = true, msg = "" };
            }
            catch (Exception ex)
            {
                return new { success = false, msg = ex.Message };
            }
        }
    }
}
