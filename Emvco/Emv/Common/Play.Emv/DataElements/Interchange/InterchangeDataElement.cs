﻿using Play.Ber.Identifiers;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

public abstract record InterchangeDataElement<T> : DataElement<T>
{
    #region Constructor

    protected InterchangeDataElement(T value) : base(value)
    { }

    #endregion

    #region Instance Members

    protected static Tag CreateProprietaryTag(byte dataFieldId)
    {
        const ushort proprietaryShortTagIdentifier = 0xC0;
        const ushort proprietaryLongTagIdentifier = 0xC000;

        if (dataFieldId <= 30)
            return new Tag((uint) (proprietaryShortTagIdentifier + dataFieldId));

        return new Tag((uint) (proprietaryLongTagIdentifier + dataFieldId));
    }

    public virtual ushort GetDataFieldId() => (ushort) ((uint) GetTag()).GetMaskedValue(0xC0);

    #endregion
}