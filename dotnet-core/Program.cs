using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace taskcoachq
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(var x in GetTaskcoachXMLs())
            {
                Console.WriteLine(x);
            }
        }

        public static IEnumerable<FileInfo> GetTaskcoachXMLs(
            string envVarName = "TASKCOACH_FP",
            string tskExtDefault = ".tsk")
        {
        var TASKCOACH_XML_ENV =
            Environment.GetEnvironmentVariable(envVarName);
        if(string.IsNullOrEmpty(TASKCOACH_XML_ENV))
        {
            var msg = $"Environment variable `{envVarName}` undefined";
            throw new ArgumentException(msg);
        }

        var q =
            from fp in TASKCOACH_XML_ENV.Split(
              new[] {Path.PathSeparator}, StringSplitOptions.None)
            let _fp = new FileInfo(fp)
            where true
              && _fp.Exists
              && _fp.Extension.ToLower() == tskExtDefault
            select _fp;
        return q;
        }
    }
}
