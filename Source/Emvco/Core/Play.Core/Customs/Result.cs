using System;
using System.Linq;

namespace Play.Core
{
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
}