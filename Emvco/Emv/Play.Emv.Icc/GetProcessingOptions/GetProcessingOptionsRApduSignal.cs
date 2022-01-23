namespace Play.Icc.Emv.GetProcessingOptions;

public class GetProcessingOptionsRApduSignal : RApduSignal
{
    #region Constructor

    public GetProcessingOptionsRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}