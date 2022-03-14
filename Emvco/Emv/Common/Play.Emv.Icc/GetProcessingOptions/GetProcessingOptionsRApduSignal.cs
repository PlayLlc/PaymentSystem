namespace Play.Emv.Icc.GetProcessingOptions;

public class GetProcessingOptionsRApduSignal : RApduSignal
{
    #region Constructor

    public GetProcessingOptionsRApduSignal(byte[] value) : base(value)
    { }

    public GetProcessingOptionsRApduSignal(byte[] value, Level1Error level1Error) : base(value, level1Error)
    { }

    #endregion

    #region Instance Members

    public override bool IsSuccessful() => throw new NotImplementedException();

    #endregion
}