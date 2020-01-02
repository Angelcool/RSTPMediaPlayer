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
using VideoPlayerForWpf.Controls;
using VideoPlayerForWpf.Controls.Events;
using VideoPlayerForWpf.Models;
using VideoPlayerForWpf.Service;

namespace VideoPlayerForWpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private IList<MonitorEntity> monitorEntities;

        private MonitorDatasService monitorDatasService = new MonitorDatasService();

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += this.MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.monitorEntities = this.monitorDatasService.LoadMonitorDatas();

            this.Load9MonitorUnits();
        }

        private void ScreenSwitch_ScreenChanged(object sender, ScreenChangedEventArgs e)
        {
            switch (e.ScreenCount)
            {
                case ScreenCount.Four:
                    this.Load4MonitorUnits();

                    break;
                case ScreenCount.Six:
                    this.Load6MonitorUnits();

                    break;
                case ScreenCount.Nine:
                    this.Load9MonitorUnits();

                    break;
                case ScreenCount.SixTeen:
                    this.Load16MonitorUnits();

                    break;
                default:
                    break;
            }
        }

        private void OnToggleNav(object sender, RoutedEventArgs e)
        {
            var tbtn = sender as ToggleButton;

            this.rightNav.Width = tbtn.IsChecked == true ? new GridLength(240) : new GridLength(1);
        }

        #region Private Methods

        private void Load4MonitorUnits()
        {
            // 清理所有控件
            this.gridContainer.Children.Clear();

            // 清理行定义
            this.gridContainer.RowDefinitions.Clear();

            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var main = CreateMonitorUnit(0, 0, 0, 3);

            main.LoadedBehavior = MediaState.Play;

            this.gridContainer.Children.Add(main);
            this.gridContainer.Children.Add(CreateMonitorUnit(1, 0, 1));
            this.gridContainer.Children.Add(CreateMonitorUnit(2, 1, 1));
            this.gridContainer.Children.Add(CreateMonitorUnit(3, 2, 1));
        }

        private void Load6MonitorUnits()
        {
            // 清理所有控件
            this.gridContainer.Children.Clear();

            // 清理行定义
            this.gridContainer.RowDefinitions.Clear();


            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var main = CreateMonitorUnit(0, 0, 0, 2, 2);

            main.LoadedBehavior = MediaState.Play;

            this.gridContainer.Children.Add(main);
            this.gridContainer.Children.Add(CreateMonitorUnit(1, 2, 0));
            this.gridContainer.Children.Add(CreateMonitorUnit(2, 2, 1));

            this.gridContainer.Children.Add(CreateMonitorUnit(3, 0, 2));
            this.gridContainer.Children.Add(CreateMonitorUnit(4, 1, 2));
            this.gridContainer.Children.Add(CreateMonitorUnit(5, 2, 2));
        }

        private void Load9MonitorUnits()
        {
            // 清理所有控件
            this.gridContainer.Children.Clear();

            // 清理行定义
            this.gridContainer.RowDefinitions.Clear();


            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var main = CreateMonitorUnit(0, 0, 0, 2, 2);

            main.LoadedBehavior = MediaState.Play;

            this.gridContainer.Children.Add(main);
            this.gridContainer.Children.Add(CreateMonitorUnit(1, 2, 0));
            this.gridContainer.Children.Add(CreateMonitorUnit(2, 2, 1));

            this.gridContainer.Children.Add(CreateMonitorUnit(3, 0, 2));
            this.gridContainer.Children.Add(CreateMonitorUnit(4, 1, 2));
            this.gridContainer.Children.Add(CreateMonitorUnit(5, 2, 2));

            this.gridContainer.Children.Add(CreateMonitorUnit(6, 0, 3));
            this.gridContainer.Children.Add(CreateMonitorUnit(7, 1, 3));
            this.gridContainer.Children.Add(CreateMonitorUnit(8, 2, 3));
        }

        private void Load16MonitorUnits()
        {
            // 清理所有控件
            this.gridContainer.Children.Clear();

            // 清理行定义
            this.gridContainer.RowDefinitions.Clear();

            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var main = CreateMonitorUnit(0, 0, 0, 2, 3);

            main.LoadedBehavior = MediaState.Play;

            this.gridContainer.Children.Add(main);
            this.gridContainer.Children.Add(CreateMonitorUnit(1, 2, 0));
            this.gridContainer.Children.Add(CreateMonitorUnit(2, 2, 1));
            this.gridContainer.Children.Add(CreateMonitorUnit(3, 2, 2));

            this.gridContainer.Children.Add(CreateMonitorUnit(4, 0, 3));
            this.gridContainer.Children.Add(CreateMonitorUnit(5, 1, 3));
            this.gridContainer.Children.Add(CreateMonitorUnit(6, 2, 3));

            this.gridContainer.Children.Add(CreateMonitorUnit(7, 0, 4));
            this.gridContainer.Children.Add(CreateMonitorUnit(8, 1, 4));
            this.gridContainer.Children.Add(CreateMonitorUnit(9, 2, 4));

            this.gridContainer.Children.Add(CreateMonitorUnit(10, 0, 5));
            this.gridContainer.Children.Add(CreateMonitorUnit(11, 1, 5));
            this.gridContainer.Children.Add(CreateMonitorUnit(12, 2, 5));

            this.gridContainer.Children.Add(CreateMonitorUnit(13, 0, 6));
            this.gridContainer.Children.Add(CreateMonitorUnit(14, 1, 6));
            this.gridContainer.Children.Add(CreateMonitorUnit(15, 2, 6));
        }

        private void Unit_ZoomChanged(object sender, ZoomChangedEventArgs e)
        {
            var unit = sender as MonitorUnit;

            if (e.IsFullScreen)
            {
                this.tbNavigation.IsChecked = false;
                this.OnToggleNav(this.tbNavigation, new RoutedEventArgs());

                unit.ShowFullScreen(this.gridContainer);

                return;
            }

            unit.SetValue(Panel.ZIndexProperty, 1);

            unit.Restore();
        }

        private MonitorUnit CreateMonitorUnit(int dataIndex, int col, int row, int colspan = 1, int rowspan = 1)
        {
            var unit = new MonitorUnit();

            unit.MonitorData = this.monitorEntities[dataIndex];

            unit.Initialize(col, row, colspan, rowspan);

            unit.ZoomChanged += this.Unit_ZoomChanged;

            return unit;
        }


        #endregion
    }
}
