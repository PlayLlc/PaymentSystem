using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.DataExchange;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Terminal.Contracts.SignalIn;

namespace Play.Emv.Kernel.DataExchange;

public partial class DataExchangeKernelService
{
    #region Instance Members

    #region Endpoint

    public void SendRequest(KernelSessionId sessionId)
    {
        lock (_Lock)
        {
            if (!_Lock.Responses.ContainsKey(DekRequestType.DataNeeded))
                return;

            QueryTerminalRequest queryKernelResponse = new(new DataExchangeKernelId(sessionId.GetKernelId(), sessionId),
                (DataNeeded) _Lock.Requests[DekRequestType.DataNeeded]);

            _EndpointClient.Send(queryKernelResponse);
            _Lock.Responses[DekRequestType.DataNeeded].Clear();
        }
    }

    #endregion

    #endregion

    #region Query

    /// <exception cref="InvalidOperationException"></exception>
    public bool IsEmpty(DekRequestType type)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(type))
                throw new InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");

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
                throw new InvalidOperationException($"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");

            return _Lock.Requests[type].TryPeek(out result);
        }
    }

    #endregion

    #region Write

    /// <summary>
    ///     Will enqueue the <see cref="Tag" /> values provided in the argument to the data exchange object list specified by
    ///     the <see cref="DekRequestType" /> argument
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listItems"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void Enqueue(DekRequestType type, params Tag[] listItems)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.ContainsKey(type))
                throw new TerminalDataException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");

            _Lock.Requests[type].Enqueue(listItems);
        }
    }

    /// <summary>
    ///     Initializes data exchange objects that contain a list of tags for the Kernel or Terminal to retrieve
    /// </summary>
    /// <param name="list"></param>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    public void Initialize(DataExchangeRequest list)
    {
        lock (_Lock.Requests)
        {
            DekRequestType dekRequestType = DekRequestType.Get(list.GetTag());

            if (!_Lock.Requests.TryGetValue(dekRequestType, out DataExchangeRequest? dataExchangeRequest))
            {
                _ = _Lock.Requests.TryAdd(dekRequestType, DekRequestType.GetDefaultList(dekRequestType));

                return;
            }

            // if the list is already initialized, but isn't empty, then something went wrong. We'll throw here
            if (!dataExchangeRequest.IsEmpty())
            {
                throw new TerminalDataException(
                    $"The {nameof(DataExchangeKernelService)} cannot {nameof(Initialize)} because a non empty list already exists for the {nameof(DekRequestType)} with the tag {dekRequestType}");
            }

            // otherwise we'll just enqueue the empty list
            dataExchangeRequest.Enqueue(list.GetDataObjects());
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    public void Initialize(DekRequestType dekRequestType)
    {
        lock (_Lock.Requests)
        {
            if (!_Lock.Requests.TryGetValue(dekRequestType, out DataExchangeRequest? dataExchangeRequest))
            {
                _ = _Lock.Requests.TryAdd(dekRequestType, DekRequestType.GetDefaultList(dekRequestType));

                return;
            }

            // if the list is already initialized, but isn't empty, then something went wrong. We'll throw here
            if (!dataExchangeRequest.IsEmpty())
            {
                throw new TerminalDataException(
                    $"The {nameof(DataExchangeKernelService)} cannot {nameof(Initialize)} because a non empty list already exists for the {nameof(DekRequestType)} with the tag {dekRequestType}");
            }
        }
    }

    /// <summary>
    ///     Resolves known objects from the <see cref="KernelDatabase" /> for the list specified by the
    ///     <see cref="DekRequestType" />, and returns an integer representing the number of objects remaining to be resolved
    /// </summary>
    /// <param name="requestType"></param>
    /// <returns>
    ///     An integer value that represents the number of data objects yet to be resolved for the list specified by the
    ///     <see cref="DekRequestType" /> in the argument
    /// </returns>
    /// <exception cref="TerminalDataException"></exception>
    public int Resolve(DekRequestType requestType)
    {
        lock (_Lock.Requests)
        {
            if (requestType == DekRequestType.TagsToRead)
                return ResolveTagsToReadYet(_TlvDatabase, _Lock);

            if (requestType == DekRequestType.DataNeeded)
                return ResolveDataNeeded(_TlvDatabase, _Lock);

            throw new TerminalDataException($"The {nameof(DekRequestType)} value: [{requestType}] is not recognized");
        }
    }

    /// <exception cref="TerminalDataException"></exception>
    private static int ResolveDataNeeded(IReadTlvDatabase database, DataExchangeKernelLock dekLock)
    {
        if (!dekLock.Requests.ContainsKey(DekRequestType.DataNeeded))
            throw new TerminalDataException($"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");

        DataNeeded dataNeeded = (DataNeeded) dekLock.Requests[DekRequestType.DataNeeded];
        int unresolvedObjectCount = dataNeeded.Resolve(database);

        return unresolvedObjectCount;
    }

    /// <exception cref="TerminalDataException"></exception>
    private static int ResolveTagsToReadYet(IReadTlvDatabase database, DataExchangeKernelLock dekLock)
    {
        if (!dekLock.Requests.ContainsKey(DekRequestType.TagsToRead))
            throw new TerminalDataException($"The {nameof(DataExchangeKernelService)} could not Dequeue the List Item because the List does not exist");

        if (!dekLock.Responses.ContainsKey(DekResponseType.DataToSend))
            throw new TerminalDataException($"The {nameof(DataExchangeKernelService)} could not Enqueue the List Item because the List does not exist");

        TagsToRead tagsToReadYet = (TagsToRead) dekLock.Requests[DekRequestType.TagsToRead];
        IEnumerable<PrimitiveValue> resolvedObjects = tagsToReadYet.Resolve(database);
        int unresolvedObjectCount = tagsToReadYet.Count();

        // Enqueue the resolved objects so we can inform the Terminal of their values
        dekLock.Responses[DekResponseType.DataToSend].Enqueue(resolvedObjects.ToArray());

        return unresolvedObjectCount;
    }

    #endregion
}