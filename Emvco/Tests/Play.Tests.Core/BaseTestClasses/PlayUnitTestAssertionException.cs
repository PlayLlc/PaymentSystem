using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Exceptions;

using Xunit;

namespace Play.Tests.Core.BaseTestClasses;

public class PlayUnitTestAssertionException : PlayException
{
    #region Constructor

    public PlayUnitTestAssertionException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"\n\tMessage:  {message}" + $"\n\n\tInnerException", innerException)
    { }

    #endregion
}