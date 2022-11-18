namespace Play.Restful.Clients
{
    public class ApiConfiguration
    {
        #region Instance Values

        public string BasePath { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }

        #endregion
    }
}