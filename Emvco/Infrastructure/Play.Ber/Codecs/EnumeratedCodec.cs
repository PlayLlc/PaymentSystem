//using System;

//using Play.Core.Throwers;

//namespace Play.Core.Ber.Contents
//{
//    public sealed class EnumeratedCodec : BerPrimitiveCodec
//    {
//        private static readonly string Identifier = typeof(EnumeratedCodec).FullName;

//        public override string GetIdentifier() => Identifier;

//        #region Validation

//        protected override void Validate(ReadOnlySpan<byte> value)
//        {
//            Check.ForExactLength(value, 1, nameof(value));
//        }

//        public override bool IsValid(ReadOnlySpan<byte> value)
//        {
//            return value.Length == 1;
//        }

//        #endregion Validation

//        #region EncodeTagLengthValue

//        public override byte[] EncodeTagLengthValue(dynamic value)
//        {
//            return EncodeTagLengthValue(value);
//        }

//        public byte[] EncodeTagLengthValue(bool value)
//        {
//            return value ? _DefaultEncodedTrue : _DefaultEncodedFalse;
//        }

//        #endregion EncodeTagLengthValue

//        #region Decode

//        public override dynamic Decode(ReadOnlySpan<byte> value)
//        {
//            Validate(value);

//            return value[0] != 0;
//        }

//        public EnumObject<T> Decode<T>(ReadOnlySpan<byte> value, EnumObject<T> enumObject) where T : unmanaged
//        {
//            Type t = typeof(T);
//            if(enumObject._Value is int)

//            Validate(value);

//            return value[0] != 0;
//        }

//        #endregion Decode

//    }
//}

