using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public record class TestPrimitive : DataElement<byte>
{
    public TestPrimitive(byte _Value) : base(_Value)
    {
    }

    public override PrimitiveValue Decode(TagLengthValue value) => throw new NotImplementedException();
    public override PlayEncodingId GetEncodingId() => PlayCodec.BinaryCodec.GetEncodingId();
    public override Tag GetTag() => throw new NotImplementedException();

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);

    public override byte[] EncodeValue(BerCodec codec, int length) => PlayCodec.BinaryCodec.Encode(_Value);

    public override byte[] EncodeValue(BerCodec codec) => PlayCodec.BinaryCodec.Encode(_Value);
}
