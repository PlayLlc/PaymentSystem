using Play.Messaging;

namespace Play.Emv.Kernel.Contracts
{
    public readonly record struct KernelChannel
    {
        #region Static Metadata

        public static readonly ChannelTypeId Id;

        #endregion

        #region Instance Values

        private readonly ChannelTypeId _Value;

        #endregion

        #region Constructor

        static KernelChannel()
        {
            Id = new ChannelTypeId(nameof(KernelChannel));
        }

        private KernelChannel(ChannelTypeId value)
        {
            _Value = value;
        }

        #endregion

        #region Operator Overrides

        public static explicit operator ChannelTypeId(KernelChannel value) => value._Value;

        #endregion
    }
}