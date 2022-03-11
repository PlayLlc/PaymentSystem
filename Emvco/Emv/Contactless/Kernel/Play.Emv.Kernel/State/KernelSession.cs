﻿using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Kernel.Exceptions;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security.Authentications.Static;
using Play.Emv.Sessions;
using Play.Globalization.Time;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Kernel.State;

public class KernelSession
{
    #region Instance Values

    private readonly KernelSessionId _KernelSessionId;
    private readonly TimeoutManager _TimeoutManager;
    private readonly CorrelationId _CorrelationId;
    private readonly ActiveApplicationFileLocator _ActiveApplicationFileLocator = new();
    private readonly StaticDataToBeAuthenticated _StaticDataToBeAuthenticated = new();

    #endregion

    #region Constructor

    public KernelSession(CorrelationId correlationId, KernelSessionId kernelSessionId)
    {
        _CorrelationId = correlationId;
        _KernelSessionId = kernelSessionId;
        _TimeoutManager = new TimeoutManager();
    }

    #endregion

    #region Instance Members

    public CorrelationId GetCorrelationId() => _CorrelationId;
    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public TransactionSessionId GetTransactionSessionId() => _KernelSessionId.GetTransactionSessionId();

    #endregion

    #region Static Data To Be Authenticated

    public void EnqueueStaticDataToBeAuthenticated(EmvCodec codec, ReadRecordResponse rapdu) =>
        _StaticDataToBeAuthenticated.Enqueue(codec, rapdu);

    /// <param name="tagList"></param>
    /// <param name="database"></param>
    /// <exception cref="TerminalDataException"></exception>
    public void EnqueueStaticDataToBeAuthenticated(StaticDataAuthenticationTagList tagList, IQueryTlvDatabase database)
    {
        if (!_ActiveApplicationFileLocator.IsEmpty())
        {
            throw new TerminalDataException(
                $"The Kernel attempted to add the {nameof(ApplicationInterchangeProfile)} out of order. The records identified by the {nameof(ApplicationFileLocator)} must enqueue first");
        }

        _StaticDataToBeAuthenticated.Enqueue(tagList, database);
    }

    #endregion

    #region Active Application File Locator

    public void EnqueueActiveTag(RecordRange[] values) => _ActiveApplicationFileLocator.Enqueue(values);
    public bool TryPeekActiveTag(out RecordRange result) => _ActiveApplicationFileLocator.TryPeek(out result);

    public TagLengthValue[] ResolveActiveTag(ReadRecordResponse rapdu)
    {
        _ = _ActiveApplicationFileLocator.TryDequeue(out RecordRange? result);

        return rapdu.GetRecords();
    }

    public void ClearActiveTags() => _ActiveApplicationFileLocator.Clear();

    #endregion

    #region Timeout Management

    public void StartTimeout(Milliseconds timeout, Action timeoutHandler) => _TimeoutManager.Start(timeout, timeoutHandler);
    public void StopTimeout() => _TimeoutManager.Stop();
    public bool TimedOut() => _TimeoutManager.TimedOut();

    #endregion
}