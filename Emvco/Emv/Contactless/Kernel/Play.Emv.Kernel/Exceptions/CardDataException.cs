using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Exceptions;

namespace Play.Emv.Kernel.Exceptions
{
    public class CardDataException : PlayException
    {
        #region Constructor

        public CardDataException(
            string parameterName,
            string message,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(CardDataException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
        { }

        public CardDataException(
            string message,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(CardDataException), fileName, memberName, lineNumber)} {message}")
        { }

        public CardDataException(
            Exception innerException,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(CardDataException), fileName, memberName, lineNumber)}", innerException)
        { }

        public CardDataException(
            string message,
            Exception innerException,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(CardDataException), fileName, memberName, lineNumber)} {message}", innerException)
        { }

        #endregion
    }
}
