using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Exceptions;

namespace Play.Emv.Kernel.Exceptions
{
    public class TerminalDataException : PlayException
    {
        #region Constructor

        public TerminalDataException(
            string parameterName,
            string message,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(TerminalDataException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
        { }

        public TerminalDataException(
            string message,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(TerminalDataException), fileName, memberName, lineNumber)} {message}")
        { }

        public TerminalDataException(
            Exception innerException,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(TerminalDataException), fileName, memberName, lineNumber)}", innerException)
        { }

        public TerminalDataException(
            string message,
            Exception innerException,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(TerminalDataException), fileName, memberName, lineNumber)} {message}", innerException)
        { }

        #endregion
    }
}
