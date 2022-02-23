using Play.Codecs;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Interchange.DataFields;

namespace Play.Interchange.Messages.Builders;

/// <summary>
///     In ISO 8583, a bitmap is a field or subfield within a message, which indicates whether other data elements or data
///     element sub-fields are present elsewhere in the message. The bitmap may be represented as 8 bytes of binary data or
///     as 16 hexadecimal characters (0–9, A–F) in the AsciiCodec or EBCDIC character sets. A message will contain at least one
///     bitmap, called the primary bitmap, which indicates data elements 1 to 64 are present.
/// </summary>
//internal class BitmapBuilder
//{
//    #region Static Metadata

//    private const byte _ByteLength = 8;
//    private const byte _MaxValue = 128;

//    #endregion

//    #region Instance Values

//    private readonly ulong _SecondaryBitmap = 0;
//    private ulong _PrimaryBitmap = 0;

//    #endregion

//    #region Instance Members

//    public void Set(InterchangeDataField dataField)
//    {
//        _PrimaryBitmap = 0;

//        var bitPosition = (byte) dataField.GetDataFieldId();

//        if (bitPosition <= _MaxValue)
//            _PrimaryBitmap.SetBit(bitPosition);
//        else
//            _SecondaryBitmap.SetBit((byte) (bitPosition / 64));
//    }

//    #endregion
//}



public class BitMapBuilder
{
    private ulong _Value;

    public void Add(DataFieldId value)
    {
        _Value.SetBit(value);
    }

    public void CopyTo(Span<byte> buffer, ref int offset)
    {
        PlayCodec.NumericCodec.Encode(_Value, buffer, ref offset);
    }

    public BitMap Create()
    {
        return new BitMap(_Value);
    }
}

public record BitMap
{
    private readonly ulong _Value;

    internal BitMap(ulong value)
    {
        _Value = value;
    }

    public int GetDataFieldCount() => _Value.GetSetBitCount();

    public void GetDataFieldIds(Span<ulong> buffer)
    {
        if (buffer.Length != GetDataFieldCount())
            throw new IndexOutOfRangeException();
          
        for (byte i = 0, j = 0; j < Specs.Integer.UInt64.BitCount; j++)
        {

            if (_Value.IsBitSet(j))
            {
                buffer[i++] = j;
            }
        } 
    }

    public SortedSet<DataFieldId> GetDataFieldIds()
    {
        SortedSet<DataFieldId> result = new();

        for (byte i = 0; i < Specs.Integer.UInt64.BitCount; i++)
        {

            if (_Value.IsBitSet(i))
            {
                result.Add(i);
            }
        }

        return result;
    }

    public void CopyTo(Span<byte> buffer, ref int offset)
    {
        PlayCodec.NumericCodec.Encode(_Value, buffer, ref offset);
    }

    public bool IsDataFieldPresent(DataFieldId dataFieldId)
    {
        return _Value.IsBitSet(dataFieldId);
    }

}