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

namespace VideoPlayerForWpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ScreenSwitch_ScreenChanged(object sender, ScreenChangedEventArgs e)
        {
            MessageBox.Show(e.ScreenCount.ToString());
        }

        private void OnToggleNav(object sender, RoutedEventArgs e)
        {
            var tbtn = sender as ToggleButton;

            this.rightNav.Width = tbtn.IsChecked == true ? new GridLength(240) : new GridLength(1);
        }
    }
}
