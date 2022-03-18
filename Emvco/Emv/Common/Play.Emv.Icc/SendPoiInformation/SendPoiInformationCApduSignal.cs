using Play.Emv.DataElements;
using Play.Emv.Templates.Requests;

namespace Play.Emv.Icc;

public class SendPoiInformationCApduSignal : CApduSignal
{
    #region Constructor

    protected SendPoiInformationCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction,
     parameter1, parameter2)
    { }

    protected SendPoiInformationCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) :
        base(@class, instruction, parameter1, parameter2, le)
    { }

    protected SendPoiInformationCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    protected SendPoiInformationCApduSignal(
        byte @class,
        byte instruction,
        byte parameter1,
        byte parameter2,
        ReadOnlySpan<byte> data,
        uint? le) : base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public static SendPoiInformationCApduSignal Create(CommandTemplate commandTemplate) =>
        new(0x80, 0x1A, 0, 0, commandTemplate.EncodeTagLengthValue(), 0);

    public static SendPoiInformationCApduSignal Create(DataObjectListResult commandTemplate) =>
        new(0x80, 0x1A, 0, 0, commandTemplate.AsByteArray(), 0);

    #endregion
}