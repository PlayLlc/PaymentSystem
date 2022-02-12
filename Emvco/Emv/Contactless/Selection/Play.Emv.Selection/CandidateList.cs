using System.Collections;

namespace Play.Emv.Selection;

/// <summary>
///     A Queue of <see cref="Combination" /> to help with Entry Processing. When initialized, the queue is created
///     by enqueue-ing the highest priority combinations.
/// </summary>
public class CandidateList : IReadOnlyCollection<Combination>
{
    #region Instance Values

    private readonly List<Combination> _Value;
    public int Count => _Value.Count;

    #endregion

    #region Constructor

    public CandidateList()
    {
        _Value = new List<Combination>();
    }

    #endregion

    #region Instance Members

    public void Add(Combination item)
    {
        _Value.Add(item);
        _Value.Sort();
    }

    public void AddRange(Combination[] items)
    {
        for (nint i = 0; i < items.Length; i++)
            Add(items[i]);
        _Value.Sort();
    }

    public Combination[] AsArray() => _Value.Count == 0 ? Array.Empty<Combination>() : _Value.ToArray();
    public void Clear() => _Value.Clear();
    public Combination ElementAt(int index) => _Value.ElementAt(index);
    public IEnumerator<Combination> GetEnumerator() => _Value.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool TryGetNext(out Combination? result)
    {
        if (_Value.Count > 0)
        {
            result = _Value.ElementAt(0);
            _Value.Remove(result);

            return true;
        }

        result = default;

        return false;
    }

    #endregion
}