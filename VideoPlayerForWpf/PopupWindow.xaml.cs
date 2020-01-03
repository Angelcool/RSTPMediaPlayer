using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using VideoPlayerForWpf.Controls.Events;
using Vlc.DotNet.Forms;

namespace VideoPlayerForWpf
{
    /// <summary>
    /// PopupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PopupWindow
    {
        #region Events

        public event EventHandler<ZoomChangedEventArgs> ZoomChanged;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public PopupWindow()
        {
            InitializeComponent();
            //初始化配置，指定引用库  
            //vlcControl.MediaPlayer.VlcLibDirectoryNeeded += App.OnVlcControlNeedsLibDirectory;
            //MediaPlayer.MediaPlayer.EndInit();

            VlcControl myVlcControl = new VlcControl();
            // 创建绑定，绑定Image
            Binding bing = new Binding
            {
                Source = myVlcControl,
                Path = new PropertyPath("VideoSource")
            };

            MediaPlayer.SetBinding(Image.SourceProperty, bing);
            myVlcControl.Play();
            Loaded += PopupWindow_Loaded;
        }

        /// <summary>
        /// 播放器加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PopupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FileInfo info = new FileInfo(@"C:\Users\汪小虎\Source\Repos\1571637122209918.mp4");
            //MediaPlayer.MediaPlayer
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tbn)
            {
                this.ZoomChanged?.Invoke(this, new ZoomChangedEventArgs { IsFullScreen = tbn.IsChecked ?? false });
            }
        }
    }
}
