using System.Windows;
using System.Windows.Input;

namespace VideoPlayerForWpf.Themes
{
    public class MercuriusWindow : Window
    {
        #region 依赖属性

        /// <summary>
        /// 副标题依赖属性
        /// </summary>
        public string SubTitle
        {
            get { return (string)GetValue(SubTitleProperty); }
            set { SetValue(SubTitleProperty, value); }
        }

        /// <summary>
        /// 注册副标题依赖属性
        /// </summary>
        public static readonly DependencyProperty SubTitleProperty = DependencyProperty.Register("SubTitle", typeof(string), typeof(MercuriusWindow), new PropertyMetadata(""));

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public MercuriusWindow()
        {
            this.SetValue(Window.StyleProperty, Application.Current.Resources[typeof(MercuriusWindow)]);

            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sender, e) => SystemCommands.CloseWindow(this)));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (sender, e) => SystemCommands.MaximizeWindow(this), (sender, e) => e.CanExecute = e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (sender, e) => SystemCommands.MinimizeWindow(this), (sender, e) => e.CanExecute = this.ResizeMode != ResizeMode.NoResize));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (sender, e) => SystemCommands.RestoreWindow(this), (sender, e) => e.CanExecute = e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip));
        }
    }
}
