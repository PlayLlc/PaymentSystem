using System.Collections;

using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Interchange.Messages.Body;

public class AcquirerBody : IReadOnlyCollection<byte>
{
    #region Instance Values

    private readonly List<byte> _Value;
    public int Count => _Value.Count;

    #endregion

    #region Constructor

    public AcquirerBody()
    {
        _Value = new List<byte>();
    }

    #endregion

    #region Instance Members

    public void Add(DataField dataField)
    {
        dataField.CopyTo(_Value);
    }

    public void CopyTo(Span<byte> buffer, int offset)
    {
        SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(Count);

        lock (_Value)
        {
            for (int i = 0, j = offset; i < Count; i++, j++)
                buffer[j] = _Value.ElementAt(j);
        }
    }

    public IEnumerator<byte> GetEnumerator() => _Value.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}