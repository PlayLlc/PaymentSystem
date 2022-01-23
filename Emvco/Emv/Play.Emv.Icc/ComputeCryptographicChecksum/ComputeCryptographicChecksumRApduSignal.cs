using System;

namespace Play.Icc.Emv.ComputeCryptographicChecksum;

public class ComputeCryptographicChecksumRApduSignal : RApduSignal
{
    #region Constructor

    public ComputeCryptographicChecksumRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>

        // Check out Status Words
        throw new NotImplementedException();

    #endregion
}