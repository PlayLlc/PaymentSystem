using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Exceptions;

namespace Play.Emv.Kernel.Exceptions
{
    /// <summary>
    /// When the kernel and the card do not have a matching operating mode
    /// </summary>
    public class MagstripeNotSupportedException : PlayException
    {
        #region Constructor

        public MagstripeNotSupportedException(
            string parameterName,
            string message,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(MagstripeNotSupportedException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
        { }

        public MagstripeNotSupportedException(
            string message,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(MagstripeNotSupportedException), fileName, memberName, lineNumber)} {message}")
        { }

        public MagstripeNotSupportedException(
            Exception innerException,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(MagstripeNotSupportedException), fileName, memberName, lineNumber)}", innerException)
        { }

        public MagstripeNotSupportedException(
            string message,
            Exception innerException,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(MagstripeNotSupportedException), fileName, memberName, lineNumber)} {message}", innerException)
        { }

        #endregion
    }
}
