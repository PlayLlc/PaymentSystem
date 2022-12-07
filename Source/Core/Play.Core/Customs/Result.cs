using System;
using System.Linq;

namespace Play.Core;

public class Result<_T> : Result
{
    #region Instance Values

    public _T Value { get; set; }

    #endregion

    #region Constructor

    public Result(_T value, params string[] errors) : base(errors)
    {
        Value = value;
    }

    public Result(_T value)
    {
        Value = value;
        Succeeded = true;
    }

    #endregion
}

public class Result
{
    #region Instance Values

    public bool Succeeded { get; set; }
    public string[] Errors { get; set; } = Array.Empty<string>();

    #endregion

    #region Constructor

    public Result(params string[] errors)
    {
        Succeeded = false;
        Errors = errors.ToArray();
    }

    public Result()
    {
        Succeeded = true;
    }

    #endregion
}