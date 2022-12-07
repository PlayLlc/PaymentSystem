﻿using System.Linq.Expressions;

namespace Play.Persistence.Mongo.Helpers;

public class SortConfig<_>
{
    #region Instance Values

    public SortOrder SortOrder { get; set; }

    public Expression<Func<_, object>>? SortBy { get; set; }

    #endregion
}