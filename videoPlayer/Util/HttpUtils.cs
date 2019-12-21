using log4net;
using Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using videoPlayer.model;

namespace videoPlayer.Util
{
    class HttpUtils
    {

        private static ILog logger = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void HttpGetByThread (RunConfig cfg) {

            Thread t = new Thread(new ParameterizedThreadStart(HttpGet));//创建了线程还未开启
            t.Start(cfg);

        }
        public static void HttpGet (Object obj)
        {
            RunConfig cfg = (RunConfig)obj;
            string url;
            switch (cfg.urlIndex)
            {
                case 1:
                    url = cfg.callUrl;
                    break;
                case 2:
                    url = cfg.nextCallUrl;
                    break;
                default:
                    url = cfg.pcUrl + "?winPattern=" + cfg.winPatternEnums + "&localPort=" + cfg.localPort + "&processId=" + cfg.processId;
                    break;
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                logger.Info("请求{" + url + "}的结果是：" + retString);
            }
            catch (Exception e) {
                logger.Info("请求{" + url + "}失败：", e);
            }
        }
    }
}
