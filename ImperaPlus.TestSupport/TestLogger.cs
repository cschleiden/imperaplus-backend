using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImperaPlus.Domain;

namespace ImperaPlus.TestSupport
{
    public class TestLogger : Domain.ILogger
    {
        public void Log(LogLevel level, string format, params object[] args)
        {
            Debug.WriteLine(format, args);
        }
    }
}
