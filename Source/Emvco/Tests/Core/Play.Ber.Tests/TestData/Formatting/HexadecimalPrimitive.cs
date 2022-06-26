//using System;

//using Play.Ber.DataObjects;
//using Play.Core.Codecs;
//using Play.Core.Iso8825.Ber.Contents;

//namespace Play.Core.Iso8825.Tests.Ber.TestData.Formatting
//{
//    public class HexadecimalPrimitive : PrimitiveValue
//    {
//        #region Static Metadata

//        public static HexadecimalPrimitive Default;

//        #endregion

//        #region Instance Values

//        private readonly byte _ByteLength;
//        private readonly byte _CharacterLength;
//        protected override Codec Codec => Codec.Hexadecimal;

//        #endregion

//        #region Constructor

//        static HexadecimalPrimitive()
//        {
//            Default = new HexadecimalPrimitive();
//        }

//        private HexadecimalPrimitive()
//        {
//            _ByteLength = 0;
//            _CharacterLength = 0;
//        }

//        public HexadecimalPrimitive(ReadOnlySpan<byte> value) : base(value)
//        {
//            Validate(value);
//            _ByteLength = (byte) _Value.Length;
//            _CharacterLength = (byte) _Value.Length;
//        }

//        #endregion

//        #region Instance Members

//        public override PrimitiveValue GetNullPrimitive() => Default;
//        protected internal override PrimitiveValue Create(ReadOnlySpan<byte> value) => new HexadecimalPrimitive(value);

//        #endregion
//    }
//}

