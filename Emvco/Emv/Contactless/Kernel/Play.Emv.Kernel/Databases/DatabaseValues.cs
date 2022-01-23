﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;

namespace Play.Emv.Kernel.Databases;

public class DatabaseValues : IReadOnlyCollection<DatabaseValue> // IEnumerable<DatabaseValue>
{
    #region Instance Values

    private readonly List<DatabaseValue> _Values;
    public int Count => _Values.Count;

    #endregion

    #region Constructor

    public DatabaseValues(params DatabaseValue[] values)
    {
        _Values = values.ToList();
    }

    public DatabaseValues(params TagLengthValue[] values)
    {
        _Values = values.Select(a => new DatabaseValue(a)).ToList();
    }

    #endregion

    #region Instance Members

    public byte[] EncodeTagLengthValues()
    {
        return _Values.SelectMany(a => a.EncodeTagLengthValue()).ToArray();
    }

    public IEnumerator<DatabaseValue> GetEnumerator() => _Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int GetTagLengthValueByteCount() => checked((int) _Values.Sum(a => a.GetTagLengthValueByteCount()));

    #endregion
}