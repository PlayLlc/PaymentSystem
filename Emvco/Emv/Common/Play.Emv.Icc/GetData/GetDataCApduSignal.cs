using Play.Ber.Identifiers;
using Play.Core;
using Play.Icc.Messaging.Apdu;
using Play.Icc.Messaging.Apdu.GetData;

namespace Play.Emv.Icc.GetData;

/// <summary>
///     Gets a TLV encoded object that is not encapsulated as an Elementary File Record
/// </summary>
public class GetDataCApduSignal : CApduSignal
{
    #region Constructor

    private GetDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
        parameter2)
    { }

    private GetDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) : base(@class, instruction,
        parameter1, parameter2, le)
    { }

    private GetDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class,
        instruction, parameter1, parameter2, data)
    { }

    private GetDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint? le) : base(
        @class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    /// <param name="tag">
    ///     The <see cref="Tag" /> must be less than or equal to a <see cref="ushort" /> value
    /// </param>
    /// <returns></returns>
    /// <exception cref="Play.Icc.Exceptions.Iso7816Exception"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public static GetDataCApduSignal Create(Tag tag)
    {
        GetDataApduCommand cApdu = GetDataApduCommand.Create(ProprietaryMessageIdentifier._8x, tag);

        return new GetDataCApduSignal(cApdu.GetClass(), cApdu.GetInstruction(), cApdu.GetParameter1(), cApdu.GetParameter2(),
            cApdu.GetData(), cApdu.GetLe());
    }

    #endregion
}