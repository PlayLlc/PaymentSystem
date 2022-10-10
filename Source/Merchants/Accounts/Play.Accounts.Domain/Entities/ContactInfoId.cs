using Play.Domain.Entities;

namespace Play.Accounts.Domain.Entities
{
    public record ContactInfoId : EntityId<string>
    {
        #region Constructor

        public ContactInfoId(string id) : base(id)
        { }

        #endregion

        #region Instance Members

        public static ContactInfoId New()
        {
            return new ContactInfoId(GenerateStringId());
        }

        #endregion
    }
}