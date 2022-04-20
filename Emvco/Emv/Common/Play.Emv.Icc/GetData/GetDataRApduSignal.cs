using Play.Emv.Ber;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Icc;

public class GetDataRApduSignal : RApduSignal
{
    #region Constructor

    public GetDataRApduSignal(byte[] value) : base(value)
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