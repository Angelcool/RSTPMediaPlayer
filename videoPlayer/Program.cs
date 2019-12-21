using System;
using log4net;
using Logger;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using videoPlayer.eunm;
using videoPlayer.model;
using System.Configuration;

namespace videoPlayer
{
    static class Program
    {
        private static ILog logger = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 默认的tcp端口
        /// </summary>
        private static int defaultPort = 2333;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            logger.Info("启动参数是：" + String.Join("，", args));
            RunConfig cfg = getCfg(args);

            ///防止程序重复启动
            Process[] processes = Process.GetProcessesByName("videoPlayer");
            if (processes.Length >= 2)
            {
                Console.WriteLine("检测到程序启动中！");
                Process.GetCurrentProcess().Close();
                //return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Run(new frmMain(cfg));
        }

        /// <summary>
        /// 解析要运行的模式
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static RunConfig getCfg(string[] args)
        {
            RunConfig cfg = new RunConfig();


            //cfg.winPatternEnums = WinPatternEnums.CALL;
            //cfg.winPatternEnums = WinPatternEnums.SPLIT;
            cfg.winPatternEnums = WinPatternEnums.PREVIEW;
            //cfg.winPatternEnums = WinPatternEnums.PREVIEW_VERTICAL;
            //cfg.winPatternEnums = WinPatternEnums.CRUISE;
            cfg.pcUrl = "http://127.0.0.1:8099//video/registerPlayer";
            cfg.callUrl = "http://127.0.0.1:8099//video/startRollCall";
            cfg.nextCallUrl = "http://127.0.0.1:8099//video/startRollCallNext";

            cfg.localPort = defaultPort;
            

            try
            {
                cfg.isShowHistoryList = Convert.ToBoolean(ConfigurationManager.AppSettings["showHistoryList"]);
                cfg.winPatternEnums = (WinPatternEnums)Enum.Parse(typeof(WinPatternEnums), args[0]);
                cfg.pcUrl = args[1];
            } catch (Exception e) {
                logger.Error("初始化参数失败,使用默认参数：", e);
            }




            return cfg;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error("非UI线程异常：" + e.ExceptionObject.ToString());
            //throw new NotImplementedException();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            logger.Error("UI线程异常：" + e.Exception.Message);
            //throw new NotImplementedException();
        }
    }
}
