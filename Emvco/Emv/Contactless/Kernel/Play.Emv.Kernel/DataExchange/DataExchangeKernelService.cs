using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Messaging;

namespace Play.Emv.Kernel.DataExchange;

// HACK: This needs some serious love. This definitely isn't following Single Responsibility Principle. We're doing Double Dispatch with S3R1 because we're storing the DataObjectList items here instead of the TLV database. Take a look at the design of this and refactor

public class DataExchangeKernelService
{
    #region Instance Values

    protected readonly IQueryTlvDatabase _TlvDatabase;
    private readonly ISendTerminalQueryResponse _KernelEndpoint;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly DataExchangeKernelLock _Lock = new();

    #endregion

    #region Constructor

    public DataExchangeKernelService(
        IHandleTerminalRequests terminalEndpoint,
        KernelDatabase kernelDatabase,
        ISendTerminalQueryResponse kernelEndpoint,
        IHandleBlockingPcdRequests pcdEndpoint)
    {
        _TerminalEndpoint = terminalEndpoint;
        _KernelEndpoint = kernelEndpoint;
        _TlvDatabase = kernelDatabase;
    }

    #endregion

    #region Instance Members

    // TODO: Send callback to terminal as well

    #region Clear

    public void Clear()
    {
        lock (_Lock)
        {
            _Lock.Responses.Clear();
            _Lock.Requests.Clear();
        }
    }

    #endregion

    #endregion

    public class DataExchangeKernelLock
    {
        #region Instance Values

        public readonly ConcurrentDictionary<Tag, DataExchangeRequest> Requests;
        public readonly ConcurrentDictionary<Tag, DataExchangeResponse> Responses;

        #endregion

        #region Constructor

        public DataExchangeKernelLock()
        {
            Requests = new ConcurrentDictionary<Tag, DataExchangeRequest>();
            Responses = new ConcurrentDictionary<Tag, DataExchangeResponse>();
        }

        #endregion
    }

    #region Responses

