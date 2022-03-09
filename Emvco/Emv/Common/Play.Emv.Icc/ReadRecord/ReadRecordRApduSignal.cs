using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.Icc.ReadRecord;

public class ReadRecordRApduSignal : RApduSignal
{
    #region Constructor

    public ReadRecordRApduSignal(ShortFileId shortFileId, byte[] response) : base(response)
    {
        _ShortFileId = shortFileId;
    }

    #endregion

    #region Instance Members

    public override bool IsSuccessful() => throw new NotImplementedException();

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}