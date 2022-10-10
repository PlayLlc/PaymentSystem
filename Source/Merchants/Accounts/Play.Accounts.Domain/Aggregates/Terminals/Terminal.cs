using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Accounts.Domain.Aggregates.Users;
using Play.Domain;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates.Terminals
{
    public class Terminal : Aggregate<string>
    {
        #region Instance Values

        private readonly TerminalId _Id;
        private readonly MerchantId _MerchantId;
        private readonly UserId _UserId;
        private readonly string _DeviceId;

        #endregion

        #region Constructor

        private Terminal(TerminalId id, MerchantId merchantId, UserId userId, string deviceId)
        {
            _Id = id;
            _MerchantId = merchantId;
            _UserId = userId;
            _DeviceId = deviceId;

            // Entity Framework only
        }

        #endregion

        #region Instance Members

        public static Terminal CreateNewTerminal(MerchantId merchantId, UserId userId, string deviceId)
        {
            // Rules
            return new Terminal(TerminalId.New(), merchantId, userId, deviceId);

            // Publish Domain Event
        }

        public override IDto AsDto()
        {
            return new TerminalDto
            {
                Id = _Id.Id,
                MerchantId = _MerchantId,
                UserId = _UserId,
                DeviceId = _DeviceId
            };
        }

        public override TerminalId GetId()
        {
            return (TerminalId) _Id;
        }

        #endregion
    }
}