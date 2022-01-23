using Play.Ber.Emv.DataObjects;
using Play.Core.Extensions;
using Play.Icc.Messaging.Apdu;

namespace Play.Icc.Emv.GenerateApplicationCryptogram;

public class GenerateApplicationCryptogramCApduSignal : CApduSignal
{
    #region Constructor

    protected GenerateApplicationCryptogramCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) :
        base(@class, instruction, parameter1, parameter2)
    { }

    protected GenerateApplicationCryptogramCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) :
        base(@class, instruction, parameter1, parameter2, le)
    { }

    protected GenerateApplicationCryptogramCApduSignal(
        byte @class,
        byte instruction,
        byte parameter1,
        byte parameter2,
        ReadOnlySpan<byte> data) : base(@class, instruction, parameter1, parameter2, data)
    { }

    protected GenerateApplicationCryptogramCApduSignal(
        byte @class,
        byte instruction,
        byte parameter1,
        byte parameter2,
        ReadOnlySpan<byte> data,
        uint? le) : base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public static GenerateApplicationCryptogramCApduSignal Create(
        CryptogramType cryptogramType,
        bool isCdaRequested,
        DataObjectListResult cardRiskManagementDataObjectListResult)
    {
        return Create(cryptogramType, isCdaRequested, cardRiskManagementDataObjectListResult.AsCommandTemplate());
    }

    public static GenerateApplicationCryptogramCApduSignal Create(
        CryptogramType cryptogramType,
        bool isCdaRequested,
        DataObjectListResult cardRiskManagementDataObjectListResult,
        DataObjectListResult dataStorageDataObjectListResult)
    {
        CommandTemplate? commandTemplate = new(cardRiskManagementDataObjectListResult.AsByteArray()
                                                   .AsSpan()
                                                   .ConcatArrays(dataStorageDataObjectListResult.AsByteArray()));

        return Create(cryptogramType, isCdaRequested, commandTemplate);
    }

    private static GenerateApplicationCryptogramCApduSignal Create(
        CryptogramType cryptogramType,
        bool isCdaRequested,
        CommandTemplate commandTemplate)
    {
        if (cryptogramType == CryptogramType.TransactionCryptogram)
            return CreateTc(isCdaRequested, commandTemplate);

        if (cryptogramType == CryptogramType.ApplicationAuthenticationCryptogram)
            return CreateAac(commandTemplate);

        if (cryptogramType == CryptogramType.AuthorizationRequestCryptogram)
            return CreateArqc(isCdaRequested, commandTemplate);

        throw new ArgumentOutOfRangeException(nameof(cryptogramType),
                                              $"The {nameof(CryptogramType)} value: {cryptogramType} could not be recognized");
    }

    /// <summary>
    ///     Creates a Command APDU to request a Application Authentication Cryptogram
    /// </summary>
    /// <param name="transactionRelatedData">
    ///     The data field of the command message contains CDOL1 Related Data coded  according to CDOL1 following the
    ///     rules defined in Book 3 section 4.1.4. In the case of IDS data writing, the data field of the command message
    ///     is a concatenation of CDOL1 Related Data followed by DSDOL related data coded according to DSDOL following the
    ///     rules defined in section 4.1.4.
    /// </param>
    /// <returns></returns>
    private static GenerateApplicationCryptogramCApduSignal CreateAac(CommandTemplate transactionRelatedData)
    {
        return new(new Class(Messaging.Apdu.SecureMessaging.NotRecognized, LogicalChannel.BasicChannel), Instruction.CardBlock, 0, 0,
                   transactionRelatedData.EncodeValue());
    }

    /// <summary>
    ///     Creates a Command APDU to request a Application Request Cryptogram
    /// </summary>
    /// <param name="isCdaSignatureRequested">
    ///     Specifies if a Combination Data Authentication Signature has been request
    /// </param>
    /// <param name="transactionRelatedData">
    ///     The data field of the command message contains CDOL1 Related Data coded  according to CDOL1 following the
    ///     rules defined in Book 3 section 4.1.4. In the case of IDS data writing, the data field of the command message
    ///     is a concatenation of CDOL1 Related Data followed by DSDOL related data coded according to DSDOL following the
    ///     rules defined in section 4.1.4.
    /// </param>
    /// <returns></returns>
    private static GenerateApplicationCryptogramCApduSignal CreateArqc(bool isCdaSignatureRequested, CommandTemplate transactionRelatedData)
    {
        byte referenceControlParameter = (byte) (0b10000000 | (byte) (isCdaSignatureRequested ? 0b10000 : 0));

        return new
            GenerateApplicationCryptogramCApduSignal(new Class(Messaging.Apdu.SecureMessaging.NotRecognized, LogicalChannel.BasicChannel),
                                                     Instruction.CardBlock, referenceControlParameter, 0,
                                                     transactionRelatedData.EncodeValue());
    }

    /// <summary>
    ///     Creates a Command APDU to request a Transaction Cryptogram
    /// </summary>
    /// <param name="isCdaSignatureRequested">
    ///     Specifies if a Combination Data Authentication Signature has been request
    /// </param>
    /// <param name="transactionRelatedData">
    ///     The data field of the command message contains CDOL1 Related Data coded  according to CDOL1 following the
    ///     rules defined in Book 3 section 4.1.4. In the case of IDS data writing, the data field of the command message
    ///     is a concatenation of CDOL1 Related Data followed by DSDOL related data coded according to DSDOL following the
    ///     rules defined in section 4.1.4.
    /// </param>
    /// <returns></returns>
    private static GenerateApplicationCryptogramCApduSignal CreateTc(bool isCdaSignatureRequested, CommandTemplate transactionRelatedData)
    {
        byte referenceControlParameter = (byte) (0b01000000 | (byte) (isCdaSignatureRequested ? 0b10000 : 0));

        return new
            GenerateApplicationCryptogramCApduSignal(new Class(Messaging.Apdu.SecureMessaging.NotRecognized, LogicalChannel.BasicChannel),
                                                     Instruction.CardBlock, referenceControlParameter, 0,
                                                     transactionRelatedData.EncodeValue());
    }

    #endregion
}