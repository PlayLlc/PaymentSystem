namespace Play.Interchange.Messages.Body;

public record AcquirerBody
{
    #region Instance Values

    private readonly byte[] _Value;
    public int Count => _Value.Length;

    #endregion

    #region Constructor

    public AcquirerBody(ReadOnlySpan<byte> value)
    {
        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    // HACK: Too slow and shit
    public int GetByteCount() => _Value.Length;

    public void CopyTo(Span<byte> buffer)
    {
        for (int i = 2, j = 0; j < Count; i++, j++)
            buffer[i] = _Value.ElementAt(j);
    }

    #endregion
}