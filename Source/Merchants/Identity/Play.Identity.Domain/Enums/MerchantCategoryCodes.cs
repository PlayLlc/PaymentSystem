using System.Collections.Immutable;

using Play.Core;

namespace Play.Identity.Domain.Enums;

public record MerchantCategoryCodes : EnumObject<ushort>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<ushort, MerchantCategoryCodes> _ValueObjectMap;
    public static MerchantCategoryCodes Empty;
    public static MerchantCategoryCodes Accounting;
    public static MerchantCategoryCodes Childcare;
    public static MerchantCategoryCodes Consulting;
    public static MerchantCategoryCodes Delivery;
    public static MerchantCategoryCodes LegalServices;

    #endregion

    #region Instance Values

    public readonly string Name;

    #endregion

    #region Constructor

    private MerchantCategoryCodes(string name, ushort value) : base(value)
    {
        Name = name;
    }

    static MerchantCategoryCodes()
    {
        Empty = new MerchantCategoryCodes(nameof(Empty), 0);
        Accounting = new MerchantCategoryCodes(nameof(Accounting), 8931);
        Childcare = new MerchantCategoryCodes(nameof(Childcare), 8351);
        Consulting = new MerchantCategoryCodes(nameof(Consulting), 7392);
        Delivery = new MerchantCategoryCodes(nameof(Delivery), 4214);
        LegalServices = new MerchantCategoryCodes(nameof(LegalServices), 8111);

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

    public override MerchantCategoryCodes[] GetAll() => _ValueObjectMap.Values.ToArray();

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