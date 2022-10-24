﻿using System.Collections.Immutable;

using Play.Core;

namespace Play.Accounts.Domain.Enums;

public record MerchantCategoryCodes : EnumObject<ushort>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<ushort, MerchantCategoryCodes> _ValueObjectMap;
    public static MerchantCategoryCodes Empty;
    public static MerchantCategoryCodes Accounting;
    public static MerchantCategoryCodes Childcare;
    public static MerchantCategoryCodes Consulting;
    public static MerchantCategoryCodes Delivery;
    public static MerchantCategoryCodes Design;
    public static MerchantCategoryCodes InteriorDesign;
    public static MerchantCategoryCodes LegalServices;

    #endregion

    #region Constructor

    private MerchantCategoryCodes(ushort value) : base(value)
    { }

    static MerchantCategoryCodes()
    {
        Empty = new MerchantCategoryCodes(0);
        Accounting = new MerchantCategoryCodes(8931);
        Childcare = new MerchantCategoryCodes(8351);
        Consulting = new MerchantCategoryCodes(7392);
        Delivery = new MerchantCategoryCodes(4214);
        LegalServices = new MerchantCategoryCodes(8111);

        // ...
        _ValueObjectMap = new Dictionary<ushort, MerchantCategoryCodes>
        {
            {Accounting, Accounting},
            {Childcare, Childcare},
            {Consulting, Consulting},
            {Delivery, Delivery},
            {LegalServices, LegalServices}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override MerchantCategoryCodes[] GetAll()
    {
        return _ValueObjectMap.Values.ToArray();
    }

    public override bool TryGet(ushort value, out EnumObject<ushort>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out MerchantCategoryCodes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}