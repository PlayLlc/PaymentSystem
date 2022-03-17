using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Interchange.DataFields;

namespace Play.Interchange.Messages.Builders;

internal class BitMapBuilder
{
    #region Instance Values

    private ulong _Value = 0;

    #endregion

    #region Constructor

    public BitMapBuilder()
    { }

    #endregion

    #region Instance Members

    public void Add(DataFieldId value)
    {
        _Value.SetBit(value);
    }

    /// <summary>
    ///     CopyTo
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="Play.Codecs.Exceptions.CodecParsingException"></exception>
    public void CopyTo(Span<byte> buffer, ref int offset)
    {
        PlayCodec.NumericCodec.Encode(_Value, buffer, ref offset);
    }

    public BitMap Create()
    {
        BitMap? result = new(_Value);
        _Value = 0;

        return result;
    }

    #endregion
}