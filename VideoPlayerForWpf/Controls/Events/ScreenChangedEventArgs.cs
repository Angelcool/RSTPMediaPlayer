using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerForWpf.Controls.Events
{
    /// <summary>
    /// 屏幕改变事件信息
    /// </summary>
    public class ScreenChangedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// 屏幕数量
        /// </summary>
        public ScreenCount ScreenCount { get; set; }

        #endregion
    }

    /// <summary>
    /// 屏幕数量枚举
    /// </summary>
    public enum ScreenCount
    {
        /// <summary>
        /// 四屏
        /// </summary>
        Four = 4,

        /// <summary>
        /// 六屏
        /// </summary>
        Six = 6,

        /// <summary>
        /// 九屏
        /// </summary>
        Nine = 9,

        /// <summary>
        /// 十六屏
        /// </summary>
        SixTeen = 16
    }
}
