using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Messaging;

namespace Play.Emv.Kernel.Contracts
{
    public readonly record struct Kernel2Channel
    {
        #region Static Metadata

        public static readonly ChannelTypeId Id;

        #endregion

        #region Instance Values

        private readonly ChannelTypeId _Value;

        #endregion

        #region Constructor

        static Kernel2Channel()
        {
            Id = new ChannelTypeId(nameof(Kernel2Channel));
        }

        private Kernel2Channel(ChannelTypeId value)
        {
            _Value = value;
        }

        #endregion

        #region Operator Overrides

        public static explicit operator ChannelTypeId(Kernel2Channel value) => value._Value;

        #endregion
    }
}