using System;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;

namespace Play.Emv.Kernel.Services.Conditions;

public record CardholderVerificationRule
{
    #region Instance Values

    private readonly CvmCode _CvmCode;
    private readonly CvmConditionCode _CvmConditionCode;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    public CardholderVerificationRule(ReadOnlySpan<byte> value)
    {
        if (value.Length != 2)
            throw new DataElementParsingException(nameof(value));

        _CvmCode = new CvmCode(value[0]);
        _CvmConditionCode = new CvmConditionCode(value[1]);
    }

    private CardholderVerificationRule(CvmCode cvmCode, CvmConditionCode cvmConditionCode)
    {
        _CvmCode = cvmCode;
        _CvmConditionCode = cvmConditionCode;
    }

    #endregion

    #region Instance Members

    /// <exception cref="DataElementParsingException"></exception>
    public bool IsSupported(IQueryTlvDatabase database)
    {
        if (!CvmCondition.Is(_CvmConditionCode, database, out CvmConditionCode? cvmConditionCode))
            return false;

        TerminalCapabilities terminalCapabilities =
            TerminalCapabilities.Decode(database.Get(TerminalCapabilities.Tag).EncodeValue().AsSpan());

        if (!CvmCode.IsRecognized(cvmCode, terminalCapabilities))
            return false;

        return true;
    }

    /// <summary>
    ///     This factory method creates the <see cref="CardholderVerificationRule" /> if the
    ///     <see cref="CvmCondition" /> is
    ///     recognized by the terminal
    /// </summary>
    /// <param name="database"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// HACK: This method should not be used to instantiate the list of CVMRules in the CvmList. Iterating through the records to create the list would use brute force to check the conditions to process a CVM. Instead, we should process each condition and code one by one, and select that code if it passes, instead of validating the condition of every single rule
    public static bool TryCreate(ReadOnlySpan<byte> value, IQueryTlvDatabase database, out CardholderVerificationRule? result)
    {
        if (value.Length != 2)
        {
            result = null;

            return false;
        }

        if (!CvmCondition.TryCreate(value[1], database, out CvmConditionCode? cvmConditionCode))
        {
            result = null;

            return false;
        }

        TerminalCapabilities terminalCapabilities =
            TerminalCapabilities.Decode(database.Get(TerminalCapabilities.Tag).EncodeValue().AsSpan());

        CvmCode cvmCode = new(value[1]);

        if (!CvmCode.IsRecognized(cvmCode, terminalCapabilities))
        {
            result = null;

            return false;
        }

        result = new CardholderVerificationRule(cvmCode, cvmConditionCode!.Value);

        return true;
    }

    #endregion
}