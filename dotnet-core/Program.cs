using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace taskcoachq
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<(FileInfo src, FileInfo dst)> q =
                GetTaskcoachXMLs().Zip(
                    GetSrcCtrlPaths(),
                    (src, dst) => (src, dst));
            foreach(var t in q)
            {
                using(var srcF = t.src.OpenRead())
                using(var dstF = t.dst.OpenWrite())
                {
                    var srcXE = XElement.Load(srcF);
                    var dstXE =
                        GetSourceControllableTskFile(srcXE);
                    dstXE.Save(dstF);
                }
            }
        }

        private static XElement GetSourceControllableTskFile(
            XElement xeIn)
        {
            return xeIn;
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

        public static IEnumerable<FileInfo> GetSrcCtrlPaths(
            string fileExtDefault = ".tsksrcctrl")
        {
            var q =
              from e in GetTaskcoachXMLs()
              let fPath = Path.ChangeExtension(
                e.FullName, fileExtDefault)
              select new FileInfo(fPath);
            return q;
        }
    }
}
