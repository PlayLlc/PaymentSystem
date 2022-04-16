using Play.Messaging;

namespace Play.Emv.Reader.Contracts
{
    public readonly record struct ReaderChannel
    {
        #region Static Metadata

        public static readonly ChannelTypeId Id;

        #endregion

        #region Instance Values

        private readonly ChannelTypeId _Value;

        #endregion

        #region Constructor

        static ReaderChannel()
        {
            Id = new ChannelTypeId(nameof(ReaderChannel));
        }

        private ReaderChannel(ChannelTypeId value)
        {
            _Value = value;
        }

        #endregion

        #region Operator Overrides

        public static explicit operator ChannelTypeId(ReaderChannel value) => value._Value;

        #endregion
    }
}