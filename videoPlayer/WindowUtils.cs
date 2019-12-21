using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace videoPlayer
{
    class WindowUtils
    {
        private static WindowUtils windowUtils = new WindowUtils();


        public static WindowUtils INSTANCE()
        {
            return windowUtils;
        }

        public void test() {
            Panel panel1 = new Panel();
        }

    }
}
