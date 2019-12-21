using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace videoPlayer.model
{
    /// <summary>
    /// 窗口位置和大小信息
    /// </summary>
    public class WinInfo
    {
        public int index { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public override string ToString()
        {
            return "index=" + index + "\t"
                + "x=" + x + "\t"
                + "y=" + y + "\t"
                + "width=" + width + "\t"
                + "height=" + height;
        }
    }
}
