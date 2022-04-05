using System;

namespace Play.Icc.Messaging.Apdu;

public abstract class ApduResponse : IApduResponse
{
    #region Instance Values

    protected readonly byte[] _Data;
    protected readonly StatusWords _StatusWords;

    #endregion

    #region Constructor

    protected ApduResponse(byte[] value)
    {
        _StatusWords = new StatusWords(value[..2]);

        _Data = value.Length == 2 ? Array.Empty<byte>() : value[2..];
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Returns the data field of the body as a raw encoded byte array
    /// </summary>
    /// <returns></returns>
    public byte[] GetData() => _Data;

    public int GetDataByteCount() => _Data.Length;
    public byte GetStatusWord1() => _StatusWords.GetStatusWord1();
    public byte GetStatusWord2() => _StatusWords.GetStatusWord2();
    public string GetStatusWordDescription() => _StatusWords.GetDescription();
    public StatusWordInfo GetStatusWordInfo() => _StatusWords.GetStatusWordInfo();
    public StatusWords GetStatusWords() => _StatusWords;

    #endregion
}