using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Exceptions;

namespace Play.Domain.ValueObjects
{
    public class ValueObjectException : PlayException
    {
        #region Constructor

        protected ValueObjectException(string message, Exception innerException) : base(message, innerException)
        { }

        protected ValueObjectException(string message) : base(message)
        { }

        public ValueObjectException(
            string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
        { }

        public ValueObjectException(
            string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
        { }

        public ValueObjectException(
            Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
            base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}", innerException)
        { }

        public ValueObjectException(
            string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}",
            innerException)
        { }

        #endregion
    }
}