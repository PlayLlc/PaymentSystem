namespace Play.Emv.Icc.ReadRecord;

public class ReadRecordRApduSignal : RApduSignal
{
    #region Constructor

    public ReadRecordRApduSignal(byte[] response) : base(response)
    { }

    #endregion

    #region Instance Members

    public override bool IsSuccessful() => throw new NotImplementedException();

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}