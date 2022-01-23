//using System;
//using Play.Core.Codecs;
//using Play.Core.Iso8825.Ber.Contents;

//namespace Play.Core.Iso8825.Tests.Ber.TestData.Formatting
//{
//    public class HexadecimalPrimitive : PrimitiveValue
//    {
//        public static HexadecimalPrimitive Default;
//        private readonly byte _ByteLength;
//        private readonly byte _CharacterLength;

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

//        protected override Codec Codec => Codec.Hexadecimal;

//        public override PrimitiveValue GetNullPrimitive()
//        {
//            return Default;
//        }

//        protected internal override PrimitiveValue Create(ReadOnlySpan<byte> value)
//        {
//            return new HexadecimalPrimitive(value);
//        }
//    }
//}

