using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs.Exceptions;

namespace Play.Emv.Selection.Exceptions
{

    public class EmptyCandidateListException : CodecParsingException
    {
        #region Constructor

        public EmptyCandidateListException(
            string message,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(EmptyCandidateListException), fileName, memberName, lineNumber)} {message}")
        { }

        public EmptyCandidateListException(
            Exception innerException,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(EmptyCandidateListException), fileName, memberName, lineNumber)}", innerException)
        { }

        public EmptyCandidateListException(
            string message,
            Exception innerException,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) : base(
            $"{TraceExceptionMessage(typeof(EmptyCandidateListException), fileName, memberName, lineNumber)} {message}", innerException)
        { }

        #endregion
    }
}
