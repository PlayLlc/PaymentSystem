namespace Play.Inventory.Api.Models;

public class ErrorViewModel
{
    #region Instance Values

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string? RequestId { get; set; }

    #endregion
}