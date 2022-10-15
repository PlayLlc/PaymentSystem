using Play.Accounts.Contracts.Dtos;
using Play.Domain;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates.Terminals
{
    public class Terminal : Aggregate<string>
    {
        #region Instance Values

        private readonly string _Id;
        private readonly string _MerchantId;
        private readonly string _UserId;
        private readonly string _DeviceId;

        #endregion

        #region Constructor

        private Terminal(string id, string merchantId, string userId, string deviceId)
        {
            _Id = id;
            _MerchantId = merchantId;
            _UserId = userId;
            _DeviceId = deviceId;

            // Entity Framework only
        }

        #endregion

        #region Instance Members

        public static Terminal CreateNewTerminal(string merchantId, string userId, string deviceId)
        {
            // Rules
            return new Terminal(GenerateSimpleStringId(), merchantId, userId, deviceId);

            // Publish Domain Event
        }

        public override IDto AsDto()
        {
            return new TerminalDto
            {
                Id = _Id,
                MerchantId = _MerchantId,
                UserId = _UserId,
                DeviceId = _DeviceId
            };
        }

        public override string GetId()
        {
            return _Id;
        }

        #endregion
    }
}