namespace Play.Mvc.Swagger;

public class SwaggerConfiguration
{
    #region Instance Values

    public string[] Versions { get; set; } = Array.Empty<string>();
    public string ApplicationTitle { get; set; } = string.Empty;
    public string ApplicationDescription { get; set; } = string.Empty;

    #endregion
}