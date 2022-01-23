using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.Messaging.Apdu.SelectFile;

namespace Play.Icc.Emv.FileControlInformation;

public class GetFileControlInformationCApduSignal : CApduSignal
{
    #region Constructor

    protected GetFileControlInformationCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) :
        base(@class, instruction, parameter1, parameter2)
    { }

    protected GetFileControlInformationCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) :
        base(@class, instruction, parameter1, parameter2, le)
    { }

    protected GetFileControlInformationCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data)
        : base(@class, instruction, parameter1, parameter2, data)
    { }

    protected GetFileControlInformationCApduSignal(
        byte @class,
        byte instruction,
        byte parameter1,
        byte parameter2,
        ReadOnlySpan<byte> data,
        uint? le) : base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Reads the whole record from the currently selected Elementary File
    /// </summary>
    public static GetFileControlInformationCApduSignal Get(DedicatedFileName dedicatedFileName)
    {
        SelectApduCommand cApdu = SelectApduCommand.DedicatedFile(dedicatedFileName);

        return new GetFileControlInformationCApduSignal(cApdu.GetClass(), cApdu.GetInstruction(), cApdu.GetParameter1(),
                                                        cApdu.GetParameter2(), cApdu.GetData(), cApdu.GetLe());
    }

    /// <summary>
    ///     Reads the whole record from the currently selected Elementary File
    /// </summary>
    public static GetFileControlInformationCApduSignal GetProximityPaymentSystemEnvironment()
    {
        SelectApduCommand cApdu = SelectApduCommand.SelectProximityPaymentSystemEnvironment();

        return new GetFileControlInformationCApduSignal(cApdu.GetClass(), cApdu.GetInstruction(), cApdu.GetParameter1(),
                                                        cApdu.GetParameter2(), cApdu.GetData(), cApdu.GetLe());
    }

    #endregion
}