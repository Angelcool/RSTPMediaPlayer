using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using VideoPlayerForWpf.Models;

namespace VideoPlayerForWpf.Controller
{
    /// <summary>
    /// 切组接口
    /// </summary>
    public class GroupController : ApiController
    {
        /// <summary>
        /// //定义在 GroupController 中的一个事件，参数是MessageArgs对象
        /// </summary>
        public static event EventHandler<CallBackEntity> VideoCallBackEvent;

        /// <summary>
        /// 切组成功后回调
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Choose(CallBackEntity model)
        {
            model.Type = 0;
            VideoCallBackEvent?.Invoke(null, model);

            return new { success = true, msg = string.Empty };
        }

        /// <summary>
        /// 开始呼叫成功后回调
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Start(CallBackEntity model)
        {
            model.Type = 1;
            VideoCallBackEvent?.Invoke(null, model);

            return new { success = true, msg = string.Empty };
        }

        /// <summary>
        /// 停止呼叫成功后回调
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Stop(CallBackEntity model)
        {
            model.Type = 2;
            VideoCallBackEvent?.Invoke(null, model);

            return new { success = true, msg = string.Empty };
        }
    }
}
