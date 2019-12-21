using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logger
{
    public class LogHelper
    {
        public static ILog GetLogger(Type type)
        {
            var log = LogManager.GetLogger(type);
            return log;
        }
       
    }
}
