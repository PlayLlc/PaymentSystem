using Play.Emv.Ber.Enums;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.Icc;

public class ReadRecordRApduSignal : RApduSignal
{
    #region Instance Values

    private readonly ShortFileId _ShortFileId;

    #endregion

    #region Constructor

    public ReadRecordRApduSignal(byte[] response, ShortFileId shortFileId) : base(response)
    {
        _ShortFileId = shortFileId;
    }

    public ReadRecordRApduSignal(byte[] response, ShortFileId shortFileId, Level1Error level1Error) : base(response, level1Error)
    {
        _ShortFileId = shortFileId;
    }

    #endregion

    #region Instance Members

    public ShortFileId GetShortFileId() => _ShortFileId;
    public override bool IsSuccessful() => throw new NotImplementedException();

    #endregion
}