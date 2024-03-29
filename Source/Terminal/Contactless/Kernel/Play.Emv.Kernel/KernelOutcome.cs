﻿using System;

using Play.Ber.Tags;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Kernel.DataExchange;

namespace Play.Emv.Kernel;

internal class KernelOutcome
{
    #region Static Metadata

    private static readonly Tag[] _EmvDiscretionaryData =
    {
        ApplicationCapabilitiesInformation.Tag, ApplicationCurrencyCode.Tag, BalanceReadBeforeGenAc.Tag, BalanceReadAfterGenAc.Tag, DataStorageSummary3.Tag,
        DataStorageSummaryStatus.Tag, ErrorIndication.Tag, PostGenAcPutDataStatus.Tag, PreGenAcPutDataStatus.Tag, ThirdPartyData.Tag, TornRecord.Tag
    };

    private static readonly Tag[] _MagstripeDiscretionaryData =
    {
        ApplicationCapabilitiesInformation.Tag, Track1DiscretionaryData.Tag, Track2DiscretionaryData.Tag, ErrorIndication.Tag, ThirdPartyData.Tag
    };

    #endregion

    #region Instance Members

    /// <summary>
    ///     CreateEmvDiscretionaryData
    /// </summary>
    /// <param name="database"></param>
    /// <param name="dataExchanger"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public static void CreateEmvDiscretionaryData(IReadTlvDatabase database, DataExchangeKernelService dataExchanger)
    {
        // HACK: this logic should live inside discretionary data
        dataExchanger.Initialize(DekResponseType.DiscretionaryData);

        for (nint i = 0; i < _EmvDiscretionaryData.Length; i++)
        {
            if (database.IsPresent(_EmvDiscretionaryData[i]))
                dataExchanger.Enqueue(DekResponseType.DiscretionaryData, database.Get(_EmvDiscretionaryData[i]));
        }
    }

    /// <summary>
    ///     CreateMagstripeDiscretionaryData
    /// </summary>
    /// <param name="database"></param>
    /// <param name="dataExchanger"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public static void CreateMagstripeDiscretionaryData(IReadTlvDatabase database, DataExchangeKernelService dataExchanger)
    {
        // HACK: this logic should live inside discretionary data
        dataExchanger.Initialize(DekResponseType.DiscretionaryData);

        for (nint i = 0; i < _MagstripeDiscretionaryData.Length; i++)
        {
            if (database.IsPresent(_MagstripeDiscretionaryData[i]))
                dataExchanger.Enqueue(DekResponseType.DiscretionaryData, database.Get(_MagstripeDiscretionaryData[i]));
        }
    }

    #endregion
}