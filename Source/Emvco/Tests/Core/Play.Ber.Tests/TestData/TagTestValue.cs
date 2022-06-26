using Play.Codecs;
using Play.Core.Extensions;

namespace Play.Core.Iso8825.Tests.Ber.TagTests.Tags.TestData
{
    public class TagTestValue
    {
        #region Instance Values

        public byte[] Value;

        #endregion

        #region Constructor

        public TagTestValue(byte[] value)
        {
            Value = value;
        }

        #endregion

        #region Instance Members

        public uint GetUInt32() => PlayCodec.UnsignedIntegerCodec.DecodeToUInt32(Value);

        #endregion
    }
}