using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Domain;

public class BusinessRuleValidationException : PlayException
{
    #region Constructor

    protected BusinessRuleValidationException(string message, Exception innerException) : base(message, innerException)
    { }

    protected BusinessRuleValidationException(string message) : base(message)
    { }

    public BusinessRuleValidationException(
        IBusinessRule businessRule, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base(
            $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}. The {nameof(IBusinessRule)}: [{businessRule.GetType().FullName}] was broken. {businessRule.Message}")
    { }

    public BusinessRuleValidationException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
    { }

    public BusinessRuleValidationException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}", innerException)
    { }

    public BusinessRuleValidationException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}