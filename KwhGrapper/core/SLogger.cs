using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwhGrapper.core
{
    public class SLogger
    {
        private SLogger() { }

        static SLogger() { }

        public static SLogger Instance = new SLogger();

        
        public void Log(string msg,Exception ex = null)
        {

        }


        public void LogDebug(string msg, Exception ex = null)
        {

        }

        public void LogErr(string msg, Exception ex = null)
        {

        }

        public void LogWarn(string msg, Exception ex = null)
        {

        }

        public void LogInfo(string msg, Exception ex = null)
        {

        }
    }
}
