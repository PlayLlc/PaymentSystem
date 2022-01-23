using System;

namespace Play.Icc.Emv.GetChallenge;

public class GetChallengeRApduSignal : RApduSignal
{
    #region Constructor

    public GetChallengeRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>

        // Check out Status Words
        throw new NotImplementedException();

    #endregion
}