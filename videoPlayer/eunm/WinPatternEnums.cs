using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace videoPlayer.eunm
{
    /// <summary>
    /// 设置窗口模式
    /// </summary>
    public enum WinPatternEnums
    {
        [Description("预览模式")]
        PREVIEW,
        [Description("分屏模式")]
        SPLIT,
        [Description("巡航模式")]
        CRUISE,
        [Description("点名模式")]
        CALL,
        [Description("竖屏预览模式")]
        PREVIEW_VERTICAL
    }
}
