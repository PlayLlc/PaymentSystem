using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Play.AuthenticationManagement.Identity.Models;

namespace Play.AuthenticationManagement.Identity.Data;

internal class DbContext : IdentityDbContext<User>
{
    public DbContext(DbContextOptions options) : base(options)
    {
    }
}
