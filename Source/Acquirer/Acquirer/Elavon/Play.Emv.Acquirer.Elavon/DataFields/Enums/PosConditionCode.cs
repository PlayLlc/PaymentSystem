using System.Collections.Immutable;

using Play.Core;
using Play.Emv.Acquirer.DataFields;

namespace Play.Emv.Acquirer.Elavon.DataFields.Enums;

public record PosConditionCode : EnumObject<ushort>
{
    #region Static Metadata

    public static readonly PosConditionCode Empty = new();
    public static readonly PosConditionCode IssuerTimedOut1220 = new(1002);
    public static readonly PosConditionCode IssuerUnavailable1220 = new(1003);
    public static readonly PosConditionCode TerminalProcessed1220 = new(1004);
    public static readonly PosConditionCode ChipProcessed1220 = new(1005);
    public static readonly PosConditionCode UnderFloorLimit1220 = new(1006);
    public static readonly PosConditionCode StipByAcquirer1220 = new(1007);
    public static readonly PosConditionCode UnspecifiedOrUnknown1220 = new(1376);
    public static readonly PosConditionCode CustomerCancellation1420 = new(4000);
    public static readonly PosConditionCode LateResponse1420 = new(4006);
    public static readonly PosConditionCode UnableToCompleteTransaction1420 = new(4007);
    public static readonly PosConditionCode SuspectedFraud1420 = new(4008);
    public static readonly PosConditionCode InvalidResponseNoActionTaken1420 = new(4020);

    private static readonly ImmutableDictionary<ushort, PosConditionCode> _ValueObjectMap = new Dictionary<ushort, PosConditionCode>()
    {
        {IssuerTimedOut1220, IssuerTimedOut1220},
        {IssuerUnavailable1220, IssuerUnavailable1220},
        {TerminalProcessed1220, TerminalProcessed1220},
        {ChipProcessed1220, ChipProcessed1220},
        {UnderFloorLimit1220, UnderFloorLimit1220},
        {StipByAcquirer1220, StipByAcquirer1220},
        {UnspecifiedOrUnknown1220, UnspecifiedOrUnknown1220},
        {CustomerCancellation1420, CustomerCancellation1420},
        {LateResponse1420, LateResponse1420},
        {UnableToCompleteTransaction1420, UnableToCompleteTransaction1420},
        {SuspectedFraud1420, SuspectedFraud1420},
        {InvalidResponseNoActionTaken1420, InvalidResponseNoActionTaken1420}
    }.ToImmutableDictionary();

    #endregion

    #region Constructor

    public PosConditionCode()
    { }

    public PosConditionCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PosConditionCode[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(ushort value, out EnumObject<ushort>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out PosConditionCode? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}