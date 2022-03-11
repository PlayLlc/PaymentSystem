using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.Icc.ReadRecord;

public class ReadRecordRApduSignal : RApduSignal
{
    #region Instance Values

    private readonly ShortFileId _ShortFileId;

    #endregion

    #region Constructor

    public ReadRecordRApduSignal(byte[] response, ShortFileId shortFileId) : base(response)
    { }

    #endregion

    #region Instance Members

    public ShortFileId GetShortFileId() => _ShortFileId;
    public override bool IsSuccessful() => throw new NotImplementedException();

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}