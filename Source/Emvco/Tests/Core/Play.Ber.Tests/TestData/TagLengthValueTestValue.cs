using System.Linq;

namespace Play.Core.Iso8825.Tests.Ber.TestData
{
    public class TagLengthValueTestValue
    {
        #region Instance Values

        public byte[] Length;
        public byte[] Tag;
        public byte[] Value;
        public byte[] TagLengthValue => Tag.Concat(Length).Concat(Value).ToArray();

        #endregion

        #region Constructor

        public TagLengthValueTestValue(byte[] tag, byte[] length, byte[] value)
        {
            Tag = tag;
            Length = length;
            Value = value;
        }

        #endregion
    }
}