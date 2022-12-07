using System;

using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Outcomes;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel.Databases;

public partial class KernelDatabase
{
    #region Instance Members

    /// <summary>
    ///     CreateEmvDiscretionaryData
    /// </summary>
    /// <param name="dataExchanger"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void CreateEmvDiscretionaryData(DataExchangeKernelService dataExchanger)
    {
        // HACK: this logic should live inside discretionary data
        KernelOutcome.CreateEmvDiscretionaryData(this, dataExchanger);
    }

    /// <exception cref="TerminalDataException"></exception>
    public void CreateEmvDataRecord(DataExchangeKernelService dataExchanger) =>
        dataExchanger.Enqueue(DekResponseType.DiscretionaryData, DataRecord.CreateEmvDataRecord(this));

    /// <exception cref="TerminalDataException"></exception>
    public void CreateMagstripeDataRecord(DataExchangeKernelService dataExchanger) =>
        dataExchanger.Enqueue(DekResponseType.DiscretionaryData, DataRecord.CreateMagstripeDataRecord(this));

    /// <summary>
    ///     CreateMagstripeDiscretionaryData
    /// </summary>
    /// <param name="dataExchanger"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public void CreateMagstripeDiscretionaryData(DataExchangeKernelService dataExchanger)
    {
        KernelOutcome.CreateMagstripeDiscretionaryData(this, dataExchanger);
    }

    #endregion
}