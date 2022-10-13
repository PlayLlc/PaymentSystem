namespace Play.Accounts.Api.Models
{
    public class Result
    {
        #region Instance Values

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
    }

    public class Result<_Return> : Result
    {
        #region Instance Values

        public _Return Object { get; set; }

        #endregion
    }
}