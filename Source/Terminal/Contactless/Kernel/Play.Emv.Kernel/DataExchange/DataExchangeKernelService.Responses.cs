using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.DataExchange;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel.DataExchange;

public partial class DataExchangeKernelService
{
    #region Instance Members

    #region Endpoint

    /// <summary>
    ///     SendResponse
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="correlationId"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void SendResponse(KernelSessionId sessionId, CorrelationId? correlationId)
    {
        lock (_Lock)
        {
            // BUG: We need to resolve the way we're going to correlate the DET requests and the DEK responses. It's not always going to be a 1 - 1, and we might not even want to always correlate the response with the request
            //throw new NotImplementedException();

            if (!_Lock.Responses.ContainsKey(DekResponseType.DataToSend))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataExchangeKernelService)} could not {nameof(SendResponse)} the List Item to the Terminal because the List does not exist");
            }

            QueryKernelResponse queryKernelResponse = new(correlationId, (DataToSend) _Lock.Responses[DekResponseType.DataToSend],
                new DataExchangeTerminalId(sessionId.GetKernelId(), sessionId.GetTransactionSessionId()));

            _EndpointClient.Send(queryKernelResponse);
            _Lock.Responses[DekResponseType.DataToSend].Clear();
        }
    }

    #endregion

    #endregion

    #region Query

    /// <exception cref="TerminalDataException"></exception>
    public bool IsEmpty(DekResponseType type)
    {
        lock (_Lock.Responses)
        {
            if (!_Lock.Responses.ContainsKey(type))
                throw new TerminalDataException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");

            return _Lock.Responses[type].Count() == 0;
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    public bool TryPeek(DekResponseType type, out PrimitiveValue? result)
    {
        lock (_Lock.Responses)
        {
            if (!_Lock.Responses.ContainsKey(type))
                throw new TerminalDataException($"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");

            return _Lock.Responses[type].TryPeek(out result);
        }
    }

    #endregion

    #region Write

    /// <summary>
    ///     Initializes the data exchange object that contain a list of <see cref="PrimitiveValue" /> objects for the Kernel to
    ///     send to the Card or the Terminal
    /// </summary>
    /// <param name="list"></param>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    public void Initialize(DataExchangeResponse list)
    {
        lock (_Lock.Responses)
        {
            DekResponseType dekResponseType = DekResponseType.Get(list.GetTag());

            if (!_Lock.Responses.TryGetValue(dekResponseType, out DataExchangeResponse? dekExchangeResponse))
            {
                _ = _Lock.Responses.TryAdd(dekResponseType, DekResponseType.GetDefaultList(dekResponseType));

                return;
            }

            // if the list is already initialized, but isn't empty, then something went wrong. We'll throw here
            if (!dekExchangeResponse.IsEmpty())
            {
                throw new TerminalDataException(
                    $"The {nameof(DataExchangeKernelService)} cannot {nameof(Initialize)} because a non empty list already exists for the {nameof(DekResponseType)} with the tag {dekResponseType}");
            }

            // otherwise we'll just enqueue the empty list
            dekExchangeResponse.Enqueue(list.GetDataObjects());
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    public void Initialize(DekResponseType dekResponseType)
    {
        lock (_Lock.Responses)
        {
            if (!_Lock.Responses.TryGetValue(dekResponseType, out DataExchangeResponse? dataExchangeResponse))
            {
                _ = _Lock.Responses.TryAdd(dekResponseType, DekResponseType.GetDefaultList(dekResponseType));

                return;
            }

            // if the list is already initialized, but isn't empty, then something went wrong. We'll throw here
            // Book Emv C-2: 4.3 Lists. -> Initialize
            if (!dataExchangeResponse.IsEmpty())
            {
                dataExchangeResponse.Clear();
            }
        }
    }

    /// <summary>
    ///     Will enqueue the <see cref="PrimitiveValue" /> values provided in the argument to the data exchange object list
    ///     specified by
    ///     the <see cref="DekResponseType" /> argument
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listItems"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Enqueue(DekResponseType type, params PrimitiveValue[] listItems)
    {
        lock (_Lock.Responses)
        {
            if (!_Lock.Responses.TryGetValue(type, out DataExchangeResponse? dekResponseList))
                throw new TerminalDataException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");

            dekResponseList!.Enqueue(listItems);
        }
    }

    /// <summary>
    ///     Resolves known objects from the list of <see cref="PrimitiveValue" /> provided in the argument for the list
    ///     specified by the <see cref="DekResponseType" />, and returns an integer representing the number of objects
    ///     remaining
    ///     to be resolved
    /// </summary>
    /// <param name="responseType"></param>
    /// <param name="values"></param>
    /// <returns>
    ///     An integer value that represents the number of data objects yet to be resolved for the list specified by the
    ///     <see cref="DekResponseType" /> in the argument
    /// </returns>
    /// <exception cref="TerminalDataException"></exception>
    public int Resolve(DekResponseType responseType, params PrimitiveValue[] values)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(responseType))
            {
                throw new TerminalDataException(
                    $"The {nameof(DataExchangeKernelService)} could not {nameof(Resolve)} the {nameof(DekResponseType)} because it has not yet been initialized");
            }

            DataExchangeResponse list = _Lock.Responses[responseType];
            list.Enqueue(values);

            return list.Count();
        }
    }

    #endregion
}