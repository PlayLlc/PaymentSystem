using System.Collections;

using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Selection.Contracts;
using Play.Globalization;
using Play.Icc.FileSystem.DedicatedFiles;

using TransactionProfile = Play.Emv.Selection.Configuration.TransactionProfile;

namespace Play.Emv.Selection;

/// <remarks>
///     This type implements <see cref="IReadOnlyDictionary{TKey,TValue}" /> but the underlying values are mutable
/// </remarks>
public class PreProcessingIndicators : IReadOnlyDictionary<CombinationCompositeKey, PreProcessingIndicator>
{
    #region Instance Values

    private readonly Dictionary<CombinationCompositeKey, PreProcessingIndicator> _Values;
    public int Count => _Values.Count;
    public IEnumerable<CombinationCompositeKey> Keys => _Values.Keys;
    public IEnumerable<PreProcessingIndicator> Values => _Values.Values;

    #endregion

    #region Constructor

    public PreProcessingIndicators(TransactionProfile[] entryPointConfiguration)
    {
        if (!entryPointConfiguration.Any())
            _Values = new Dictionary<CombinationCompositeKey, PreProcessingIndicator>();

        _Values = entryPointConfiguration.ToDictionary(a => a.GetKey(), b => new PreProcessingIndicator(b));
    }

    #endregion

    #region Instance Members

    public bool ContainsKey(CombinationCompositeKey key) => _Values.ContainsKey(key);
    public IEnumerator<KeyValuePair<CombinationCompositeKey, PreProcessingIndicator>> GetEnumerator() => _Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public KernelId[] GetKernelIds() => _Values.Values.Select(a => a.GetKernelId()).ToArray();
    public bool IsExtendedSelectionSupported(CombinationCompositeKey key) => _Values[key].IsExtendedSelectionSupported();

    /// <summary>
    ///     Determines whether the
    ///     <param name="applicationIdentifier" />
    ///     is a Full or Partial Match
    /// </summary>
    /// <param name="applicationIdentifier"></param>
    /// <returns>
    ///     <see cref="bool" />
    /// </returns>
    public bool IsMatchingAid(DedicatedFileName applicationIdentifier)
    {
        List<DedicatedFileName> applicationIdentifiers = _Values.Values.Select(a => a.GetApplicationIdentifier()).ToList();

        if (applicationIdentifiers.Any(a => a.IsFullMatch(applicationIdentifier)))
            return true;

        if (applicationIdentifiers.Any(a => a.IsPartialMatch(applicationIdentifier)))
            return true;

        return false;
    }

    public bool IsMatchingKernel(KernelIdentifier kernelIdentifier)
    {
        return _Values.Values.Where(a => a.ContactlessApplicationNotAllowed == false).Any(a => a.GetKernelId().Equals(kernelIdentifier.AsKernelId()));
    }

    /// <summary>
    ///     This will clear <see cref="TerminalTransactionQualifiers" /> and <see cref="PreProcessingIndicator" /> objects
    /// </summary>
    public void ResetPreprocessingIndicators()
    {
        for (int i = 0; i < _Values.Count; i++)
            _Values.ElementAt(i).Value.ResetPreprocessingIndicators();
    }

    public void ResetTerminalTransactionQualifiers()
    {
        for (int i = 0; i < _Values.Count; i++)
            _Values.ElementAt(i).Value.ResetTerminalTransactionQualifiers();
    }

    /// <summary>
    ///     This will set All fields in accordance to Start A Preprocessing
    /// </summary>
    public void Set(AmountAuthorizedNumeric amountAuthorizedNumeric, CultureProfile cultureProfile)
    {
        for (int i = 0; i < _Values.Count; i++)
            _Values.ElementAt(i).Value.Set(amountAuthorizedNumeric, cultureProfile);
    }

    public bool TryGetValue(CombinationCompositeKey key, out PreProcessingIndicator value) => _Values.TryGetValue(key, out value!);

    #endregion

    public PreProcessingIndicator this[CombinationCompositeKey key] => _Values[key];
}