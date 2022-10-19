using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Services;
using Play.Accounts.Persistence.Sql.Entities;

namespace Play.Accounts.Application.Services
{
    public class PasswordHasher : IHashPasswords
    {
        #region Static Metadata

        private static readonly UserIdentity _DefaultUserIdentity = new(new UserDto());

        #endregion

        #region Instance Values

        private readonly IPasswordHasher<UserIdentity> _PasswordHasher;

        #endregion

        #region Constructor

        public PasswordHasher(UserManager<UserIdentity> userManager)
        {
            _PasswordHasher = userManager.PasswordHasher;
        }

        #endregion

        #region Instance Members

        public string GeneratePasswordHash(string password)
        {
            return _PasswordHasher.HashPassword(_DefaultUserIdentity, password);
        }

        #endregion
    }
}