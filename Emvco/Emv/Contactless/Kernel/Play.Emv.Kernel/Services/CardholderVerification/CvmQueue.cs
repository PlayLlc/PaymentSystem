using System;
using System.Collections.Generic;
using System.Linq;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services;

internal class CvmQueue
{
    #region Instance Values

    private readonly List<CardholderVerificationRule> _Rules;
    private readonly Money _XAmount;
    private readonly Money _YAmount;
    private byte _Offset = 0;

    #endregion

    #region Constructor

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public CvmQueue(IQueryTlvDatabase database)
    {
        CvmList cvmList = CvmList.Decode(database.Get(CvmList.Tag).EncodeValue().AsSpan());
        ApplicationCurrencyCode applicationCurrencyCode =
            ApplicationCurrencyCode.Decode(database.Get(ApplicationCurrencyCode.Tag).EncodeValue().AsSpan());
        _XAmount = cvmList.GetXAmount(applicationCurrencyCode);
        _YAmount = cvmList.GetYAmount(applicationCurrencyCode);
        _Rules = cvmList.GetCardholderVerificationRules().Where(a => CvmCondition.Exists(a.GetCvmConditionCode())).ToList();
    }

    #endregion

    #region Instance Members

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public bool TrySelect(IQueryTlvDatabase database, out CardholderVerificationRule? rule)
    {
        if (_Rules.Count < (_Offset - 1))
        {
            rule = null;

            return false;
        }

        TerminalCapabilities terminalCapabilities =
            TerminalCapabilities.Decode(database.Get(TerminalCapabilities.Tag).EncodeValue().AsSpan());

        for (; _Offset < _Rules.Count; _Offset++)
        {
            if (!CvmCondition.TryGet(_Rules[_Offset].GetCvmConditionCode(), out CvmCondition? cvmCondition))
                continue;

            if (cvmCondition!.IsConditionSatisfied(_Rules[_Offset].GetCvmConditionCode(), database))
                continue;

            if (!_Rules[_Offset].GetCvmCode().IsRecognized(terminalCapabilities))
                continue;

            rule = _Rules[_Offset];

            return true;
        }

        rule = null;

        return false;
    }

    #endregion
}