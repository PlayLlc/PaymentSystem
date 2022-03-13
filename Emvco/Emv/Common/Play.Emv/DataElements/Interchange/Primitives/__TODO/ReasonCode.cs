using Play.Core;

namespace Play.Emv.DataElements.__TODO;

public record ReasonCode : EnumObject<ushort>
{
    #region Static Metadata

    public static readonly FunctionCode IssuerTimedOut1220 = new FunctionCode(1002);
    public static readonly FunctionCode IssuerUnavailable1220 = new FunctionCode(1003);
    public static readonly FunctionCode TerminalProcessed1220 = new FunctionCode(1004);
    public static readonly FunctionCode ChipProcessed1220 = new FunctionCode(1005);
    public static readonly FunctionCode UnderFloorLimit1220 = new FunctionCode(1006);
    public static readonly FunctionCode StipByAcquirer1220 = new FunctionCode(1007);
    public static readonly FunctionCode UnspecifiedOrUnknown1220 = new FunctionCode(1376);
    public static readonly FunctionCode CustomerCancellation1420 = new FunctionCode(4000);
    public static readonly FunctionCode LateResponse1420 = new FunctionCode(4006);
    public static readonly FunctionCode UnableToCompleteTransaction1420 = new FunctionCode(4007);
    public static readonly FunctionCode SuspectedFraud1420 = new FunctionCode(4008);
    public static readonly FunctionCode InvalidResponseNoActionTaken1420 = new FunctionCode(4020);

    #endregion

    #region Constructor

    public ReasonCode(ushort value) : base(value)
    { }

    #endregion
}