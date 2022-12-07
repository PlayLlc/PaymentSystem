using System;

namespace Play.Core.Exceptions;

/// <summary>
///     This is the base exception for all custom exception thrown in our codebase
/// </summary>
public abstract class PlayException : Exception
{
    #region Constructor

    protected PlayException(string message) : base(message)
    { }

    protected PlayException(string message, Exception innerException) : base(message, innerException)
    { }

    #endregion

    #region Instance Members

    protected static string TraceExceptionMessage(Type exceptionType, string fileName, string methodName, int lineNumber) =>
        $"[An exception of type [{exceptionType.Name}] occurred in [{fileName}] while executing the [{methodName}] method on line {lineNumber}] ";

    #endregion
}