    /// <summary>
    ///     SendResponse
    /// </summary>
    /// <param name="kernelSessionId"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void SendResponse(KernelSessionId kernelSessionId)
    {
        lock (_Lock)
        {
            if (!_Lock.Requests.ContainsKey(DekResponseType.DataToSend))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataExchangeKernelService)} could not {nameof(SendResponse)} the List Item to the Terminal because the List does not exist");
            }

            // BUG: We're going to need to send the DataToSend to the Terminal without a CorrelationId sometimes

            QueryKernelResponse queryKernelResponse = new(null, (DataToSend) _Lock.Responses[DekResponseType.DataToSend],
                new DataExchangeTerminalId(kernelSessionId.GetKernelId(), kernelSessionId.GetTransactionSessionId()));

            _KernelEndpoint.Send(queryKernelResponse);
            _Lock.Responses[DekResponseType.DataToSend].Clear();
        }
    }

    /// <summary>
    ///     SendResponse
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="correlationId"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void SendResponse(KernelSessionId sessionId, CorrelationId correlationId)
    {
        lock (_Lock)
        {
            if (!_Lock.Requests.ContainsKey(DekResponseType.DataToSend))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataExchangeKernelService)} could not {nameof(SendResponse)} the List Item to the Terminal because the List does not exist");
            }

            QueryKernelResponse queryKernelResponse = new(correlationId, (DataToSend) _Lock.Responses[DekResponseType.DataToSend],
                new DataExchangeTerminalId(sessionId.GetKernelId(), sessionId.GetTransactionSessionId()));

            _KernelEndpoint.Send(queryKernelResponse);
            _Lock.Responses[DekResponseType.DataToSend].Clear();
        }
    }

    //public void SendResponse(CorrelationId correlationId, DekResponseType type)
    //{
    //    lock (_Lock)
    //    {
    //        if (!_Lock.Responses.ContainsKey(type))
    //            return;

    //        // TODO: I'm pretty sure we're only supposed to be sending DataToSend. If that's correct, then let's fix this method so that's the only list we're able to send to the terminal

    //        QueryKernelResponse queryKernelResponse = new(correlationId, new DataToSend(_Lock.Responses[type].AsArray()),
    //                                                      new DataExchangeTerminalId(_KernelSessionId.GetKernelId(),
    //                                                                                 _KernelSessionId.GetTransactionSessionId()));

    //        _KernelEndpoint.Send(queryKernelResponse);
    //        _Lock.Responses[type].Clear();
    //    }
    //}

    public void Initialize(DekResponseType listType)
    {
        lock (_Lock)
        {
            if (_Lock.Responses.ContainsKey(listType))
                return;

            _ = _Lock.Responses.TryAdd(listType, DekResponseType.GetDefault(listType));
        }
    }

    public void Initialize(DataExchangeResponse list)
    {
        lock (_Lock)
        {
            if (_Lock.Responses.ContainsKey(list.GetTag()))
                _Lock.Responses[list.GetTag()].Enqueue(list);

            _ = _Lock.Responses.TryAdd(list.GetTag(), list);
        }
    }

    /// <summary>
    ///     Enqueue
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listItem"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(DekResponseType type, TagLengthValue listItem)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Lock.Responses[type].Enqueue(listItem);
        }
    }

    /// <summary>
    ///     Enqueue
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listItems"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(DekResponseType type, TagLengthValue[] listItems)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Lock.Responses[type].Enqueue(listItems);
        }
    }

    /// <summary>
    ///     Enqueue
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listItems"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(DekResponseType type, DataExchangeResponse listItems)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Lock.Responses[type].Enqueue(listItems);
        }
    }

    /// <summary>
    ///     Enqueue
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listItems"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(DekRequestType type, DataExchangeRequest listItems)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Lock.Requests[type].Enqueue(listItems);
        }
    }

    /// <summary>
    ///     Checks for any non-empty values in the database from the remaining list of <see cref="TagsToRead" />. If any values
    ///     are present in the database it will dequeue the <see cref="Tag" /> from TagsToReadYet and Enqueue the
    ///     <see cref="DataToSend" /> buffer with the <see cref="DatabaseValue" />
    /// </summary>
    /// <param name="typeType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public int Resolve(DekResponseType typeType)
    {
        lock (_Lock)
        {
            for (int i = 0; i < _Lock.Responses[typeType].Count(); i++)
            {
                if (!_Lock.Requests[typeType].TryDequeue(out Tag tagToRead))
                    throw new InvalidOperationException();

                if (_TlvDatabase.IsPresentAndNotEmpty(tagToRead))
                    _Lock.Responses[typeType].Enqueue(_TlvDatabase.Get(tagToRead));
                else
                    _Lock.Requests[typeType].Enqueue(tagToRead);
            }

            return _Lock.Requests[typeType].Count();
        }
    }

    #endregion

    #region Requests

    public void SendRequest(KernelSessionId sessionId)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(DekRequestType.DataNeeded))
                return;

            QueryTerminalRequest queryKernelResponse = new(new DataExchangeKernelId(sessionId.GetKernelId(), sessionId),
                (DataNeeded) _Lock.Requests[DekRequestType.DataNeeded]);

            _TerminalEndpoint.Request(queryKernelResponse);
            _Lock.Responses[DekRequestType.DataNeeded].Clear();
        }
    }

    /// <summary>
    ///     Attempts to get requested tags that have not yet been read
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryDequeue(out Tag result)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(DekRequestType.TagsToRead))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");
            }

            return _Lock.Requests[DekRequestType.TagsToRead].TryDequeue(out result);
        }
    }

    /// <summary>
    ///     Checks for any non-empty values in the database from the remaining list of <see cref="TagsToRead" />. If any values
    ///     are present in the database it will dequeue the <see cref="Tag" /> from TagsToReadYet and Enqueue the
    ///     <see cref="DataToSend" />
    ///     buffer with the <see cref="DatabaseValue" />
    /// </summary>
    /// <param name="listType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public int Resolve(DekRequestType listType)
    {
        lock (_Lock.Requests)
        {
            if (listType == DekRequestType.DataNeeded)
                return _Lock.Requests[listType].Count();

            if (listType == DekRequestType.TagsToRead)
                return Resolve((TagsToRead?) _Lock.Requests.GetValueOrDefault(TagsToRead.Tag), _TlvDatabase);

            throw new InvalidOperationException($"The {nameof(DekRequestType)} provided has not been implemented");
        }
    }

    /// <summary>
    ///     Resolve
    /// </summary>
    /// <param name="tagsToRead"></param>
    /// <param name="tlvDatabase"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static int Resolve(TagsToRead? tagsToRead, IQueryTlvDatabase tlvDatabase)
    {
        if (tagsToRead == null)
        {
            throw new InvalidOperationException(
                $"The {nameof(DataExchangeKernelService)} could not Resolve the {nameof(TagsToRead)} because the list has not been initialized");
        }

        for (int i = 0; i < tagsToRead.Count(); i++)
        {
            if (!tagsToRead.TryDequeue(out Tag tagToRead))
                throw new InvalidOperationException();

            if (tlvDatabase.IsPresentAndNotEmpty(tagToRead))
                continue;

            tagsToRead.Enqueue(tagToRead);
        }

        return tagsToRead.Count();
    }

    /// <summary>
    ///     Checks the <see cref="TagLengthValue" /> results provided by the <see cref="GetDataResponse" /> and resolves any
    ///     matching values for <see cref="TagsToRead" />
    /// </summary>
    /// <param name="dataBatchResponse"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public int Resolve(GetDataBatchResponse dataBatchResponse)
    {
        lock (_Lock.Requests)
        {
            TagsToRead tagsToRead = (TagsToRead) _Lock.Requests[DekRequestType.TagsToRead];
            tagsToRead.Resolve(dataBatchResponse.GetTagLengthValuesResult());

            return tagsToRead.Count();
        }
    }

    /// <summary>
    ///     TryGetDataBatchRequest
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public bool TryGetDataBatchRequest(TransactionSessionId sessionId, out GetDataBatchRequest? result)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(TagsToRead.Tag))
            {
                result = null;

                return false;
            }

            result = GetDataBatchRequest.Create(sessionId,
                TagsToRead.Decode(_Lock.Requests[TagsToRead.Tag].EncodeTagLengthValue().AsSpan()));

            return true;
        }
    }

    // TODO: We might only be initializing DataNeeded here
    public void Initialize(DataExchangeRequest list)
    {
        lock (_Lock.Requests)
        {
            if (_Lock.Requests.ContainsKey(list.GetTag()))
                _Lock.Requests[list.GetTag()].Enqueue(list);

            _ = _Lock.Requests.TryAdd(list.GetTag(), list);
        }
    }

    /// <summary>
    ///     Enqueue
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listItem"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(DekRequestType type, Tag listItem)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Lock.Requests[type].Enqueue(listItem);
        }
    }

    /// <summary>
    ///     Enqueue
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listItems"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(DekRequestType type, Tag[] listItems)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Lock.Requests[type].Enqueue(listItems);
        }
    }

    #endregion
}