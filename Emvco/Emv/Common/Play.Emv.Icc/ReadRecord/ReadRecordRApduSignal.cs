using Play.Emv.Ber.Enums;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Icc.Messaging.Apdu;

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

    public override bool IsSuccessful()
    {
        if (GetStatusWords() != StatusWords._9000)
            return false;

        return true;
    }

    #endregion
}