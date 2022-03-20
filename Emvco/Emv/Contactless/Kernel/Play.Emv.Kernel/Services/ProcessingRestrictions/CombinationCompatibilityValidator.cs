using System;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services.ProcessingRestrictions;

public class CombinationCompatibilityValidator : IValidateCombinationCapability
{
    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public void Process(KernelDatabase database)
    {
        CheckVersionNumber(database);
        HandleApplicationActivationStatus(database);
    }

    #region PRE.1 - PRE.3

    /// <remarks>EMV Book C-2 Section PRE.1 - PRE.3 </remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static void CheckVersionNumber(KernelDatabase database)
    {
        if (!database.IsPresentAndNotEmpty(ApplicationVersionNumberCard.Tag))
            return;

        ApplicationVersionNumberCard versionNumberCard = (ApplicationVersionNumberCard) database.Get(ApplicationVersionNumberCard.Tag);

        ApplicationVersionNumberReader versionNumberReader =
            (ApplicationVersionNumberReader) database.Get(ApplicationVersionNumberReader.Tag);

        if ((ushort) versionNumberCard != (ushort) versionNumberReader)
            database.Set(TerminalVerificationResultCodes.IccAndTerminalHaveDifferentApplicationVersions);
    }

    #endregion

    #endregion

    #region PRE.4 - PRE.8

    /// <remarks>EMV Book C-2 Section PRE.4 - PRE.8 </remarks>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void HandleApplicationActivationStatus(KernelDatabase database)
    {
        TransactionDate transactionDate = (TransactionDate) database.Get(TransactionDate.Tag);

        HandleApplicationNotYetActive(database, transactionDate);
        HandleExpiredApplication(database, transactionDate);
    }

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static void HandleApplicationNotYetActive(KernelDatabase database, TransactionDate transactionDate)
    {
        ApplicationEffectiveDate applicationEffectiveDate = (ApplicationEffectiveDate) database.Get(ApplicationEffectiveDate.Tag);

        if ((uint) transactionDate < (uint) applicationEffectiveDate)
            database.Set(TerminalVerificationResultCodes.ExpiredApplication);
    }

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static void HandleExpiredApplication(KernelDatabase database, TransactionDate transactionDate)
    {
        ApplicationExpirationDate applicationExpirationDate = (ApplicationExpirationDate) database.Get(ApplicationExpirationDate.Tag);

        if ((uint) transactionDate > (uint) applicationExpirationDate)
            database.Set(TerminalVerificationResultCodes.ExpiredApplication);
    }

    #endregion
}