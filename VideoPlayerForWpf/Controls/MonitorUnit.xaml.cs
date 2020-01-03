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
using VideoPlayerForWpf.Models;

namespace VideoPlayerForWpf.Controls
{
    /// <summary>
    /// MonitorUnit.xaml 的交互逻辑
    /// </summary>
    public partial class MonitorUnit : UserControl
    {
        #region Fields

        private MonitorEntity monitorData;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public MonitorEntity MonitorData
        {
            get { return this.monitorData; }
            set
            {
                this.monitorData = value;

                if (value != null)
                {
                    this.tbTitle.Text = $"{value.Name} {value.Code} {value.Address}";
                }
                else
                {
                    this.tbTitle.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 加载方式
        /// </summary>
        public MediaState LoadedBehavior
        {
            get { return (MediaState)GetValue(LoadedBehaviorProperty); }
            set { SetValue(LoadedBehaviorProperty, value); }
        }

        /// <summary>
        /// 表格
        /// </summary>
        public int GridRowSpan { get; private set; }

        public int GridColumnSpan { get; private set; }

        public int GridRowIndex { get; private set; }

        public int GridColumnIndex { get; private set; }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// 注册加载方式依赖属性
        /// </summary>
        public static readonly DependencyProperty LoadedBehaviorProperty = DependencyProperty.Register("LoadedBehavior", typeof(MediaState), typeof(MonitorUnit), new PropertyMetadata());

        #endregion

        #region Events

        public event EventHandler<ZoomChangedEventArgs> ZoomChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// 构造方法
        /// </summary>
        public MonitorUnit()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="columnSpan"></param>
        /// <param name="rowSpan"></param>
        public void Initialize(int column, int row = 0, int columnSpan = 1, int rowSpan = 1)
        {
            this.SetValue(Grid.RowProperty, row);
            this.SetValue(Grid.ColumnProperty, column);

            if (columnSpan > 0)
            {
                this.SetValue(Grid.ColumnSpanProperty, columnSpan);
            }

            if (rowSpan > 0)
            {
                this.SetValue(Grid.RowSpanProperty, rowSpan);
            }

            this.GridRowIndex = row;
            this.GridColumnIndex = column;

            this.GridRowSpan = rowSpan;
            this.GridColumnSpan = columnSpan;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentContainer"></param>
        public void ShowFullScreen(Grid parentContainer)
        {
            //MessageBox.Show("111");
            this.SetValue(Grid.RowProperty, 0);
            this.SetValue(Grid.ColumnProperty, 0);
            this.SetValue(Grid.RowSpanProperty, parentContainer.RowDefinitions.Count);
            this.SetValue(Grid.ColumnSpanProperty, parentContainer.ColumnDefinitions.Count);

            this.SetValue(Panel.ZIndexProperty, 1000);

            if (this.LoadedBehavior != MediaState.Play)
            {
                this.mePlayer.Play();
            }
        }

        /// <summary>
        /// 还原
        /// </summary>
        public void Restore()
        {
            this.tbZoom.IsChecked = false;
            this.Initialize(this.GridColumnIndex, this.GridRowIndex, this.GridColumnSpan, this.GridRowSpan);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPushToDevice_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOutPlay_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow popup = new PopupWindow
            {
                Owner = App.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            popup.Show();
        }
    }
}
