﻿using System.Linq.Expressions;

namespace Play.Persistence.Mongo.Helpers;

public class UpdateFieldConfig<_>
{
    #region Instance Values

    public object? Value { get; set; }

    public Expression<Func<_, object?>>? Field { get; set; }

    #endregion
}