using System;
using System.Collections.Concurrent;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Databases.Tlv;
using Play.Emv.Pcd.Contracts;
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
        ISendTerminalQueryResponse kernelEndpoint)
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

    /// <exception cref="InvalidOperationException"></exception>
    public bool IsEmpty(DekRequestType type)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");
            }

            if (!_Lock.Requests.ContainsKey(type))
                return true;

            if (_Lock.Requests[type].Count() == 0)
                return true;

            return false;
        }
    }

    /// <exception cref="InvalidOperationException"></exception>
    public bool TryPeek(DekRequestType type, out Tag result)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");
            }

            return _Lock.Requests[type].TryPeek(out result);
        }
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    public void Resolve(GetDataResponse dataResponse)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(DekRequestType.TagsToRead))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");
            }

            ((TagsToRead) _Lock.Requests[DekRequestType.TagsToRead]).Resolve(dataResponse.GetTagLengthValueResult());
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
    public void Resolve(IQueryTlvDatabase listType)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(DekRequestType.TagsToRead))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");
            }

            TagsToRead tagsToRead = (TagsToRead) _Lock.Requests[DekRequestType.TagsToRead];

            for (int i = 0; i < tagsToRead.Count(); i++)
            {
                if (!tagsToRead.TryDequeue(out Tag tagToRead))
                    throw new InvalidOperationException();

                if (_TlvDatabase.IsPresentAndNotEmpty(tagToRead))
                    continue;

                tagsToRead.Enqueue(tagToRead);
            }
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

    public void Initialize(DekRequestType listType)
    {
        lock (_Lock)
        {
            if (_Lock.Requests.ContainsKey(listType))
                return;

            _ = _Lock.Requests.TryAdd(listType, DekRequestType.GetDefault(listType));
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
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
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
    public void Enqueue(DekRequestType type, params Tag[] listItems)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Lock.Requests[type].Enqueue(listItems);
        }
    }

    #endregion

    #region Responses

    /// <exception cref="InvalidOperationException"></exception>
    public bool IsEmpty(DekResponseType type)
    {
        lock (_Lock.Responses)
        {
            if (!_Lock.Responses.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            if (!_Lock.Responses.ContainsKey(type))
                return true;

            if (_Lock.Responses[type].Count() == 0)
                return true;

            return false;
        }
    }

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
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not {nameof(SendResponse)} the List Item to the Terminal because the List does not exist");
            }

            // BUG: We're going to need to send the DataToSend to the Terminal without a CorrelationId sometimes

            QueryKernelResponse queryKernelResponse = new(null, (DataToSend) _Lock.Responses[DekResponseType.DataToSend],
                                                          new DataExchangeTerminalId(kernelSessionId.GetKernelId(),
                                                                                     kernelSessionId.GetTransactionSessionId()));

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
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not {nameof(SendResponse)} the List Item to the Terminal because the List does not exist");
            }

            QueryKernelResponse queryKernelResponse = new(correlationId, (DataToSend) _Lock.Responses[DekResponseType.DataToSend],
                                                          new DataExchangeTerminalId(sessionId.GetKernelId(),
                                                                                     sessionId.GetTransactionSessionId()));

            _KernelEndpoint.Send(queryKernelResponse);
            _Lock.Responses[DekResponseType.DataToSend].Clear();
        }
    }

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
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
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
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
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
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
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
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
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
                if (!_Lock.Requests.ContainsKey(DekRequestType.TagsToRead))
                    throw new InvalidOperationException();

                if (!_Lock.Requests[DekRequestType.TagsToRead].TryDequeue(out Tag tagToRead))
                    return 0;

                if (_TlvDatabase.IsPresent(tagToRead))
                    _Lock.Responses[typeType].Enqueue(_TlvDatabase.Get(tagToRead));
                else
                    _Lock.Requests[typeType].Enqueue(tagToRead);
            }

            return _Lock.Requests[typeType].Count();
        }
    }

    #endregion
}