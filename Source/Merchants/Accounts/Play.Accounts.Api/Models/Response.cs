namespace Play.Accounts.Api.Models
{
    public class Response
    {
        #region Instance Values

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
    }

    public class Response<TReturn> : Response
    {
        #region Instance Values

        public TReturn Object { get; set; }

        #endregion
    }
}