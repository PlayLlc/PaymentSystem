using Microsoft.AspNetCore.Identity;

namespace Play.AuthenticationManagement.Identity.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime LastPasswordChangedDate { get; set; }
}
