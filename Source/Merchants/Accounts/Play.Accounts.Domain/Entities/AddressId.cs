using Play.Domain.Entities;

namespace Play.Accounts.Domain.Entities
{
    public record AddressId : EntityId<string>
    {
        #region Constructor

        public AddressId(string id) : base(id)
        { }

        #endregion

        #region Instance Members

        public static AddressId New()
        {
            return new AddressId(GenerateStringId());
        }

        #endregion
    }
}