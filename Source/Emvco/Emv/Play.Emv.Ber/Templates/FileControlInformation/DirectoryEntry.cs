#nullable enable
using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.Templates;

public record DirectoryEntry : Template
{
    #region Static Metadata

    public static readonly Tag Tag = 0x61;

    public static Tag[] ChildTags =
    {
        ApplicationDedicatedFileName.Tag, ApplicationLabel.Tag, ApplicationPriorityIndicator.Tag, ExtendedSelection.Tag, KernelIdentifier.Tag
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
        ApplicationDedicatedFileName applicationDedicatedFileName, ApplicationPriorityIndicator applicationPriorityIndicator,
        ApplicationLabel? applicationLabel, KernelIdentifier? kernelIdentifier, ExtendedSelection? extendedSelection)
    {
        _ApplicationDedicatedFileName = applicationDedicatedFileName;
        _ApplicationLabel = applicationLabel;
        _ApplicationPriorityIndicator = applicationPriorityIndicator;
        _KernelIdentifier = InitializeKernelIdentifierField(applicationDedicatedFileName, kernelIdentifier);
        _ExtendedSelection = extendedSelection;
    }

    #endregion

    #region Serialization

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DirectoryEntry Decode(ReadOnlyMemory<byte> value)
    {
        if (_Codec.DecodeFirstTag(value.Span) == Tag)
            return Decode(_Codec.DecodeChildren(value));

        return Decode(_Codec.DecodeSiblings(value));
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    public static DirectoryEntry Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        ApplicationLabel? applicationLabel = null;
        KernelIdentifier? kernelIdentifier = null;
        ExtendedSelection? extendedSelection = null;

        ApplicationDedicatedFileName applicationDedicatedFileName =
            encodedTlvSiblings.TryGetValueOctetsOfSibling(ApplicationDedicatedFileName.Tag, out ReadOnlyMemory<byte> rawApplicationDedicatedFileName)
                ? ApplicationDedicatedFileName.Decode(rawApplicationDedicatedFileName)
                : throw new CardDataMissingException(
                    $"A problem occurred while decoding {nameof(DirectoryEntry)}. A {nameof(ApplicationDedicatedFileName)} was expected but could not be found");

        ApplicationPriorityIndicator applicationPriorityIndicator =
            encodedTlvSiblings.TryGetValueOctetsOfSibling(ApplicationPriorityIndicator.Tag, out ReadOnlyMemory<byte> rawApplicationPriorityIndicator)
                ? ApplicationPriorityIndicator.Decode(rawApplicationPriorityIndicator)
                : new ApplicationPriorityIndicator(0);

        // Nullable values
        if (encodedTlvSiblings.TryGetValueOctetsOfSibling(ApplicationLabel.Tag, out ReadOnlyMemory<byte> rawApplicationLabel))
            applicationLabel = (ApplicationLabel?) ApplicationLabel.Decode(rawApplicationLabel);
        if (encodedTlvSiblings.TryGetValueOctetsOfSibling(KernelIdentifier.Tag, out ReadOnlyMemory<byte> rawKernelIdentifier))
            kernelIdentifier = (KernelIdentifier?) KernelIdentifier.Decode(rawKernelIdentifier);
        if (encodedTlvSiblings.TryGetValueOctetsOfSibling(ExtendedSelection.Tag, out ReadOnlyMemory<byte> rawExtendedSelection))
            extendedSelection = (ExtendedSelection?) ExtendedSelection.Decode(rawExtendedSelection);

        // TryGetDefault
        if ((kernelIdentifier == null) && TryGetDefaultKernelIdentifier(applicationDedicatedFileName, out KernelIdentifier kernelIdDefault))
            kernelIdentifier = kernelIdDefault;

        return new DirectoryEntry(applicationDedicatedFileName, applicationPriorityIndicator, applicationLabel, kernelIdentifier, extendedSelection);
    }

    #endregion

    #region Instance Members

    public override Tag[] GetChildTags() => ChildTags;
    public ApplicationDedicatedFileName GetApplicationDedicatedFileName() => _ApplicationDedicatedFileName;

    //public bool ApplicationCannotBeSelectedWithoutConfirmationByTheCardholder() =>
    //    _ApplicationPriorityIndicator.ApplicationCannotBeSelectedWithoutConfirmationByTheCardholder();

    public ApplicationPriorityRank GetApplicationPriorityRank() =>
        _ApplicationPriorityIndicator?.GetApplicationPriorityRank() ?? ApplicationPriorityRankTypes.Fifteenth;

    public bool TryGetKernelIdentifier(out KernelIdentifier? result)
    {
        if (_KernelIdentifier is not null)
        {
            result = _KernelIdentifier;

            return true;
        }

        if (ShortKernelIdTypes.Empty.TryGet(_ApplicationDedicatedFileName.GetRegisteredApplicationProviderIndicator(), out ShortKernelIdTypes? kernelType))
        {
            result = new KernelIdentifier(kernelType!);

            return true;
        }

        result = null;

        return false;
    }

    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => GetValueByteCount();

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
    /// <exception cref="DataElementParsingException"></exception>
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

    /// <summary>
    ///     TrGetShortKernelIdentifier
    /// </summary>
    /// <param name="shortKernelIdTypes"></param>
    /// <returns></returns>
    /// <exception cref="DataElementParsingException"></exception>
    public bool TrGetShortKernelIdentifier(out ShortKernelIdTypes shortKernelIdTypes)
    {
        if (_KernelIdentifier == null)
        {
            shortKernelIdTypes = default;

            return false;
        }

        shortKernelIdTypes = _KernelIdentifier.GetShortKernelId();

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
        ApplicationDedicatedFileName applicationDedicatedFileName, KernelIdentifier? kernelIdentifier)
    {
        if (!kernelIdentifier?.IsDefaultKernelIdentifierNeeded() ?? true)
            return kernelIdentifier!;

        KernelIdentifier.TryGetDefaultKernelIdentifier(applicationDedicatedFileName, out KernelIdentifier? result);

        return result;
    }

    private static bool TryGetDefaultKernelIdentifier(ApplicationDedicatedFileName applicationDedicatedFileName, out KernelIdentifier kernelIdentifier) =>
        KernelIdentifier.TryGetDefaultKernelIdentifier(applicationDedicatedFileName, out kernelIdentifier);

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[]
        {
            /*
             * 
    private readonly ApplicationDedicatedFileName _ApplicationDedicatedFileName;
    private readonly ApplicationLabel? _ApplicationLabel;
    private readonly ApplicationPriorityIndicator _ApplicationPriorityIndicator;
    private readonly ExtendedSelection? _ExtendedSelection;
    private readonly KernelIdentifier? _KernelIdentifier;
             */ _ApplicationDedicatedFileName, _ApplicationLabel, _ApplicationPriorityIndicator, _ExtendedSelection, _KernelIdentifier
        };
    }

    #endregion
}