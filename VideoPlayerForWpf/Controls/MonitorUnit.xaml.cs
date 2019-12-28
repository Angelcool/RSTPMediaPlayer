using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VideoPlayerForWpf.Controls
{
    /// <summary>
    /// MonitorUnit.xaml 的交互逻辑
    /// </summary>
    public partial class MonitorUnit : UserControl
    {
        public MediaState LoadedBehavior
        {
            get { return (MediaState)GetValue(LoadedBehaviorProperty); }
            set { SetValue(LoadedBehaviorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadedBehavior.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadedBehaviorProperty = DependencyProperty.Register("LoadedBehavior", typeof(MediaState), typeof(MonitorUnit), new PropertyMetadata());

        #region Constructors

        /// <summary>
        /// 构造方法
        /// </summary>
        public MonitorUnit()
        {
            InitializeComponent();
        }

        #endregion
    }
}
