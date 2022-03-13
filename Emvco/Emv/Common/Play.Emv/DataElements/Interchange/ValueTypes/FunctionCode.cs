using Play.Core;

namespace Play.Emv.DataElements;

public record FunctionCode : EnumObject<ushort>
{
    #region Static Metadata

    public static readonly FunctionCode Original1220Message = new FunctionCode(200);
    public static readonly FunctionCode FirstRepetitionOf1220 = new FunctionCode(201);
    public static readonly FunctionCode SecondRepetitionOf1220 = new FunctionCode(202);
    public static readonly FunctionCode ThirdRepetitionOf1220 = new FunctionCode(203);
    public static readonly FunctionCode Original1420Message = new FunctionCode(400);
    public static readonly FunctionCode FirstRepetitionOf1420 = new FunctionCode(401);
    public static readonly FunctionCode SecondRepetitionOf1420 = new FunctionCode(402);
    public static readonly FunctionCode ThirdRepetitionOf1420 = new FunctionCode(403);
    public static readonly FunctionCode SessionManagementMessage = new FunctionCode(200);
    public static readonly FunctionCode ReconciliationMessage = new FunctionCode(200);

    #endregion

    #region Constructor

    public FunctionCode(ushort value) : base(value)
    { }

    #endregion
}