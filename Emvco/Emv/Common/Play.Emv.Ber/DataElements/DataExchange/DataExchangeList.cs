namespace Play.Emv.Ber.DataElements;

public abstract record DataExchangeList<_T> : DataElement<_T[]>
{
    #region Instance Values

    protected new readonly Queue<_T> _Value;

    #endregion

    #region Constructor

    protected DataExchangeList(_T[] value) : base(value)
    {
        _Value = new Queue<_T>(value);
    }

    #endregion

    #region Instance Members

    public _T[] AsArray() => _Value.Count == 0 ? Array.Empty<_T>() : _Value.ToArray();

    public void Clear()
    {
        _Value.Clear();
    }

    public int Count() => _Value.Count;
    public bool IsEmpty() => Count() == 0;

    public void Enqueue(_T item)
    {
        if (_Value.Contains(item))
            return;

        _Value.Enqueue(item);
    }

    public void Enqueue(_T[] items)
    {
        for (nint i = 0; i < items.Length; i++)
            Enqueue(items[i]);
    }

    public bool TryDequeue(out _T? result)
    {
        if (!_Value.TryDequeue(out _T? localResult))
        {
            result = default;

            return false;
        }

        result = localResult;

        return true;
    }

    #endregion
}