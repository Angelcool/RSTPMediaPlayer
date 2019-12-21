using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace videoPlayer
{
    /// <summary>
    /// 收藏弹框
    /// </summary>
    public partial class Popout : Form
    {
        public Popout()
        {
            InitializeComponent();
        }
        public string collectName { get; set; }
        private void Popout_Load(object sender, EventArgs e)
        {
            txtCollectName.Text = "";
        }
        Boolean textboxHasText = false;//判断输入框是否有文本
        private void button2_Click(object sender, EventArgs e)
        {
            collectName = txtCollectName.Text;
            string pattern = @"^[\\Α-\￥a-zA-Z]{1}[\\Α-\￥0-9a-zA-Z]{1,14}$";
            var b = Regex.IsMatch(collectName,pattern);
            if (b)
            {
                this.DialogResult = DialogResult.OK;
            };
            label3.Text = "2-15位中英文，首位非数字";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtCollectName_Enter(object sender, EventArgs e)
        {
            label3.Text = "";
            if (textboxHasText == false)
                txtCollectName.Text = "";

            txtCollectName.ForeColor = Color.Black;
        }

        private void txtCollectName_Leave(object sender, EventArgs e)
        {
            if (txtCollectName.Text == "")
            {
                txtCollectName.Text = "2-15位中英文，首位非数字";
                txtCollectName.ForeColor = Color.LightGray;
                textboxHasText = false;
            }
            else
                textboxHasText = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
