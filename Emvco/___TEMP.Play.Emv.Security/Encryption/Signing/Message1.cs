using System;
using System.Collections;

using Play.Core.Exceptions;

namespace Play.Emv.Security.Encryption.Signing;

internal class Message1 : IEnumerable
{
    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public Message1(byte[] value)
    {
        CheckCore.ForNullOrEmptySequence(value, nameof(value));
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value;
    public Span<byte> AsSpan() => _Value.AsSpan();
    public int GetByteCount() => _Value.Length;
    public IEnumerator GetEnumerator() => _Value.GetEnumerator();

    #endregion

    public byte this[int index] => _Value[index];
    public ReadOnlySpan<byte> this[Range index] => _Value[index];
}