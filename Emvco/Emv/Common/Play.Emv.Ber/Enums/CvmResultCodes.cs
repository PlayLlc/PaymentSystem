using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber;

public record CvmResultCodes : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CvmResultCodes> _ValueObjectMap;

    /// <remarks>Hex: 0x00; Decimal: 0</remarks>
    public static readonly CvmResultCodes Unknown;

    /// <remarks>Hex: 0x01; Decimal: 1</remarks>
    public static readonly CvmResultCodes Failed;

    /// <remarks>Hex: 0x02; Decimal: 2</remarks>
    public static readonly CvmResultCodes Successful;

    #endregion

    #region Constructor

    static CvmResultCodes()
    {
        Unknown = new CvmResultCodes(0);
        Failed = new CvmResultCodes(1);
        Successful = new CvmResultCodes(2);
        _ValueObjectMap = new Dictionary<byte, CvmResultCodes> {{Unknown, Unknown}, {Failed, Failed}, {Successful, Successful}}.ToImmutableSortedDictionary();
    }

    private CvmResultCodes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static CvmResultCodes[] GetAll() => _ValueObjectMap.Values.ToArray();

    #endregion
}