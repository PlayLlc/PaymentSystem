﻿using System;

using Play.Ber.Identifiers;
using Play.Emv.Database;
using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services.Selection.CvmConditions;

internal record PurchaseWithCashbackCondition : CvmCondition
{
    #region Static Metadata

    public static readonly CvmConditionCode Code = new(5);

    #endregion

    #region Instance Values

    protected override Tag[] _RequiredData => throw new NotImplementedException();

    #endregion

    #region Instance Members

    public override CvmConditionCode GetConditionCode() => Code;
    protected override bool IsConditionSatisfied(KernelDatabase database, Money xAmount, Money yAmount)
    {
        return database.IsPurchaseTransactionWithCashback(); 
    }

    #endregion
}