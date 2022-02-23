using Play.Codecs;
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

    public void CopyTo(Span<byte> buffer, ref int offset)
    {
        PlayCodec.NumericCodec.Encode(_Value, buffer, ref offset);
    }

    public BitMap Create()
    {
        var result = new BitMap(_Value);
        _Value = 0;

        return result;
    }

    #endregion
}