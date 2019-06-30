<Query Kind="Program" />

void Main()
{
  DumpTskFiles();
}

public static void DumpTskFiles()
{
  GetRecurrenceUnits().Distinct()
                      .OrderBy(e => e)
                      .Dump();
}

public static IEnumerable<string> GetRecurrenceUnits()
{
  var q =
    from tskfp in GetTaskcoachXMLStreams()
    from tsk in tskfp.Descendants("task")
    from rec in tsk.Descendants("recurrence")
    from unit in rec.Attributes("unit")
    select unit.Value;
  return q;
}

public static IEnumerable<XElement>
  GetTasksFilterByRecurrence()
{
  var q =
    from tskfp in GetTaskcoachXMLStreams()
    from tsk in tskfp.Descendants("task")
    let recurrence = tsk.Elements("recurrence").SingleOrDefault()
    let excludeRecurrences = new[] {"daily"}
    where false 
      || recurrence == null
      || !excludeRecurrences.Contains(
           recurrence.Attribute("unit").Value)
    select tsk;
  return q;
}

public static IEnumerable<XElement> GetTaskcoachXMLStreams()
{
  return from xml in GetTaskcoachXMLs()
         select XElement.Load(xml.FullName);
}

// Name argument is case-insensitive.
public static IEnumerable<FileInfo> GetTaskcoachXMLs(
  string envVarName = "TASKCOACH_FP")
{
  var TASKCOACH_XML_ENV =
    Environment.GetEnvironmentVariable(envVarName);
  var q =
    from fp in TASKCOACH_XML_ENV.Split(
      new[] {Path.PathSeparator}, StringSplitOptions.None)
    select new FileInfo(fp);
  return q;
}