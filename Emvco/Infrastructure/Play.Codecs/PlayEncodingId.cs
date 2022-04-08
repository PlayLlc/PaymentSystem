using System.Text;

using Play.Core.Specifications;

namespace Play.Codecs;

public readonly record struct PlayEncodingId
{
    #region Instance Values

    private readonly int _Value;

    #endregion

    #region Constructor

    public PlayEncodingId(Type value)
    {
        const int hash = 269;
        int result = 0;

        ReadOnlySpan<byte> hashSeed = Encoding.ASCII.GetBytes(value.FullName!);

        unchecked
        {
            for (int i = 0; i < hashSeed.Length; i++)
                result += hashSeed[i] * hash;

            _Value = result;
        }
    }

    #endregion
}