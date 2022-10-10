using System.Collections.Immutable;

using Play.Core;

namespace Play.Accounts.Domain.Enums
{
    public record BusinessTypes : EnumObjectString
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<string, BusinessTypes> _ValueObjectMap;
        public static BusinessTypes SoleProprietorship;
        public static BusinessTypes Partnership;
        public static BusinessTypes LimitedLiability;
        public static BusinessTypes NonProfit;
        public static BusinessTypes Exempt;

        #endregion

        #region Constructor

        private BusinessTypes(string value) : base(value)
        { }

        static BusinessTypes()
        {
            SoleProprietorship = new BusinessTypes(nameof(SoleProprietorship));
            Partnership = new BusinessTypes(nameof(Partnership));
            LimitedLiability = new BusinessTypes(nameof(LimitedLiability));
            NonProfit = new BusinessTypes(nameof(NonProfit));
            Exempt = new BusinessTypes(nameof(Exempt));

            _ValueObjectMap = new Dictionary<string, BusinessTypes>
            {
                {SoleProprietorship, SoleProprietorship},
                {Partnership, Partnership},
                {LimitedLiability, LimitedLiability},
                {NonProfit, NonProfit},
                {Exempt, Exempt}
            }.ToImmutableSortedDictionary();
        }

        #endregion

        #region Instance Members

        public override BusinessTypes[] GetAll()
        {
            return _ValueObjectMap.Values.ToArray();
        }

        public override bool TryGet(string value, out EnumObjectString? result)
        {
            if (_ValueObjectMap.TryGetValue(value, out BusinessTypes? enumResult))
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