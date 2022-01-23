namespace Play.Icc.Emv.GenerateApplicationCryptogram;

public class GenerateApplicationCryptogramRApduSignal : RApduSignal
{
    #region Constructor

    public GenerateApplicationCryptogramRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}