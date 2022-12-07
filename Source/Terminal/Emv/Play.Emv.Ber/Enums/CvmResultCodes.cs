using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.Enums;

public record CvmResultCodes : EnumObject<byte>
{
    #region Static Metadata

    public static readonly CvmResultCodes Empty = new();
    private static readonly ImmutableSortedDictionary<byte, CvmResultCodes> _ValueObjectMap;

    /// <remarks>Hex: 0x00; Decimal: 0</remarks>
    public static readonly CvmResultCodes Unknown;

    /// <remarks>Hex: 0x01; Decimal: 1</remarks>
    public static readonly CvmResultCodes Failed;

    /// <remarks>Hex: 0x02; Decimal: 2</remarks>
    public static readonly CvmResultCodes Successful;

    #endregion

    #region Constructor

    public CvmResultCodes()
    { }

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

    public override CvmResultCodes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out CvmResultCodes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}