using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security.Authentications.Static;
using Play.Globalization.Time;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Kernel.State;

public class KernelSession
{
    #region Instance Values

    public readonly StopwatchManager Stopwatch = new();
    public readonly TimeoutManager Timer = new();
    private readonly KernelSessionId _KernelSessionId;
    private readonly CorrelationId _CorrelationId;
    private readonly ActiveApplicationFileLocator _ActiveApplicationFileLocator = new();
    private readonly StaticDataToBeAuthenticated _StaticDataToBeAuthenticated = new();

    #endregion

    #region Constructor

    public KernelSession(CorrelationId correlationId, KernelSessionId kernelSessionId)
    {
        _CorrelationId = correlationId;
        _KernelSessionId = kernelSessionId;
    }

    #endregion

    #region Instance Members

    public CorrelationId GetCorrelationId() => _CorrelationId;
    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public TransactionSessionId GetTransactionSessionId() => _KernelSessionId.GetTransactionSessionId();

    #endregion

    #region Static Data To Be Authenticated

    /// <exception cref="Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    public void EnqueueStaticDataToBeAuthenticated(EmvCodec codec, ReadRecordResponse rapdu) =>
        _StaticDataToBeAuthenticated.Enqueue(codec, rapdu);

    /// <param name="tagList"></param>
    /// <param name="database"></param>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Security.Exceptions.CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void EnqueueStaticDataToBeAuthenticated(StaticDataAuthenticationTagList tagList, IQueryTlvDatabase database)
    {
        if (!_ActiveApplicationFileLocator.IsEmpty())
        {
            throw new
                TerminalDataException($"The Kernel attempted to add the {nameof(ApplicationInterchangeProfile)} out of order. The records identified by the {nameof(ApplicationFileLocator)} must enqueue first");
        }

        _StaticDataToBeAuthenticated.Enqueue(tagList, database);
    }

    #endregion

    #region Active Application File Locator

    public void EnqueueActiveTag(RecordRange[] values) => _ActiveApplicationFileLocator.Enqueue(values);
    public bool TryPeekActiveTag(out RecordRange result) => _ActiveApplicationFileLocator.TryPeek(out result);
    public bool IsActiveTagEmpty() => _ActiveApplicationFileLocator.IsEmpty();

    public TagLengthValue[] ResolveActiveTag(ReadRecordResponse rapdu)
    {
        _ = _ActiveApplicationFileLocator.TryDequeue(out RecordRange? result);

        return rapdu.GetRecords();
    }

    public void ClearActiveTags() => _ActiveApplicationFileLocator.Clear();

    #endregion
}