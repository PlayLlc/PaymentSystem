using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Merchants.Onboarding.Domain.Users
{
    public class UserRole
    {
        #region Instance Values

        public static UserRole Member => new(nameof(Member));

        public static UserRole Administrator => new(nameof(Administrator));

        public string Value { get; }

        #endregion

        #region Constructor

        private UserRole(string value)
        {
            Value = value;
        }

        #endregion
    }
}