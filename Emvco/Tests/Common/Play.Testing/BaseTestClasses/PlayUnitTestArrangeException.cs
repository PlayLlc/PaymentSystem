using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Testing.Infrastructure.BaseTestClasses;

/// <summary>
///     Represents Exceptions that are unrelated to a Test Assertion. These exceptions happen during the test setup phase
///     and not while a unit test is asserting a test condition
/// </summary>
public class PlayUnitTestArrangeException : PlayException
{
    #region Constructor

    public PlayUnitTestArrangeException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"[ARRANGE EXCEPTION] An exception occurred during the 'arrange' section of this unit test" + $"\n\tMessage:  {message}" + $"\n\tTrace: {TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} " + $"\n\n\tInnerException",
             innerException)
    { }

    #endregion
}