using Play.Messaging;

namespace Play.Emv.Terminal.Contracts
{
    public readonly record struct TerminalChannel
    {
        #region Static Metadata

        public static readonly ChannelTypeId Id;

        #endregion

        #region Instance Values

        private readonly ChannelTypeId _Value;

        #endregion

        #region Constructor

        static TerminalChannel()
        {
            Id = new ChannelTypeId(nameof(TerminalChannel));
        }

        private TerminalChannel(ChannelTypeId value)
        {
            _Value = value;
        }

        #endregion

        #region Operator Overrides

        public static explicit operator ChannelTypeId(TerminalChannel value) => value._Value;

        #endregion
    }
}