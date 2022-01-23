﻿using Play.Emv.Configuration;
using Play.Globalization.Language;

namespace Play.Emv.Display;

public interface IUserInterfaceDatabase
{
    #region Instance Members

    public DisplayMessages GetDisplayMessages(Alpha2LanguageCode languageCode);

    #endregion
}