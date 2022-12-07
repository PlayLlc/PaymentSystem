namespace Play.Codecs;

public interface IEncodeStructsToBuffer
{
    #region Instance Members

    public ushort GetByteCount<_T>(_T value) where _T : struct;
    public ushort GetByteCount<_T>(_T[] value) where _T : struct;
    public void Encode<_T>(_T value, Span<byte> buffer, ref int offset) where _T : struct;
    public void Encode<_T>(_T value, int length, Span<byte> buffer, ref int offset) where _T : struct;
    public void Encode<_T>(_T[] value, Span<byte> buffer, ref int offset) where _T : struct;
    public void Encode<_T>(_T[] value, int length, Span<byte> buffer, ref int offset) where _T : struct;

    #endregion
}