using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Emv.Ber;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;

namespace Play.Emv.Database;

public abstract partial class TlvDatabase : IManageTlvDatabaseLifetime
{
    #region Instance Values

    protected TransactionSessionId? _TransactionSessionId;

    #endregion

    #region Constructor

    protected TlvDatabase(PrimitiveValue[] persistentValues, KnownObjects knownObjects)
    {
        _PersistentConfiguration = persistentValues;
        _KnownObjects = knownObjects;
        _Database = new SortedDictionary<Tag, PrimitiveValue?>(new TagComparer());
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Activate 
    /// </summary>
    /// <param name="transactionSessionId"></param>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public virtual void Activate(TransactionSessionId transactionSessionId)
    {
        if (IsActive())
            throw new InvalidOperationException($"A command to initialize the Kernel Database was invoked but the {nameof(TlvDatabase)} is already active");

        _TransactionSessionId = transactionSessionId;

        SeedDatabase();
    }

    /// <summary>
    ///     Resets the transient values in the database to their default values. The persistent values
    ///     will remain unchanged during the database lifetime
    /// </summary>
    public virtual void Deactivate()
    {
        Clear();
    }

    protected bool IsActive() => _TransactionSessionId != null;
    public TransactionSessionId? GetTransactionSessionId() => _TransactionSessionId;

    #endregion
}