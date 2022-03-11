﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Kernel.DataExchange;

namespace Play.Emv.Kernel;

internal class KernelOutcome
{
    #region Static Metadata

    private static readonly Tag[] _EmvDiscretionaryData = new Tag[]
    {
        ApplicationCapabilitiesInformation.Tag, ApplicationCurrencyCode.Tag, BalanceReadBeforeGenAc.Tag, BalanceReadAfterGenAc.Tag,
        DataStorageSummary3.Tag, DataStorageSummaryStatus.Tag, ErrorIndication.Tag, PostGenAcPutDataStatus.Tag,
        PreGenAcPutDataStatus.Tag, ThirdPartyData.Tag, TornRecord.Tag
    };

    private static readonly Tag[] _MagstripeDiscretionaryData = new Tag[]
    {
        ApplicationCapabilitiesInformation.Tag, DiscretionaryDataCardTrack1.Tag, DiscretionaryDataCardTrack2.Tag, ErrorIndication.Tag,
        ThirdPartyData.Tag
    };

    #endregion

    #region Instance Members

    public static void CreateEmvDiscretionaryData(IQueryTlvDatabase database, DataExchangeKernelService dataExchanger)
    {
        dataExchanger.Initialize(DekResponseType.DiscretionaryData);

        for (nint i = 0; i < _EmvDiscretionaryData.Length; i++)
        {
            if (database.IsPresent(_EmvDiscretionaryData[i]))
                dataExchanger.Enqueue(DekResponseType.DiscretionaryData, database.Get(_EmvDiscretionaryData[i]));
        }
    }

    public static void CreateMagstripeDiscretionaryData(IQueryTlvDatabase database, DataExchangeKernelService dataExchanger)
    {
        dataExchanger.Initialize(DekResponseType.DiscretionaryData);

        for (nint i = 0; i < _MagstripeDiscretionaryData.Length; i++)
        {
            if (database.IsPresent(_MagstripeDiscretionaryData[i]))
                dataExchanger.Enqueue(DekResponseType.DiscretionaryData, database.Get(_MagstripeDiscretionaryData[i]));
        }
    }

    #endregion
}