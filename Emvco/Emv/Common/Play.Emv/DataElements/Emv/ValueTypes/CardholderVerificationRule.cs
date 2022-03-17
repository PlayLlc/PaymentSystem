using System;

using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

public record CvmRule
{
    #region Instance Values

    private readonly CvmCode _CvmCode;
    private readonly CvmConditionCode _CvmConditionCode;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    public CvmRule(ReadOnlySpan<byte> value)
    {
        if (value.Length != 2)
            throw new DataElementParsingException(nameof(value));

        _CvmCode = new CvmCode(value[0]);
        _CvmConditionCode = new CvmConditionCode(value[1]);
    }

    private CvmRule(CvmCode cvmCode, CvmConditionCode cvmConditionCode)
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