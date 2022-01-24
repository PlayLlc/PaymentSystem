namespace Play.Emv.Icc.ApplicationUnblock;

public class ApplicationUnblockRApduSignal : RApduSignal
{
    #region Constructor

    public ApplicationUnblockRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}