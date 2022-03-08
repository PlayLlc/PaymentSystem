using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.Kernel.State;

public class ActiveApplicationFileLocator
{
    #region Instance Values

    private readonly Queue<RecordRange> _Value;

    #endregion

    #region Constructor

    public ActiveApplicationFileLocator()
    {
        _Value = new Queue<RecordRange>();
    }

    #endregion

    #region Instance Members

    public void Clear()
    {
        _Value.Clear();
    }

    public int Count() => _Value.Count;
    public bool IsEmpty() => Count() == 0;

    public void Enqueue(RecordRange item)
    {
        if (_Value.Contains(item))
            return;

        _Value.Enqueue(item);
    }

    public void Enqueue(RecordRange[] items)
    {
        for (nint i = 0; i < items.Length; i++)
            Enqueue(items[i]);
    }

    public bool TryDequeue(out RecordRange? result)
    {
        if (!_Value.TryDequeue(out RecordRange localResult))
        {
            result = default;

            return false;
        }

        result = localResult;

        return true;
    }

    #endregion
}