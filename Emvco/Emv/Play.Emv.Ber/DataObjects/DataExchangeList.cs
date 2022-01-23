using System;
using System.Collections.Generic;

namespace Play.Ber.Emv.DataObjects;

public abstract record DataExchangeList<T> : DataElement<T[]>
{
    #region Instance Values

    protected new readonly Queue<T> _Value;

    #endregion

    #region Constructor

    protected DataExchangeList(T[] value) : base(value)
    {
        _Value = new Queue<T>(value);
    }

    #endregion

    #region Instance Members

    public T[] AsArray() => _Value.Count == 0 ? Array.Empty<T>() : _Value.ToArray();
    public void Clear() => _Value.Clear();
    public int Count() => _Value.Count;
    public bool IsEmpty() => Count() == 0;

    public void Enqueue(T item)
    {
        if (_Value.Contains(item))
            return;

        _Value.Enqueue(item);
    }

    public void Enqueue(T[] items)
    {
        for (nint i = 0; i < items.Length; i++)
            Enqueue(items[i]);
    }

    public bool TryDequeue(out T? result)
    {
        if (!_Value.TryDequeue(out T localResult))
        {
            result = default;

            return false;
        }

        result = localResult;

        return true;
    }

    #endregion
}