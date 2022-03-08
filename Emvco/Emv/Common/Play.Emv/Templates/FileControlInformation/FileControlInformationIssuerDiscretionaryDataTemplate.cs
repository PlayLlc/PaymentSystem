﻿using System;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.Templates;

public abstract class FileControlInformationIssuerDiscretionaryDataTemplate : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0xBF06;

    #endregion

    #region Instance Values

    protected PrimitiveValue[] _Values;

    #endregion

    #region Constructor

    protected FileControlInformationIssuerDiscretionaryDataTemplate()
    {
        _Values = Array.Empty<PrimitiveValue>();
    }

    protected FileControlInformationIssuerDiscretionaryDataTemplate(PrimitiveValue[] values)
    {
        _Values = values;
    }

    #endregion

    #region Instance Members

    public PrimitiveValue[] GetOptionalValues() => _Values;

    #endregion
}