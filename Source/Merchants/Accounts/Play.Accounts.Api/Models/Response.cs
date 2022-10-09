namespace Play.Accounts.Api.Models
{
    public class Response
    {
        #region Instance Values

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
    }

    public class Response<_Return> : Response
    {
        #region Instance Values

        public _Return Object { get; set; }

        #endregion
    }
}