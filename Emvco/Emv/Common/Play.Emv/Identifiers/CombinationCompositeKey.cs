using System;
using System.Collections.Generic;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Identifiers;

public class CombinationCompositeKey : IEquatable<CombinationCompositeKey>, IEqualityComparer<CombinationCompositeKey>
{
    #region Instance Values

    private readonly DedicatedFileName _DedicatedFileName;
    private readonly ShortKernelIdTypes _KernelIdTypes;
    private readonly TransactionType _TransactionType;

    #endregion

    #region Constructor

    public CombinationCompositeKey(DedicatedFileName dedicatedFileName, ShortKernelIdTypes kernelIdTypes, TransactionType transactionType)
    {
        _DedicatedFileName = dedicatedFileName;
        _KernelIdTypes = kernelIdTypes;
        _TransactionType = transactionType;
    }

    #endregion

    #region Instance Members

    public DedicatedFileName GetApplicationId() => _DedicatedFileName;
    public ShortKernelIdTypes GetKernelId() => _KernelIdTypes;

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
            && (_KernelIdTypes == other._KernelIdTypes)
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
            + (_KernelIdTypes.GetHashCode() * hash)
            + (_TransactionType.GetHashCode() * hash));
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(CombinationCompositeKey left, CombinationCompositeKey right) => left.Equals(right);
    public static bool operator !=(CombinationCompositeKey left, CombinationCompositeKey right) => !left.Equals(right);

    #endregion
}