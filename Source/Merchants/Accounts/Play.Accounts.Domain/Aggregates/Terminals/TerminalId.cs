using Play.Domain.Entities;

namespace Play.Accounts.Domain.Aggregates.Terminals
{
    public record TerminalId : EntityId<string>
    {
        #region Constructor

        public TerminalId(string id) : base(id)
        { }

        #endregion

        #region Instance Members

        public static TerminalId New()
        {
            return new TerminalId(GenerateStringId());
        }

        #endregion
    }
}