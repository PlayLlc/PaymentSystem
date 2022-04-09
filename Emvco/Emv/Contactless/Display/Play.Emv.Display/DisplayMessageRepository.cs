﻿using System;
using System.Collections.Immutable;

using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Emv.Display.Contracts;
using Play.Globalization;

namespace Play.Emv.Display;

public class DisplayMessageRepository
{
    #region Instance Values

    private readonly ImmutableDictionary<CultureProfile, DisplayMessages> _Map;

    #endregion

    #region Constructor

    public DisplayMessageRepository(DisplayMessages[] supportedMessages)
    {
        _Map = supportedMessages.ToImmutableDictionary(a => a.GetCultureProfile(), b => b);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Get
    /// </summary>
    /// <param name="cultureProfile"></param>
    /// <param name="messageIdentifiers"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public DisplayMessage Get(CultureProfile cultureProfile, MessageIdentifiers messageIdentifiers)
    {
        if (!_Map.TryGetValue(cultureProfile, out DisplayMessages? messages))
            throw new InvalidOperationException();

        return messages.Get(messageIdentifiers);
    }

    #endregion
}