using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Kernel.Contracts.SignalOut;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Messaging;

namespace Play.Emv.Kernel.DataExchange;

public class DataExchangeKernelService
{
    #region Instance Values

    protected readonly Dictionary<Tag, DataExchangeRequest> _Requests;
    protected readonly Dictionary<Tag, DataExchangeResponse> _Responses;
    protected readonly IQueryTlvDatabase _TlvDatabase;
    private readonly ISendTerminalQueryResponse _KernelEndpoint;
    private readonly KernelSessionId _KernelSessionId;
    private readonly IHandleTerminalRequests _TerminalEndpoint;

    #endregion

    #region Constructor

    public DataExchangeKernelService(
        KernelSessionId sessionId,
        IHandleTerminalRequests terminalEndpoint,
        KernelDatabase kernelDatabase,
        ISendTerminalQueryResponse kernelEndpoint)
    {
        _KernelSessionId = sessionId;
        _TerminalEndpoint = terminalEndpoint;
        _Requests = new Dictionary<Tag, DataExchangeRequest>();
        _Responses = new Dictionary<Tag, DataExchangeResponse>();
        _TlvDatabase = kernelDatabase;
        _KernelEndpoint = kernelEndpoint;
    }

    #endregion

    // TODO: Send callback to terminal as well 

    #region Responses

    public void RespondToTerminal(CorrelationId correlationId, DekResponseType type)
    {
        lock (_Responses)
        {
            if (!_Responses.ContainsKey(type))
                return;

            // TODO: I'm pretty sure we're only supposed to be sending DataToSend. If that's correct, then let's fix this method so that's the only list we're able to send to the terminal

            QueryKernelResponse queryKernelResponse = new(correlationId, new DataToSend(_Responses[type].AsArray()),
                                                          new DataExchangeTerminalId(_KernelSessionId.GetKernelId(),
                                                                                     _KernelSessionId.GetTransactionSessionId()));

            _KernelEndpoint.Send(queryKernelResponse);
            _Responses[type].Clear();
        }
    }

    public void Initialize(DekResponseType listType)
    {
        lock (_Responses)
        {
            if (_Responses.ContainsKey(listType))
                return;

            _Responses.Add(listType, DekResponseType.GetDefault(listType));
        }
    }

    public void Initialize(DataExchangeResponse list)
    {
        lock (_Responses)
        {
            if (_Responses.ContainsKey(list.GetTag()))
                _Responses[list.GetTag()].Enqueue(list);

            _Responses.Add(list.GetTag(), list);
        }
    }

