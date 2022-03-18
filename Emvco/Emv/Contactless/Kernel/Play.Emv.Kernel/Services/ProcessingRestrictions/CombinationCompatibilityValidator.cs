using System;

using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.ProcessingRestrictions;

public class CombinationCompatibilityValidator : IValidateCombinationCapability
{
    #region Instance Members

    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public void Process(KernelDatabase database)
    {
        CheckVersionNumber(database);
        HandleApplicationActivationStatus(database);
    }

    #endregion

    #region PRE.1 - PRE.3

    /// <remarks>EMV Book C-2 Section PRE.1 - PRE.3 </remarks>
    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static void CheckVersionNumber(KernelDatabase database)
    {
        if (!database.IsPresentAndNotEmpty(ApplicationVersionNumberCard.Tag))
            return;

        ApplicationVersionNumberCard versionNumbe
        ApplicationVersionNumberCard.Decode(database.Get(ApplicationVersionNumberCard.Tag).EncodeValue().AsSpan());

        ApplicationVersionNumberTerminal versionNumberTer
        ApplicationVersionNumberTerminal.Decode(database.Get(ApplicationVersionNumberTerminal.Tag).EncodeValue().AsSpan());

        if ((ushort) versionNumberCard != (ushort) versionNumberTerminal)
            database.Set(TerminalVerificationResultCodes.IccAndTerminalHaveDifferentApplicationVersions);
    }

#en
#endregiondregion

    #region PRE.4 - PRE.8

    /// <remarks>EMV Book C-2 Section PRE.4 - PRE.8 </remarks>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public void HandleApplicationActivationStatus(KernelDatabase database)
    {
        TransactionDate transactionDate = TransactionDate.Decode(database.Get(TransactionDate.Tag).EncodeValue().AsSpan());

        HandleApplicationNotYetActive(database, transactionDate);
        HandleExpiredApplication(database, transactionDate);
    }

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static void HandleApplicationNotYetActive(KernelDatabase database, TransactionDate transactionDate)
    {
        ApplicationEffectiveDate applicationE

        ApplicationEffectiveDate.Decode(database.Get(ApplicationEffectiveDate.Tag).EncodeValue().AsSpan());

        if ((uint) transactionDate < (uint) applicationEffectiveDate)
            database.Set(TerminalVerificationResultCodes.ExpiredApplication);
    }

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="Emv.Exceptions.TerminalDataException"></exception>
    public static void HandleExpiredApplication(KernelDatabase database, TransactionDate transactionDate)
    {
        ApplicationExpirationDate applic nDate =
            ApplicationExpirationDate.Decode(database.Get(ApplicationExpirationDate.Tag).EncodeValue().AsSpan());

        if ((uint) transactionDate > (uint) applicationExpirationDate)
            database.Set(TerminalVerificationResultCodes.ExpiredApplication);
    }

    #endregion
}