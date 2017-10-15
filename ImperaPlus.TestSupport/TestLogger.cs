using System.Diagnostics;
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
