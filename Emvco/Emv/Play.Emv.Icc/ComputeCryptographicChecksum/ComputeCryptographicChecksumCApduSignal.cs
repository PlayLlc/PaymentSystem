using Play.Ber.Emv.DataObjects;

namespace Play.Icc.Emv.ComputeCryptographicChecksum;

public class ComputeCryptographicChecksumCApduSignal : CApduSignal
{
    #region Constructor

    public ComputeCryptographicChecksumCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) :
        base(@class, instruction, parameter1, parameter2)
    { }

    public ComputeCryptographicChecksumCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) :
        base(@class, instruction, parameter1, parameter2, le)
    { }

    public ComputeCryptographicChecksumCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data)
        : base(@class, instruction, parameter1, parameter2, data)
    { }

    public ComputeCryptographicChecksumCApduSignal(
        byte @class,
        byte instruction,
        byte parameter1,
        byte parameter2,
        ReadOnlySpan<byte> data,
        uint le) : base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     The COMPUTE CRYPTOGRAPHIC CHECKSUM command initiates the computation of the dynamic CVC3 on the Card.
    /// </summary>
    /// <param name="unpredictableNumberDataObjectListResult">
    ///     The result from an Unpredictable Number Data Object List
    /// </param>
    /// <returns></returns>
    public static ComputeCryptographicChecksumCApduSignal Create(DataObjectListResult unpredictableNumberDataObjectListResult) =>
        new ComputeCryptographicChecksumCApduSignal(new Class(Messaging.Apdu.SecureMessaging.Proprietary),
                                                    Instruction.ComputeCryptographicChecksum, 0x8E, 0x80,
                                                    unpredictableNumberDataObjectListResult.AsCommandTemplate().EncodeValue(), 0);

    #endregion
}