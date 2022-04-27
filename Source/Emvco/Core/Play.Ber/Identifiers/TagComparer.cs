using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Ber.Identifiers
{
    public class TagComparer : IComparer<Tag>
    {
        #region Instance Members

        public int Compare(Tag x, Tag y) => x.CompareTo(y);

        #endregion
    }
}