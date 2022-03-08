using Play.Core;

namespace Play.Emv.DataElements.Interchange.Primitives.__TODO;

public record FunctionCode : EnumObject<ushort>
{
    #region Static Metadata

    public static readonly FunctionCode Original1220Message = new(200);
    public static readonly FunctionCode FirstRepetitionOf1220 = new(201);
    public static readonly FunctionCode SecondRepetitionOf1220 = new(202);
    public static readonly FunctionCode ThirdRepetitionOf1220 = new(203);
    public static readonly FunctionCode Original1420Message = new(400);
    public static readonly FunctionCode FirstRepetitionOf1420 = new(401);
    public static readonly FunctionCode SecondRepetitionOf1420 = new(402);
    public static readonly FunctionCode ThirdRepetitionOf1420 = new(403);
    public static readonly FunctionCode SessionManagementMessage = new(200);
    public static readonly FunctionCode ReconciliationMessage = new(200);

    #endregion

    #region Constructor

    public FunctionCode(ushort value) : base(value)
    { }

    #endregion
}