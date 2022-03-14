namespace Play.Emv.Icc;

public class GenerateApplicationCryptogramRApduSignal : RApduSignal
{
    #region Constructor

    public GenerateApplicationCryptogramRApduSignal(byte[] value) : base(value)
    { }

    public GenerateApplicationCryptogramRApduSignal(byte[] value, Level1Error level1Error) : base(value, level1Error)
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