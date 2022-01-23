namespace Play.Icc.Exceptions;

//public class Iso7816Exception : PlayException
//{
//    public Iso7816Exception(string parameterName, string message, [CallerFilePath] string fileName = "",
//        [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
//        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
//    { }

//    public Iso7816Exception(string message, [CallerFilePath] string fileName = "",
//        [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
//        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
//    { }

//    public Iso7816Exception(Exception innerException, [CallerFilePath] string fileName = "",
//        [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
//        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}", innerException)
//    { }

//    public Iso7816Exception(string message, Exception innerException, [CallerFilePath] string fileName = "",
//        [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
//        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}",
//        innerException)
//    { }
//}