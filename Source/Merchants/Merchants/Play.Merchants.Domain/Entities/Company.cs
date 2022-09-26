﻿using Play.Domain;

namespace Play.Merchants.Domain.Entities;

public class Company : BaseEntity
{
    #region Instance Values

    public string Name { get; set; } = string.Empty;

    #endregion
}