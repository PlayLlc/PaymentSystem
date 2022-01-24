namespace Play.Emv.Icc.Checksum;

public class ComputeCryptographicChecksumRApduSignal : RApduSignal
{
    #region Constructor

    public ComputeCryptographicChecksumRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}