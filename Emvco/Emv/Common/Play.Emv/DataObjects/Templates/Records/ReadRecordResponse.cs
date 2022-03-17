using System;
using System.Linq;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;

namespace Play.Emv.Templates;

//public class ReadRecordResponse : ReadRecordResponseTemplate
//{
//    #region Instance Values

//    private readonly byte[] _Value;

//    #endregion

//    #region Constructor

//    private ReadRecordResponse(byte[] value)
//    {
//        _Value = value;
//    }

//    #endregion

//    #region Instance Members

//    public TagLengthValue[] GetRecords() => _Codec.DecodeTagLengthValues(_Value.AsMemory());
//    public override Tag[] GetChildTags() => _Codec.DecodeTagLengthValues(_Value.AsSpan()).Select(a => a.GetTag()).ToArray();
//    public override Tag GetTag() => Tag;

//    public override ushort GetValueByteCount(BerCodec codec)
//    {
//        checked
//        {
//            return (ushort) _Value.Length;
//        }
//    }

//    public bool IsEmpty() => !_Value.Any();
//    protected override IEncodeBerDataObjects?[] GetChildren() => throw new NotImplementedException();

//    #endregion

//    #region Serialization

//    public static ReadRecordResponse Decode(ReadOnlySpan<byte> value) => new(_Codec.GetContentOctets(value));
//    public override byte[] EncodeTagLengthValue(BerCodec codec) => throw new NotImplementedException();
//    public override byte[] EncodeValue(BerCodec codec) => _Value;

//    #endregion

//    #region Equality

//    public override bool Equals(object? obj) =>
//        obj is ReadRecordResponseTemplate readRecordResponseTemplate && Equals(readRecordResponseTemplate);

//    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
//    {
//        if (x == null)
//            return y == null;

//        if (y == null)
//            return false;

//        return x.Equals(y);
//    }

//    public bool Equals(ReadRecordResponse x, ReadRecordResponse y) => x.Equals(y);
//    public bool Equals(ReadRecordResponse other) => (GetTag() == other.GetTag()) && _Value.Equals(other._Value);
//    public override bool Equals(ConstructedValue? other) => other is ReadRecordResponse response && Equals(response);

//    public override int GetHashCode()
//    {
//        unchecked
//        {
//            return GetTag().GetHashCode() + _Value.GetHashCode();
//        }
//    }

//    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();

//    #endregion
//}