﻿namespace Play.Interchange.Messages.DataFields;

public readonly record struct DataFieldId
{
    #region Instance Values

    public readonly byte Value;

    #endregion

    #region Constructor

    internal DataFieldId(byte value)
    {
        Value = value;
    }

    #endregion
}