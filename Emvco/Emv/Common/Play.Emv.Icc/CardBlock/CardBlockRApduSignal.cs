namespace Play.Emv.Icc.CardBlock;

public class CardBlockRApduSignal : RApduSignal
{
    #region Constructor

    public CardBlockRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}