using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSanook.Examples.CloudWatchLog
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            log.Debug("Hello world logging.");
            log.Info("Hello world logging.");
            log.Error("Hello world logging.");
        }
    }
}
