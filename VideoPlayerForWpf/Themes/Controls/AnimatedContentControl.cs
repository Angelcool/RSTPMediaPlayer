using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace VideoPlayerForWpf.Themes.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Mercurius.CodeBuilder.UI.Themes"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Mercurius.CodeBuilder.UI.Themes;assembly=Mercurius.CodeBuilder.UI.Themes"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:AnimatedTabControl/>
    ///
    /// </summary>
    /// <summary>
    /// 支持动画的TabControl。
    /// </summary>
    public class AnimatedContentControl : ContentControl
    {
        #region 字段

        /// <summary>
        /// 定时器
        /// </summary>
        private DispatcherTimer _timer;

        #endregion

        #region 路由事件

        /// <summary>
        /// 注册路由事件：TabItem选择正在改变事件。
        /// </summary>
        public static readonly RoutedEvent SelectionChangingEvent = EventManager.RegisterRoutedEvent(
            "SelectionChanging", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AnimatedContentControl));

        #endregion

        #region 事件

        /// <summary>
        /// 定义TabItem选择正在改变事件。
        /// </summary>
        public event RoutedEventHandler SelectionChanging
        {
            add { AddHandler(SelectionChangingEvent, value); }
            remove { RemoveHandler(SelectionChangingEvent, value); }
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 静态构造方法
        /// </summary>
        static AnimatedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedContentControl), new FrameworkPropertyMetadata(typeof(AnimatedContentControl)));
        }

        /// <summary>
        /// 默认构造方法。
        /// </summary>
        public AnimatedContentControl()
        {
            DefaultStyleKey = typeof(AnimatedContentControl);
        }

        #endregion

        #region 受保护方法

        /// <summary>
        /// 重载TabItem选择改变事件：异步处理SelectionChanging事件。
        /// </summary>
        /// <param name="e">路由事件信息</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            this.Dispatcher.BeginInvoke(
                (Action)delegate
                {
                    this.RaiseSelectionChangingEvent();

                    this.StopTimer();

                    this._timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 500) };

                    EventHandler handler = null;
                    handler = (sender, args) =>
                    {
                        this.StopTimer();
                        base.OnContentChanged(oldContent, newContent);
                    };
                    this._timer.Tick += handler;
                    this._timer.Start();
                });
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 抛出SelectionChanging事件。
        /// </summary>
        private void RaiseSelectionChangingEvent()
        {
            var args = new RoutedEventArgs(SelectionChangingEvent);
            RaiseEvent(args);
        }

        /// <summary>
        /// 停止计时器。
        /// </summary>
        private void StopTimer()
        {
            if (this._timer != null)
            {
                this._timer.Stop();
                this._timer = null;
            }
        }

        #endregion
    }
}
