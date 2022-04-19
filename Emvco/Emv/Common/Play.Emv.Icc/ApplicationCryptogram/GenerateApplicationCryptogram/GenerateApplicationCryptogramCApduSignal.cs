using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Icc;

public class GenerateApplicationCryptogramCApduSignal : CApduSignal
{
    #region Constructor

    protected GenerateApplicationCryptogramCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
        parameter2)
    { }

    protected GenerateApplicationCryptogramCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) : base(@class, instruction,
        parameter1, parameter2, le)
    { }

    protected GenerateApplicationCryptogramCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class,
        instruction, parameter1, parameter2, data)
    { }

    protected GenerateApplicationCryptogramCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint? le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public CryptogramTypes GetCryptogramTypes()
    {
        if (GetParameter1().IsBitSet(Bits.Seven))
            return CryptogramTypes.TransactionCryptogram;

        if (GetParameter1().IsBitSet(Bits.Eight))
            return CryptogramTypes.AuthorizationRequestCryptogram;

        if (GetParameter1() == 0)
            return CryptogramTypes.ApplicationAuthenticationCryptogram;

        throw new TerminalDataException($"The {nameof(GenerateApplicationCryptogramCApduSignal)} was not correctly encoded");
    }

    /// <exception cref="BerParsingException"></exception>
    public static GenerateApplicationCryptogramCApduSignal Create(
        ReferenceControlParameter referenceControlParameter, CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectListResult) =>

        // CHECK: Check that the use of the CardRiskManagementDataObjectList1RelatedData is used correctly when creating this CAPDU
        new(new Class(SecureMessaging.NotRecognized, LogicalChannel.BasicChannel), Instruction.CardBlock, (byte) referenceControlParameter, 0,
            cardRiskManagementDataObjectListResult.EncodeTagLengthValue());

    /// <exception cref="BerParsingException"></exception>
    public static GenerateApplicationCryptogramCApduSignal Create(
        ReferenceControlParameter referenceControlParameter, CardRiskManagementDataObjectList1RelatedData cardRiskManagementDataObjectListResult,
        DataObjectListResult dataStorageDataObjectListResult)
    {
        CommandTemplate dataStorageCommandTemplate = dataStorageDataObjectListResult.AsCommandTemplate();

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(cardRiskManagementDataObjectListResult.GetValueByteCount()
            + dataStorageCommandTemplate.GetValueByteCount());
        Span<byte> buffer = spanOwner.Span;

        cardRiskManagementDataObjectListResult.EncodeValue().CopyTo(buffer);
        dataStorageCommandTemplate.EncodeValue().CopyTo(buffer[cardRiskManagementDataObjectListResult.GetValueByteCount()..]);

        return new GenerateApplicationCryptogramCApduSignal(new Class(SecureMessaging.NotRecognized, LogicalChannel.BasicChannel), Instruction.CardBlock,
            (byte) referenceControlParameter, 0, buffer);
    }

    public bool IsCdaRequested() => GetParameter1().IsBitSet(Bits.Five);

    #endregion
}