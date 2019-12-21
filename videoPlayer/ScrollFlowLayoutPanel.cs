using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace videoPlayer
{
    class ScrollFlowLayoutPanel : FlowLayoutPanel
    {
        public ScrollFlowLayoutPanel()
        {

        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseClick(e);
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            this.Focus();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (this.VerticalScroll.Maximum > this.VerticalScroll.Value + 50)
                    this.VerticalScroll.Value += 50;
                else
                    this.VerticalScroll.Value = this.VerticalScroll.Maximum;
            }
            else
            {
                if (this.VerticalScroll.Value > 50)
                    this.VerticalScroll.Value -= 50;
                else
                {
                    this.VerticalScroll.Value = 0;
                }
            }

        }
    }
}
