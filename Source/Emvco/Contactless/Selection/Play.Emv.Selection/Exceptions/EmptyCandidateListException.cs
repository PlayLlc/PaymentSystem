﻿using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Selection.Exceptions;

/// <summary>
///     When Entry Point processing encounters an empty candidate list
/// </summary>
/// <remarks>This error is logically similar to a <see cref="Level2Error.MagstripeNotSupported" />  </remarks>
public class EmptyCandidateListException : CodecParsingException
{
    #region Constructor

    public EmptyCandidateListException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(EmptyCandidateListException), fileName, memberName, lineNumber)} {message}")
    { }

    public EmptyCandidateListException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(EmptyCandidateListException), fileName, memberName, lineNumber)}", innerException)
    { }

    public EmptyCandidateListException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(EmptyCandidateListException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}