using System.Windows;
using System.Windows.Controls;
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
        /// 标题扩展区依赖属性
        /// </summary>
        public Panel TitleExtraArea
        {
            get { return (Panel)GetValue(TitleExtraAreaProperty); }
            set { SetValue(TitleExtraAreaProperty, value); }
        }

        /// <summary>
        /// 标题扩展区对齐方式依赖属性
        /// </summary>
        public HorizontalAlignment TitleExtraAreaAlignment
        {
            get { return (HorizontalAlignment)GetValue(TitleExtraAreaAlignmentProperty); }
            set { SetValue(TitleExtraAreaAlignmentProperty, value); }
        }

        /// <summary>
        /// 注册标题扩展区对齐方式依赖属性
        /// </summary>
        public static readonly DependencyProperty TitleExtraAreaAlignmentProperty = DependencyProperty.Register("TitleExtraAreaAlignment", typeof(HorizontalAlignment), typeof(MercuriusWindow), new PropertyMetadata(HorizontalAlignment.Right));

        /// <summary>
        /// 注册副标题依赖属性
        /// </summary>
        public static readonly DependencyProperty SubTitleProperty = DependencyProperty.Register("SubTitle", typeof(string), typeof(MercuriusWindow), new PropertyMetadata(""));

        /// <summary>
        /// 注册标题扩展区
        /// </summary>
        public static readonly DependencyProperty TitleExtraAreaProperty = DependencyProperty.Register("TitleExtraArea", typeof(Panel), typeof(MercuriusWindow));

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
