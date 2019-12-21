using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;

namespace VideoPlayerForWpf.Themes
{
    /// <summary>
    /// 自定义对话框。
    /// </summary>
    public class MercuriusDialog : Window
    {
        public MercuriusDialog()
        {
            this.SetValue(Window.StyleProperty, Application.Current.Resources[typeof(MercuriusDialog)]);

            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sender, e) => SystemCommands.CloseWindow(this)));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (sender, e) => SystemCommands.MaximizeWindow(this), (sender, e) => e.CanExecute = e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (sender, e) => SystemCommands.MinimizeWindow(this), (sender, e) => e.CanExecute = this.ResizeMode != ResizeMode.NoResize));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (sender, e) => SystemCommands.RestoreWindow(this), (sender, e) => e.CanExecute = e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip));

            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (Application.Current != null && Application.Current.MainWindow != this)
            {
                this.Owner = Application.Current.MainWindow;
            }
        }

        /// <summary>
        /// 覆盖显示对话框的处理。
        /// </summary>
        /// <returns>关闭对话框返回值</returns>
        public new bool? ShowDialog()
        {
            var border = this.Owner.Template.FindName("overlay", this.Owner) as Rectangle;

            if (border != null)
            {
                border.Visibility = Visibility.Visible;
            }

            return base.ShowDialog();
        }

        /// <summary>
        /// 实现对话框关闭事件。
        /// </summary>
        /// <param name="e">关闭事件参数</param>
        protected override void OnClosed(EventArgs e)
        {
            var border = this.Owner.Template.FindName("overlay", this.Owner) as Rectangle;

            if (border != null)
            {
                border.Visibility = Visibility.Collapsed;
            }

            base.OnClosed(e);
        }
    }
}
