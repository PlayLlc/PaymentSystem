namespace Play.Emv.Icc.GetProcessingOptions;

public class GetProcessingOptionsRApduSignal : RApduSignal
{
    #region Constructor

    public GetProcessingOptionsRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override bool IsSuccessful() => throw new NotImplementedException();

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}