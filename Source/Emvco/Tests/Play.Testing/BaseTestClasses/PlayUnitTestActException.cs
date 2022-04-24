using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Testing.BaseTestClasses;

/// <summary>
///     Represents Exceptions that are unrelated to a Test Assertion. These exceptions happen during the test setup phase
///     and not while a unit test is asserting a test condition
/// </summary>
public class PlayUnitTestActException : PlayException
{
    #region Constructor

    public PlayUnitTestActException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"[ACT EXCEPTION] An exception occurred during the 'act' section of this unit test"
        + $"\n\tMessage:  {message}"
        + $"\n\tTrace: {TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} "
        + $"\n\n\tInnerException", innerException)
    { }

    #endregion
}