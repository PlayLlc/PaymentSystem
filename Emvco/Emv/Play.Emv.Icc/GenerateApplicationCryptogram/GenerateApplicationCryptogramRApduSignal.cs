using System;

namespace Play.Icc.Emv.GenerateApplicationCryptogram;

public class GenerateApplicationCryptogramRApduSignal : RApduSignal
{
    #region Constructor

    public GenerateApplicationCryptogramRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>

        // Check out Status Words
        throw new NotImplementedException();

    #endregion
}