    public void Enqueue(DekResponseType type, TagLengthValue listItem)
    {
        lock (_Responses)
        {
            if (!_Responses.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Responses[type].Enqueue(listItem);
        }
    }

    public void Enqueue(DekResponseType type, TagLengthValue[] listItems)
    {
        lock (_Responses)
        {
            if (!_Responses.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Responses[type].Enqueue(listItems);
        }
    }

    public void Enqueue(DekResponseType type, DataExchangeResponse listItems)
    {
        lock (_Responses)
        {
            if (!_Responses.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Responses[type].Enqueue(listItems);
        }
    }

    #endregion

    #region Requests

    public void QueryTerminal()
    {
        lock (_Responses)
        {
            if (!_Responses.ContainsKey(DekRequestType.DataNeeded))
                return;

            QueryTerminalRequest queryKernelResponse = new(new DataExchangeKernelId(_KernelSessionId.GetKernelId(), _KernelSessionId),
                                                           (DataNeeded) _Requests[DekRequestType.DataNeeded]);

            _TerminalEndpoint.Request(queryKernelResponse);
            _Responses[DekRequestType.DataNeeded].Clear();
        }
    }

    public int GetLength(DekRequestType typeItem)
    {
        lock (_Requests)
            return _Requests[typeItem].Count();
    }

    /// <summary>
    ///     Attempts to get requested tags that have not yet been read
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryDequeue(out Tag result)
    {
        lock (_Requests)
        {
            if (!_Requests.ContainsKey(DekRequestType.TagsToRead))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");
            }

            return _Requests[DekRequestType.TagsToRead].TryDequeue(out result);
        }
    }

    /// <summary>
    ///     Checks for any non-empty values in the database from the remaining list of <see cref="TagsToRead" />. If any values
    ///     are
    ///     present in the database it will dequeue the <see cref="Tag" /> from TagsToReadYet and Enqueue the
    ///     <see cref="DataToSend" />
    ///     buffer with the <see cref="DatabaseValue" />
    /// </summary>
    /// <param name="listType"></param>
    /// <returns></returns>
    public int Resolve(DekRequestType listType)
    {
        lock (_Requests)
        {
            if (listType == DekRequestType.DataNeeded)
                return _Requests[listType].Count();

            for (int i = 0; i < _Requests[DekRequestType.TagsToRead].Count(); i++)
            {
                if (!_Requests[listType].TryDequeue(out Tag tagToRead))
                    throw new InvalidOperationException();

                if (_TlvDatabase.IsPresentAndNotEmpty(tagToRead))
                    _Responses[DekResponseType.DataToSend].Enqueue(_TlvDatabase.Get(tagToRead));
                else
                    _Requests[listType].Enqueue(tagToRead);
            }

            return _Requests[listType].Count();
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
        lock (_Responses)
        {
            for (int i = 0; i < _Responses[typeType].Count(); i++)
            {
                if (!_Requests[typeType].TryDequeue(out Tag tagToRead))
                    throw new InvalidOperationException();

                if (_TlvDatabase.IsPresentAndNotEmpty(tagToRead))
                    _Responses[typeType].Enqueue(_TlvDatabase.Get(tagToRead));
                else
                    _Requests[typeType].Enqueue(tagToRead);
            }

            return _Requests[typeType].Count();
        }
    }

    ///// <summary>
    /////     Checks for any non-empty values in the database from the remaining list of <see cref="TagsToRead" />. If any values
    /////     are present in the database it will dequeue the <see cref="Tag" /> from TagsToReadYet and Enqueue the
    /////     <see cref="DataToSend" /> buffer with the <see cref="DatabaseValue" />
    ///// </summary>
    ///// <param name="typeType"></param>
    ///// <returns></returns>
    ///// <exception cref="InvalidOperationException"></exception>
    //public int Resolve(DekResponseType typeType)
    //{
    //    lock (_Responses)
    //    {
    //        for (int i = 0; i < _Responses[typeType].Count(); i++)
    //        {
    //            if (!_Requests[typeType].TryDequeue(out Tag tagToRead))
    //                throw new InvalidOperationException();

    //            if (_TlvDatabase.IsPresentAndNotEmpty(tagToRead))
    //                _Responses[typeType].Enqueue(_TlvDatabase.Get(tagToRead));
    //            else
    //                _Requests[typeType].Enqueue(tagToRead);
    //        }

    //        return _Requests[typeType].Count();
    //    }
    //}

    public void Initialize(DataExchangeRequest list)
    {
        lock (_Requests)
        {
            if (_Requests.ContainsKey(list.GetTag()))
                _Requests[list.GetTag()].Enqueue(list);

            _Requests.Add(list.GetTag(), list);
        }
    }

    public void Enqueue(DekRequestType type, Tag listItem)
    {
        lock (_Requests)
        {
            if (!_Requests.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Requests[type].Enqueue(listItem);
        }
    }

    public void Enqueue(DekRequestType type, Tag[] listItems)
    {
        lock (_Requests)
        {
            if (!_Requests.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Requests[type].Enqueue(listItems);
        }
    }

    public void Enqueue(DekRequestType type, DataExchangeRequest listItems)
    {
        lock (_Requests)
        {
            if (!_Requests.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");
            }

            _Requests[type].Enqueue(listItems);
        }
    }

    #endregion
}