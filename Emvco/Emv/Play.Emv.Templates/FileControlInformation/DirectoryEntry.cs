#nullable enable
using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements;

namespace Play.Emv.Templates.FileControlInformation;

public class DirectoryEntry : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0x61;

    public static Tag[] ChildTags =
    {
        ApplicationDedicatedFileName.Tag, ApplicationLabel.Tag, ApplicationPriorityIndicator.Tag, ExtendedSelection.Tag,
        KernelIdentifier.Tag
    };

    #endregion

    #region Instance Values

    private readonly ApplicationDedicatedFileName _ApplicationDedicatedFileName;
    private readonly ApplicationLabel? _ApplicationLabel;
    private readonly ApplicationPriorityIndicator _ApplicationPriorityIndicator;
    private readonly ExtendedSelection? _ExtendedSelection;
    private readonly KernelIdentifier? _KernelIdentifier;

    #endregion

    #region Constructor

    public DirectoryEntry(
        ApplicationDedicatedFileName applicationDedicatedFileName,
        ApplicationPriorityIndicator applicationPriorityIndicator,
        ApplicationLabel? applicationLabel,
        KernelIdentifier? kernelIdentifier,
        ExtendedSelection? extendedSelection)
    {
        _ApplicationDedicatedFileName = applicationDedicatedFileName;
        _ApplicationLabel = applicationLabel;
        _ApplicationPriorityIndicator = applicationPriorityIndicator;
        _KernelIdentifier = InitializeKernelIdentifierField(applicationDedicatedFileName, kernelIdentifier);
        _ExtendedSelection = extendedSelection;
    }

    #endregion

    #region Instance Members

    public override Tag[] GetChildTags()
    {
        return ChildTags;
    }

    public ApplicationDedicatedFileName GetApplicationDedicatedFileName()
    {
        return _ApplicationDedicatedFileName;
    }

    //public bool ApplicationCannotBeSelectedWithoutConfirmationByTheCardholder() =>
    //    _ApplicationPriorityIndicator.ApplicationCannotBeSelectedWithoutConfirmationByTheCardholder();

    public ApplicationPriorityRank GetApplicationPriorityRank()
    {
        return _ApplicationPriorityIndicator?.GetApplicationPriorityRank() ?? ApplicationPriorityRank.Fifteenth;
    }

    public bool TryGetKernelIdentifier(out KernelIdentifier? result)
    {
        if (_KernelIdentifier is null)
        {
            result = null;

            return false;
        }

        result = _KernelIdentifier;

        return true;
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        return GetValueByteCount();
    }

    public bool TrGetExtendedSelection(out ExtendedSelection? extendedSelection)
    {
        if (_ExtendedSelection == null)
        {
            extendedSelection = null;

            return false;
        }

        extendedSelection = _ExtendedSelection;

        return true;
    }

    /// <summary>
    ///     TryGetKernelIdentifierType
    /// </summary>
    /// <param name="kernelType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryGetKernelIdentifierType(out KernelType kernelType)
    {
        if (_KernelIdentifier == null)
        {
            kernelType = null;

            return false;
        }

        kernelType = _KernelIdentifier.GetKernelType();

        return true;
    }

    public bool TrGetShortKernelIdentifier(out ShortKernelId shortKernelId)
    {
        if (_KernelIdentifier == null)
        {
            shortKernelId = default;

            return false;
        }

        shortKernelId = _KernelIdentifier.GetShortKernelId();

        return true;
    }

    public bool TryGetApplicationLabel(out ReadOnlySpan<char> applicationLabel)
    {
        if (_ApplicationLabel == null)
        {
            applicationLabel = ReadOnlySpan<char>.Empty;

            return false;
        }

        applicationLabel = _ApplicationLabel;

        return true;
    }

    /// <remarks>
    ///     Book B Section 3.3.2.5 C && Table 3-6
    /// </remarks>
    private static KernelIdentifier? InitializeKernelIdentifierField(
        ApplicationDedicatedFileName applicationDedicatedFileName,
        KernelIdentifier? kernelIdentifier)
    {
        if (!kernelIdentifier?.IsDefaultKernelIdentifierNeeded() ?? true)
            return kernelIdentifier!;

        KernelIdentifier.TryGetDefaultKernelIdentifier(applicationDedicatedFileName, out KernelIdentifier? result);

        return result;
    }

    private static bool TryGetDefaultKernelIdentifier(
        ApplicationDedicatedFileName applicationDedicatedFileName,
        out KernelIdentifier kernelIdentifier)
    {
        return KernelIdentifier.TryGetDefaultKernelIdentifier(applicationDedicatedFileName, out kernelIdentifier);
    }

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[]
        {
            _ApplicationDedicatedFileName, _ApplicationLabel, _ApplicationPriorityIndicator, _ExtendedSelection, _KernelIdentifier
        };
    }

    #endregion

    #region Serialization

    public static DirectoryEntry Decode(ReadOnlyMemory<byte> value)
    {
        return Decode(_Codec.DecodeChildren(value));
    }

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static DirectoryEntry Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        ApplicationLabel? applicationLabel = null;
        KernelIdentifier? kernelIdentifier = null;
        ExtendedSelection? extendedSelection = null;

        ApplicationDedicatedFileName applicationDedicatedFileName =
            encodedTlvSiblings.TryGetValueOctetsOfChild(ApplicationDedicatedFileName.Tag,
                                                        out ReadOnlyMemory<byte> rawApplicationDedicatedFileName)
                ? ApplicationDedicatedFileName.Decode(rawApplicationDedicatedFileName)
                : throw new
                    InvalidOperationException($"A problem occurred while decoding {nameof(DirectoryEntry)}. A {nameof(ApplicationDedicatedFileName)} was expected but could not be found");

        ApplicationPriorityIndicator applicationPriorityIndicator =
            encodedTlvSiblings.TryGetValueOctetsOfChild(ApplicationPriorityIndicator.Tag,
                                                        out ReadOnlyMemory<byte> rawApplicationPriorityIndicator)
                ? ApplicationPriorityIndicator.Decode(rawApplicationPriorityIndicator)
                : new ApplicationPriorityIndicator(0);

        // Nullable values
        if (encodedTlvSiblings.TryGetValueOctetsOfChild(ApplicationLabel.Tag, out ReadOnlyMemory<byte> rawApplicationLabel))
            applicationLabel = (ApplicationLabel?) ApplicationLabel.Decode(rawApplicationLabel);
        if (encodedTlvSiblings.TryGetValueOctetsOfChild(KernelIdentifier.Tag, out ReadOnlyMemory<byte> rawKernelIdentifier))
            kernelIdentifier = (KernelIdentifier?) KernelIdentifier.Decode(rawKernelIdentifier);
        if (encodedTlvSiblings.TryGetValueOctetsOfChild(ExtendedSelection.Tag, out ReadOnlyMemory<byte> rawExtendedSelection))
            extendedSelection = (ExtendedSelection?) ExtendedSelection.Decode(rawExtendedSelection);

        // TryGetDefault
        if ((kernelIdentifier == null) && TryGetDefaultKernelIdentifier(applicationDedicatedFileName, out KernelIdentifier kernelIdDefault))
            kernelIdentifier = kernelIdDefault;

        return new DirectoryEntry(applicationDedicatedFileName, applicationPriorityIndicator, applicationLabel, kernelIdentifier,
                                  extendedSelection);
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj)
    {
        return obj is DirectoryEntry fci && Equals(fci);
    }

    public bool Equals(DirectoryEntry other)
    {
        return _ApplicationDedicatedFileName.Equals(other._ApplicationDedicatedFileName)
            && (_ApplicationLabel?.Equals(other!._ApplicationLabel) ?? (other!._ApplicationLabel == null))
            && _ApplicationPriorityIndicator.Equals(other!._ApplicationPriorityIndicator)
            && (_KernelIdentifier?.Equals(other._KernelIdentifier) ?? (other._KernelIdentifier == null))
            && (_ExtendedSelection?.Equals(_ExtendedSelection, other._ExtendedSelection) ?? (other._ExtendedSelection == null));
    }

    public override bool Equals(ConstructedValue? other)
    {
        return other is DirectoryEntry directoryEntry && Equals(directoryEntry);
    }

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        return Equals(x as DirectoryEntry, y as DirectoryEntry);
    }

    public static bool Equals(DirectoryEntry? x, DirectoryEntry? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode()
    {
        const int hash = 738459837;

        unchecked
        {
            int result = (int) (hash * GetTag());

            result += _ApplicationDedicatedFileName.GetHashCode()
                + (_ApplicationLabel?.GetHashCode() ?? 0)
                + _ApplicationPriorityIndicator.GetHashCode()
                + (_ExtendedSelection?.GetHashCode() ?? 0)
                + (_KernelIdentifier?.GetHashCode() ?? 0);

            return result;
        }
    }

    public override int GetHashCode(ConstructedValue obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}