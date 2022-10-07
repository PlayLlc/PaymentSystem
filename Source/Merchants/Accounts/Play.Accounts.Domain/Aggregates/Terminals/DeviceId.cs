using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.Aggregates.Terminals
{
    public record DeviceId : ValueObject<string>
    {
        #region Constructor

        public DeviceId(string value) : base(value)
        {
            // Validate that the device id is valid
        }

        #endregion
    }
}