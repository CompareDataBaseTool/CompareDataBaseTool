using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CompareDataBaseTool
{
    public class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        public static void WriteLog(string info)
        {
            loginfo.Info(info);
        }
    }
}
