using Play.Core.Exceptions;
using Play.Domain.Exceptions;
using System.Runtime.CompilerServices;

namespace Play.Persistence.Gremlin
{
    public class GremlinRepositoryException : RepositoryException
    {
        #region Constructor

        protected GremlinRepositoryException(string message, Exception innerException) : base(message, innerException)
        { }

        protected GremlinRepositoryException(string message) : base(message)
        { }

        public GremlinRepositoryException(
            string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
        { }

        public GremlinRepositoryException(
            string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
        { }

        public GremlinRepositoryException(
            Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
            base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}", innerException)
        { }

        public GremlinRepositoryException(
            string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}",
            innerException)
        { }

        #endregion
    }
}