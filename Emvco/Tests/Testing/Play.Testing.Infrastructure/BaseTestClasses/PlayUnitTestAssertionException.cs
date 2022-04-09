using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Testing.Infrastructure.BaseTestClasses;

public class PlayUnitTestAssertionException : PlayException
{
    #region Constructor

    public PlayUnitTestAssertionException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"\n\tMessage:  {message}" + $"\n\n\tInnerException", innerException)
    { }

    #endregion
}