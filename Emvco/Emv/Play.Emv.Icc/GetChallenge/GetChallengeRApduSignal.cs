namespace Play.Icc.Emv.GetChallenge;

public class GetChallengeRApduSignal : RApduSignal
{
    #region Constructor

    public GetChallengeRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}