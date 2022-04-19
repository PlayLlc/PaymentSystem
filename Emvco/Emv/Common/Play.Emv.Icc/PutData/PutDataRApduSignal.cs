using Play.Emv.Ber;

namespace Play.Emv.Icc;

public class PutDataRApduSignal : RApduSignal
{
    #region Constructor

    public PutDataRApduSignal(byte[] value) : base(value)
    { }

    public PutDataRApduSignal(byte[] response, Level1Error level1Error) : base(response, level1Error)
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