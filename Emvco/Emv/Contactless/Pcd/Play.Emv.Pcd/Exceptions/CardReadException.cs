﻿using System;
using System.Runtime.CompilerServices;

namespace Play.Emv.Pcd.Exceptions;

public class CardReadException : Iso7816Exception
{
    #region Constructor

    public CardReadException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CardReadException), fileName, memberName, lineNumber)} {message}")
    { }

    public CardReadException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CardReadException), fileName, memberName, lineNumber)}", innerException)
    { }

    public CardReadException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CardReadException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}