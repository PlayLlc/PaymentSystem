namespace Play.Icc.Emv.CardBlock;

public class CardBlockRApduSignal : RApduSignal
{
    #region Constructor

    public CardBlockRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error()
    {
        // Check out Status Words
        throw new NotImplementedException();
    }

    #endregion
}