using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerForWpf.Controls.Events
{
    /// <summary>
    /// 监控单元缩放改变事件参数
    /// </summary>
    public class ZoomChangedEventArgs
    {
        #region Properties

        /// <summary>
        /// 是否为全屏
        /// </summary>
        public bool IsFullScreen { get; set; }

        #endregion
    }
}
