using Play.Emv.Ber;

namespace Play.Emv.Icc;

public class ComputeCryptographicChecksumRApduSignal : RApduSignal
{
    #region Constructor

    public ComputeCryptographicChecksumRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override bool IsSuccessful() => throw new NotImplementedException();

    public Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}