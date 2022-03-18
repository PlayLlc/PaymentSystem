﻿using Play.Ber.Identifiers;
using Play.Emv.Ber;

namespace Play.Emv.Templates;

public abstract class FileControlInformationTemplate : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0x6F;

    #endregion

    #region Instance Members

    public abstract FileControlInformationProprietaryTemplate GetFileControlInformationProprietary();

    #endregion
}