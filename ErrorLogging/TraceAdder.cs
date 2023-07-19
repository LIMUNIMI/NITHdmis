using System.Diagnostics;

namespace NITHdmis.ErrorLogging
{
    public static class TraceAdder
    {
        public static void AddTrace()
        {
            Trace.TraceInformation("Trace Information");
            Trace.TraceError("Trace Error");
            Trace.TraceWarning("Trace Warning");
            Trace.Listeners.Add(new TextWriterTraceListener("Error.log"));
        }
    }
}
