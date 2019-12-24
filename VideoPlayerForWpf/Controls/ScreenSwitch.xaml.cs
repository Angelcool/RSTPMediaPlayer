using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoPlayerForWpf.Controls.Events;

namespace VideoPlayerForWpf.Controls
{
    /// <summary>
    /// 屏幕切换组件
    /// </summary>
    public partial class ScreenSwitch : UserControl
    {
        #region Fields

        private ScreenCount _screenCount = ScreenCount.Four;

        #endregion

        #region Properties

        /// <summary>
        /// 屏幕数量
        /// </summary>
        public ScreenCount ScreenCount
        {
            get { return this._screenCount; }
            set
            {
                if (this._screenCount == value)
                {
                    return;
                }

                this._screenCount = value;

                foreach (var item in spScreens.Children)
                {
                    var tbtn = item as ToggleButton;

                    if (tbtn.Tag?.ToString() == ((int)value).ToString())
                    {
                        tbtn.IsChecked = true;
                    }
                    else
                    {
                        tbtn.IsChecked = false;
                    }
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler<ScreenChangedEventArgs> ScreenChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// 构造方法
        /// </summary>
        public ScreenSwitch()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handles

        /// <summary>
        /// toggle button点击事件处理(切换屏幕数量，出发屏幕改变事件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChangeScreen(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tbtn)
            {
                tbtn.IsChecked = true;

                if (tbtn.Parent is Panel panel)
                {
                    foreach (var item in panel.Children)
                    {
                        if (item != tbtn && item is ToggleButton btn)
                        {
                            btn.IsChecked = false;
                        }
                    }
                }

                this.ScreenCount = (ScreenCount)int.Parse(tbtn.Tag.ToString());

                if (this.ScreenChanged != null)
                {
                    this.ScreenChanged.Invoke(this, new ScreenChangedEventArgs
                    {
                        ScreenCount = this.ScreenCount
                    });
                }
            }
        }

        #endregion
    }
}
