using System.Numerics;

namespace Play.Codecs;

public readonly record struct PlayEncodingId
{
    #region Instance Values

    private readonly int _Value;

    #endregion

    #region Constructor

    public PlayEncodingId(Type value)
    {
        _Value = PlayCodec.SignedIntegerCodec.DecodeToInt32(PlayCodec.UnicodeCodec.Encode(value.FullName));
 
    }

    #endregion
     

    
}