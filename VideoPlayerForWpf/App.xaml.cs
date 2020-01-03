using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Windows;
using System.Windows.Threading;
using Vlc.DotNet.Wpf;
using Vlc.DotNet.Core;
using log4net;
using Logger;

namespace VideoPlayerForWpf
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 
        /// </summary>
        public static ILog logger = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        private HttpSelfHostServer _server;
        private HttpSelfHostConfiguration _config;
        //单实例模式
        private Mutex mutex;

        /// <summary>
        /// 
        /// </summary>
        public App()
        {
            //获取本地地址绑定端口
            Uri urlBase = new UriBuilder(ConfigurationManager.AppSettings.Get("InternalUrl") ?? "http://127.0.0.1:8080").Uri;
            _config = new HttpSelfHostConfiguration(urlBase);
            _config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            _config.TransferMode = TransferMode.Streamed;
            //_config.MaxBufferSize = int.MaxValue;
            //_config.MaxReceivedMessageSize = long.MaxValue;
            //清除xml格式数据 返回json格式
            _config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //添加全局模型验证
            _config.Filters.Add(new ModelValidationFilterAttribute());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            bool startupFlag = false;
            try
            {
                mutex = new Mutex(true, "VideoPlayerForWpf", out startupFlag);
                if (!startupFlag)
                {
                    MessageBox.Show("程序已经启动!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Environment.Exit(0);
                }
                else
                {
                    //注册api容器需要使用
                    //_config.DependencyResolver = new AutofacWebApiDependencyResolver(AutofacResolver.Container);
                    //注册MVC容器
                    //DependencyResolver.SetResolver(new AutofacDependencyResolver(AutofacResolver.Container));
                    //if (hasClientTest)
                    //{
                    //_config.MessageHandlers.Add(new MessageHandler());
                    //}

                    _server = new HttpSelfHostServer(_config);
                    _server.OpenAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                logger.Error("启动时发生错误", ex);
            }
            finally
            {
                base.OnStartup(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            _server.CloseAsync().Wait();
            _server.Dispose();

            base.OnExit(e);
        }

        /// <summary>
        /// 加载程序集到内存中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVlcControlNeedsLibDirectory()
        {
            var appPath = AppDomain.CurrentDomain.BaseDirectory;
            //VlcContext.LibVlcDllsPath = appPath + @"VLC\";
            ////Set the vlc plugins directory path
            //VlcContext.LibVlcPluginsPath = appPath + @"plugins\";
            ////Set the startup options
            //VlcContext.StartupOptions.IgnoreConfig = true;
            //VlcContext.StartupOptions.LogOptions.LogInFile = false;
            //VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = false;
            //VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.None;
            ////Initialize the VlcContext
            //VlcContext.Initialize();
        }

        /// <summary>
        /// 全局异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            logger.Error("启动时发生错误", e.Exception);
            e.Handled = true;
        }
    }
}
