using Play.Emv.Ber.Enums;

namespace Play.Emv.Icc;

public class ApplicationUnblockRApduSignal : RApduSignal
{
    #region Constructor

    public ApplicationUnblockRApduSignal(byte[] value) : base(value)
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