using Play.Codecs;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Interchange.DataFields;

namespace Play.Interchange.Messages.Builders;

public record BitMap
{
    #region Static Metadata

    public const byte ByteCount = 8;

    #endregion

    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    internal BitMap(ulong value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public int GetDataFieldCount() => _Value.GetSetBitCount();

    /// <summary>
    /// GetDataFieldIds
    /// </summary>
    /// <param name="buffer"></param>
    /// <exception cref="System.IndexOutOfRangeException"></exception>
    public void GetDataFieldIds(Span<ulong> buffer)
    {
        if (buffer.Length != GetDataFieldCount())
            throw new IndexOutOfRangeException();

        for (byte i = 0, j = 0; j < Specs.Integer.UInt64.BitCount; j++)
        {
            if (_Value.IsBitSet(j))
                buffer[i++] = j;
        }
    }

    public SortedSet<DataFieldId> GetDataFieldIds()
    {
        SortedSet<DataFieldId> result = new();

        for (byte i = 0; i < Specs.Integer.UInt64.BitCount; i++)
        {
            if (_Value.IsBitSet(i))
                result.Add(i);
        }

        return result;
    }

    /// <summary>
    /// CopyTo
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="Play.Codecs.Exceptions.InternalPlayEncodingException"></exception>
    public void CopyTo(Span<byte> buffer, ref int offset)
    {
        PlayCodec.NumericCodec.Encode(_Value, buffer, ref offset);
    }

    public bool IsDataFieldPresent(DataFieldId dataFieldId) => _Value.IsBitSet(dataFieldId);

    #endregion
}