using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.Enums;

public record DeviceTypes : EnumObject<ushort>
{
    #region Static Metadata

    public static readonly DeviceTypes Empty = new();
    private static readonly ImmutableSortedDictionary<ushort, DeviceTypes> _ValueObjectMap;

    /// <remarks>Hexadecimal: 0x3030, an: 00 </remarks>
    public static readonly DeviceTypes Card;

    /// <remarks>Hexadecimal: 0x3031, an: 01 </remarks>
    public static readonly DeviceTypes SimCard;

    /// <remarks>Hexadecimal: 0x3032, an: 02 </remarks>
    public static readonly DeviceTypes KeyFob;

    /// <remarks>Hexadecimal: 0x3033, an: 03 </remarks>
    public static readonly DeviceTypes Watch;

    /// <remarks>Hexadecimal: 0x3034, an: 04 </remarks>
    public static readonly DeviceTypes MobileTag;

    /// <remarks>Hexadecimal: 0x3035, an: 05 </remarks>
    public static readonly DeviceTypes WristBand;

    /// <summary>
    ///     Mobile phone with a permanent secure element that is controlled by the Mobile Network Operator (i.e. phone company)
    /// </summary>
    /// <remarks>Hexadecimal: 0x3037, an: 07 </remarks>
    public static readonly DeviceTypes SecureElementMnoForMobile;

    /// <summary>
    ///     Mobile phone with a removable secure element
    /// </summary>
    /// <remarks>Hexadecimal: 0x3038, an: 08 </remarks>
    public static readonly DeviceTypes RemovableSecureElementForMobile;

    /// <summary>
    ///     Mobile phone with a permanent secure element
    /// </summary>
    /// <remarks>Hexadecimal: 0x3039, an: 09 </remarks>
    public static readonly DeviceTypes SecureElementForMobile;

    /// <summary>
    ///     Tablet or eBook with a removable secure element that is controlled by the Mobile Network Operator (i.e. phone
    ///     company)
    /// </summary>
    /// <remarks>Hexadecimal: 0x30A0, an: 10 </remarks>
    public static readonly DeviceTypes RemovableSecureElementMnoForTablet;

    /// <summary>
    ///     Tablet or eBook with a permanent secure element controlled by a Mobile Network Operator (i.e. phone company)
    /// </summary>
    /// <remarks>Hexadecimal: 0x30A1, an: 11 </remarks>
    public static readonly DeviceTypes SecureElementMnoForTablet;

    /// <summary>
    ///     Tablet or eBook with a removable secure element
    /// </summary>
    /// <remarks>Hexadecimal: 0x303A2, an: 12 </remarks>
    public static readonly DeviceTypes RemovableSecureElementForTablet;

    /// <summary>
    ///     Tablet or eBook with a permanent secure element
    /// </summary>
    /// <remarks>Hexadecimal: 0x30A3, an: 13 </remarks>
    public static readonly DeviceTypes SecureElementForTablet;

    #endregion

    #region Constructor

    public DeviceTypes() : base()
    { }

    static DeviceTypes()
    {
        Card = new DeviceTypes(0x3030);
        SimCard = new DeviceTypes(0x3031);
        KeyFob = new DeviceTypes(0x3032);
        Watch = new DeviceTypes(0x3033);
        MobileTag = new DeviceTypes(0x3034);
        WristBand = new DeviceTypes(0x3035);
        SecureElementMnoForMobile = new DeviceTypes(0x3037);
        RemovableSecureElementForMobile = new DeviceTypes(0x3038);
        SecureElementForMobile = new DeviceTypes(0x3039);
        RemovableSecureElementMnoForTablet = new DeviceTypes(0x30A0);
        SecureElementMnoForTablet = new DeviceTypes(0x30A1);
        RemovableSecureElementForTablet = new DeviceTypes(0x30A2);
        SecureElementForTablet = new DeviceTypes(0x30A3);

        _ValueObjectMap = new Dictionary<ushort, DeviceTypes> {{Card, Card}}.ToImmutableSortedDictionary();
    }

    private DeviceTypes(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DeviceTypes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(ushort value, out EnumObject<ushort>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out DeviceTypes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator ushort(DeviceTypes value) => value._Value;

    #endregion
}