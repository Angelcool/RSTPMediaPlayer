using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace VideoPlayerForWpf.Controller
{
    /// <summary>
    /// 切组接口
    /// </summary>
    public class GroupController : ApiController
    {
        /// <summary>
        /// 切组成功后回调
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Choose()
        {
            return new { success = true, msg = string.Empty };
        }

        /// <summary>
        /// 开始呼叫成功后回调
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Start()
        {
            return new { success = true, msg = string.Empty };
        }

        /// <summary>
        /// 停止呼叫成功后回调
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Stop()
        {
            return new { success = true, msg = string.Empty };
        }
    }
}
