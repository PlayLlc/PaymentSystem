using System;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.DataElements.CvmConditions;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Services.Conditions.CvmConditions;

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

    public CvmCode GetCvmCode() => _CvmCode;
    public CvmConditionCode GetCvmConditionCode() => _CvmConditionCode;

    #endregion
}