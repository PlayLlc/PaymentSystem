using Play.Core;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Domain.Enums
{
    public record UserRoles : EnumObjectString
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<string, UserRoles> _ValueObjectMap;
        public static readonly UserRoles Empty;
        public static readonly UserRoles SuperAdmin;
        public static readonly UserRoles Administrator;
        public static readonly UserRoles SalesAssociate;

        #endregion

        #region Constructor

        private UserRoles(string value) : base(value)
        { }

        static UserRoles()
        {
            Empty = new UserRoles("");
            SuperAdmin = new UserRoles(nameof(SuperAdmin));
            Administrator = new UserRoles(nameof(Administrator));
            SalesAssociate = new UserRoles(nameof(SalesAssociate));
            _ValueObjectMap = new Dictionary<string, UserRoles>
            {
                {SuperAdmin, SuperAdmin},
                {Administrator, Administrator},
                {SalesAssociate, SalesAssociate}
            }.ToImmutableSortedDictionary();
        }

        #endregion

        #region Instance Members

        public override UserRoles[] GetAll()
        {
            return _ValueObjectMap.Values.ToArray();
        }

        public UserRoles Get(string value)
        {
            return _ValueObjectMap[value];
        }

        public override bool TryGet(string value, out EnumObjectString? result)
        {
            if (_ValueObjectMap.TryGetValue(value, out UserRoles? enumResult))
            {
                result = enumResult;

                return true;
            }

            result = null;

            return false;
        }

        #endregion
    }
}