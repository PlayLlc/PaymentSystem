using Play.Core.Extensions;
using Play.Emv.DataElements;
using Play.Emv.Templates.FileControlInformation;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Selection.Contracts;

public class Combination : IEqualityComparer<Combination>, IEquatable<Combination>, IComparable<Combination>
{
    #region Instance Values

    private readonly DedicatedFileName _ApplicationId;
    private readonly ApplicationPriorityRank _ApplicationPriorityRank;
    private readonly CombinationCompositeKey _CombinationCompositeKey;
    private readonly PreProcessingIndicator _PreProcessingIndicator;

    #endregion

    #region Constructor

    protected Combination(
        CombinationCompositeKey key,
        DedicatedFileName applicationId,
        ApplicationPriorityRank applicationPriorityRank,
        PreProcessingIndicator preProcessingIndicator)
    {
        _CombinationCompositeKey = key;
        _ApplicationId = applicationId;
        _ApplicationPriorityRank = applicationPriorityRank;
        _PreProcessingIndicator = preProcessingIndicator;
    }

    #endregion

    #region Instance Members

    public int CompareTo(Combination? other)
    {
        if (other == null)
            return -1;

        if (_ApplicationPriorityRank > other._ApplicationPriorityRank)
            return -1;

        if (_ApplicationPriorityRank == other._ApplicationPriorityRank)
            return 0;

        return 1;
    }

    public static Combination Create(DirectoryEntry directoryEntry, PreProcessingIndicator preProcessingIndicator)
    {
        if (!preProcessingIndicator.IsExtendedSelectionSupported())
        {
            return new Combination(preProcessingIndicator.GetKey(), preProcessingIndicator.GetApplicationIdentifier(),
                directoryEntry.GetApplicationPriorityRank(), preProcessingIndicator);
        }

        DedicatedFileName? applicationId = directoryEntry.TrGetExtendedSelection(out ExtendedSelection? extendedSelectionResult)
            ? new DedicatedFileName(preProcessingIndicator.GetApplicationIdentifier().AsByteArray()
                .ConcatArrays(extendedSelectionResult!.AsByteArray()))
            : preProcessingIndicator.GetApplicationIdentifier();

        return new Combination(preProcessingIndicator.GetKey(), applicationId, directoryEntry.GetApplicationPriorityRank(),
            preProcessingIndicator);
    }

    public DedicatedFileName GetApplicationIdentifier() => _ApplicationId;
    public CombinationCompositeKey GetCombinationCompositeKey() => _CombinationCompositeKey;
    public ShortKernelId GetKernelId() => _CombinationCompositeKey.GetKernelId();

    #endregion

    #region Equality

    public bool Equals(Combination? x, Combination? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x._CombinationCompositeKey == y._CombinationCompositeKey;
    }

    public bool Equals(Combination? other) => throw new NotImplementedException();
    public int GetHashCode(Combination obj) => throw new NotImplementedException();

    #endregion
}