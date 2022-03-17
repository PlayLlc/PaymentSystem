using System;
using System.Collections.Concurrent;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Emv.Terminal.Session;
using Play.Messaging;

namespace Play.Emv.Terminal.DataExchange;

public class DataExchangeTerminalService
{
    #region Instance Values

    private readonly ISendTerminalQueryResponse _TerminalEndpoint;
    private readonly IHandleKernelRequests _KernelEndpoint;
    private readonly DataExchangeTerminalLock _Lock = new();

    #endregion

    #region Constructor

    public DataExchangeTerminalService(ISendTerminalQueryResponse terminalEndpoint, IHandleKernelRequests kernelEndpoint)
    {
        _TerminalEndpoint = terminalEndpoint;
        _KernelEndpoint = kernelEndpoint;
    }

    #endregion

    #region Instance Members

    public void Clear()
    {
        lock (_Lock)
        {
            _Lock.Requests.Clear();
            _Lock.Responses.Clear();
        }
    }

    #endregion

    public class DataExchangeTerminalLock
    {
        #region Instance Values

        public readonly ConcurrentDictionary<Tag, DataExchangeRequest> Requests;
        public readonly ConcurrentDictionary<Tag, DataExchangeResponse> Responses;

        #endregion

        #region Constructor

        public DataExchangeTerminalLock()
        {
            Requests = new ConcurrentDictionary<Tag, DataExchangeRequest>();
            Responses = new ConcurrentDictionary<Tag, DataExchangeResponse>();
        }

        #endregion
    }

    #region Responses

    public void RespondToKernel(CorrelationId correlationId, DataExchangeKernelId dataExchangeKernelId, DetResponseType type)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(type))
                return;

            QueryTerminalResponse queryKernelResponse = new(correlationId, new DataToSend(_Lock.Responses[type].AsArray()),
                                                            dataExchangeKernelId);

            _TerminalEndpoint.Send(queryKernelResponse);
            _Lock.Responses[type].Clear();
        }
    }

    /// <summary>
    ///     Initialize
    /// </summary>
    /// <param name="listType"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Initialize(DetResponseType listType)
    {
        lock (_Lock)
        {
            if (_Lock.Responses.ContainsKey(listType))
                return;

            if (!_Lock.Responses.TryAdd(listType, DetResponseType.GetDefault(listType)))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not initialize the {DetResponseType.GetName(listType)} because it was already initialized");
            }
        }
    }

    /// <summary>
    ///     Initialize
    /// </summary>
    /// <param name="list"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Initialize(DataExchangeResponse list)
    {
        lock (_Lock)
        {
            if (_Lock.Responses.ContainsKey(list.GetTag()))
                _Lock.Responses[list.GetTag()].Enqueue(list);

            if (!_Lock.Responses.TryAdd(list.GetTag(), list))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not initialize the {list.GetType().FullName} because it has already been initialized");
            }
        }
    }

    /// <summary>
    ///     Enqueue
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listItem"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(DetResponseType type, TagLengthValue listItem)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not Enqueue the List Item because the List does not exist");
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
    public void Enqueue(DetResponseType type, TagLengthValue[] listItems)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not Enqueue the List Item because the List does not exist");
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
    public void Enqueue(DetResponseType type, DataExchangeResponse listItems)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not Enqueue the List Item because the List does not exist");
            }

            _Lock.Responses[type].Enqueue(listItems);
        }
    }

    #endregion

    #region Requests

    public void QueryKernel(TransactionSessionId transactionSessionId, KernelId kernelId)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(DetRequestType.TagsToRead))
                return;

            QueryKernelRequest queryKernelRequest = new(new DataExchangeTerminalId(kernelId, transactionSessionId),
                                                        (TagsToRead) _Lock.Requests[DetRequestType.TagsToRead]);

            _KernelEndpoint.Request(queryKernelRequest);
            _Lock.Responses[DetRequestType.TagsToRead].Clear();
        }
    }

    // Hack: Passing in the TerminalSession seems like a hack to me. Is there a better pattern than this? Let's look at the system flow and related objects
    /// <summary>
    ///     Resolve
    /// </summary>
    /// <param name="terminalSession"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Resolve(in TerminalSession terminalSession)
    {
        lock (_Lock)
        {
            if (!_Lock.Requests.ContainsKey(DetRequestType.DataNeeded))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not{nameof(Resolve)} the requested items because the List does not exist");
            }

            DataExchangeRequest dataNeeded = _Lock.Requests[DetRequestType.DataNeeded];
            DataExchangeResponse dataToSend = _Lock.Responses[DetResponseType.DataToSend];

            for (int i = 0; i < dataNeeded.Count(); i++)
            {
                if (!dataNeeded.TryDequeue(out Tag tagRequest))
                    throw new InvalidOperationException();

                // BUG: Resolve the terminal values - these will be the proprietary InterchangeDataElements that are stored in the terminal, values like SystemTraceAuditNumber
                //dataToSend.Enqueue(_TerminalQueryResolver.Resolve(terminalSession, tagRequest));
            }
        }
    }

    public int GetLength(DetRequestType typeItem)
    {
        lock (_Lock)
        {
            return _Lock.Requests[typeItem].Count();
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
        lock (_Lock)
        {
            if (!_Lock.Requests.ContainsKey(DetRequestType.DataNeeded))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not Dequeue the {nameof(DataNeeded)} List Item because it has not been initialized");
            }

            return _Lock.Requests[DetRequestType.DataNeeded].TryDequeue(out result);
        }
    }

    /// <summary>
    ///     Initialize
    /// </summary>
    /// <param name="list"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Initialize(DataExchangeRequest list)
    {
        lock (_Lock)
        {
            if (_Lock.Requests.ContainsKey(list.GetTag()))
                _Lock.Requests[list.GetTag()].Enqueue(list);

            if (!_Lock.Requests.TryAdd(list.GetTag(), list))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not initialize the {list.GetType().FullName} because it has already been initialized");
            }
        }
    }

    /// <summary>
    ///     Enqueue
    /// </summary>
    /// <param name="listItem"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(Tag listItem)
    {
        lock (_Lock)
        {
            if (!_Lock.Requests.ContainsKey(DetRequestType.TagsToRead))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not Enqueue the {nameof(TagsToRead)} List Item because the List does not exist");
            }

            _Lock.Requests[DetRequestType.TagsToRead].Enqueue(listItem);
        }
    }

    /// <summary>
    ///     Enqueue
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listItems"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(DetRequestType type, Tag[] listItems)
    {
        lock (_Lock)
        {
            if (!_Lock.Requests.ContainsKey(type))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not Enqueue the List Item with the {nameof(Tag)}: [{(Tag) type}] because the List does not exist");
            }

            _Lock.Requests[type].Enqueue(listItems);
        }
    }

    /// <summary>
    ///     Enqueue
    /// </summary>
    /// <param name="listItems"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(DataExchangeRequest listItems)
    {
        lock (_Lock)
        {
            if (!_Lock.Requests.ContainsKey(listItems.GetTag()))
            {
                throw new
                    InvalidOperationException($"The {nameof(DataExchangeTerminalService)} could not Enqueue the List Item {listItems.GetType().Name} could not enqueue because the List does not exist");
            }

            _Lock.Requests[listItems.GetTag()].Enqueue(listItems);
        }
    }

    #endregion
}