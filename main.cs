<Query Kind="Program" />

void Main()
{
  DumpTskFiles();
}

public static void DumpTskFiles()
{
  var q =
    from x in GetTaskcoachXMLStreams()
    select x;
    q.Dump();
}

public static IEnumerable<XElement> GetTaskcoachXMLStreams()
{
	return from xml in GetTaskcoachXMLs()
			   select XElement.Load(xml.FullName);
}

// Name argument is case-insensitive.
public static IEnumerable<FileInfo> GetTaskcoachXMLs()
{
  var TASKCOACH_XML_ENV =
    Environment.GetEnvironmentVariable("TASKCOACH_FP");
  var q =
    from fp in TASKCOACH_XML_ENV.Split(
      new[] {Path.PathSeparator}, StringSplitOptions.None)
    select new FileInfo(fp);
  return q;
}