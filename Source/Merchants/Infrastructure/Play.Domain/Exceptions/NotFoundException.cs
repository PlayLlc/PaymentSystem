using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Domain.Exceptions;

public class NotFoundException : PlayException
{
    #region Constructor

    protected NotFoundException(string message, Exception innerException) : base(message, innerException)
    { }

    protected NotFoundException(string message) : base(message)
    { }

    public NotFoundException(
        Type entityType, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(NotFoundException), fileName, memberName, lineNumber)}. The {entityType.Name}")
    { }

    #endregion
}