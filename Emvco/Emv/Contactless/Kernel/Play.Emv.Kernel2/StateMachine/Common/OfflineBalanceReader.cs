using System;

using Play.Ber.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine.Common
{
    /// <summary>
    ///     A Card may have an offline balance, and some products require the balance to be read and made available to the
    ///     customer, either on a receipt or on a display. Not all cards support balance reading and those that do explicitly
    ///     indicate it in the Application Capabilities Information.
    /// </summary>
    /// <remarks>EMVco Book C-2 Section 3.10</remarks>
    //public class OfflineBalanceReader : CommonProcessing
    //{
    //    #region Instance Values

    //    protected readonly KernelDatabase _KernelDatabase;
    //    protected readonly DataExchangeKernelService _DataExchangeKernelService;
    //    private readonly IKernelEndpoint _KernelEndpoint;
    //    private readonly IGetKernelState _KernelStateResolver;
    //    private readonly IHandlePcdRequests _PcdEndpoint;

    //    protected override StateId[] _ValidStateIds { get; } =
    //    {
    //        WaitingForEmvReadRecordResponse.StateId, WaitingForGetDataResponse.StateId, WaitingForEmvModeFirstWriteFlag.StateId
    //    };

    //    #endregion

    //    #region Constructor

    //    public OfflineBalanceReader(
    //        KernelDatabase kernelDatabase,
    //        DataExchangeKernelService dataExchangeKernelService,
    //        IKernelEndpoint kernelEndpoint,
    //        IGetKernelState kernelStateResolver,
    //        IHandlePcdRequests pcdEndpoint)
    //    {
    //        _KernelDatabase = kernelDatabase;
    //        _DataExchangeKernelService = dataExchangeKernelService;
    //        _KernelEndpoint = kernelEndpoint;
    //        _KernelStateResolver = kernelStateResolver;
    //        _PcdEndpoint = pcdEndpoint;
    //    }

    //    #endregion

    //    #region Instance Members

    //    /// <exception cref="BerParsingException"></exception>
    //    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    //    /// <exception cref="TerminalDataException"></exception>
    //    /// <exception cref="InvalidOperationException"></exception>
    //    /// <exception cref="Play.Emv.Exceptions.RequestOutOfSyncException"></exception>
    //    public override KernelState Process(IGetKernelStateId kernelStateId, Kernel2Session session)
    //    { }

    //    #endregion
    //}
}