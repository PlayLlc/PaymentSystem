using Play.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Enums;
using Play.Core;

namespace Play.Accounts.Domain.ValueObjects
{
    public record UserRole : ValueObject<string>
    {
        #region Constructor

        /// <exception cref="ValueObjectException"></exception>
        public UserRole(string value) : base(value[..5])
        {
            if (!IsValid(value))
                throw new ValueObjectException($"The {nameof(UserRole)} provided was invalid: [{value}]");
        }

        #endregion

        #region Instance Members

        public static bool IsValid(string value)
        {
            return UserRoles.Empty.TryGet(value, out EnumObjectString? result);
        }

        #endregion
    }
}