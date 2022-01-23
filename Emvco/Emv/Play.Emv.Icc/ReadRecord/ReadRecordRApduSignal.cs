namespace Play.Icc.Emv.ReadRecord;

public class ReadRecordRApduSignal : RApduSignal
{
    #region Constructor

    public ReadRecordRApduSignal(byte[] response) : base(response)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}