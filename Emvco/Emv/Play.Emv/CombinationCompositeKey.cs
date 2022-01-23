using System;
using System.Collections.Generic;

using Play.Emv.DataElements;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv;

public class CombinationCompositeKey : IEquatable<CombinationCompositeKey>, IEqualityComparer<CombinationCompositeKey>
{
    #region Instance Values

    private readonly DedicatedFileName _DedicatedFileName;
    private readonly ShortKernelId _KernelId;
    private readonly TransactionType _TransactionType;

    #endregion

    #region Constructor

    public CombinationCompositeKey(DedicatedFileName dedicatedFileName, ShortKernelId kernelId, TransactionType transactionType)
    {
        _DedicatedFileName = dedicatedFileName;
        _KernelId = kernelId;
        _TransactionType = transactionType;
    }

    #endregion

    #region Instance Members

    public DedicatedFileName GetApplicationId() => _DedicatedFileName;
    public ShortKernelId GetKernelId() => _KernelId;

    public RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIndicator() =>
        _DedicatedFileName.GetRegisteredApplicationProviderIdentifier();

    public TransactionType GetTransactionType() => _TransactionType;

    #endregion

    #region Equality

    public bool Equals(CombinationCompositeKey? other)
    {
        if (other == null)
            return false;

        return (_DedicatedFileName == other._DedicatedFileName)
            && (_KernelId == other._KernelId)
            && (_TransactionType == other._TransactionType);
    }

    public bool Equals(CombinationCompositeKey? x, CombinationCompositeKey? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals(object? obj) => obj is CombinationCompositeKey combinationCompositeKey && Equals(combinationCompositeKey);
    public int GetHashCode(CombinationCompositeKey obj) => obj.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 3345659;

        return unchecked((_DedicatedFileName.GetHashCode() * hash)
            + (_KernelId.GetHashCode() * hash)
            + (_TransactionType.GetHashCode() * hash));
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(CombinationCompositeKey left, CombinationCompositeKey right) => left.Equals(right);
    public static bool operator !=(CombinationCompositeKey left, CombinationCompositeKey right) => !left.Equals(right);

    #endregion
}