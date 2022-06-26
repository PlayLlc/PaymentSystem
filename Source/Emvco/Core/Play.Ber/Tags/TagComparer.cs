using System.Collections.Generic;

namespace Play.Ber.Identifiers;

public class TagComparer : IComparer<Tag>
{
    #region Instance Members

    public int Compare(Tag x, Tag y) => x.CompareTo(y);

    #endregion
}