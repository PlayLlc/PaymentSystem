using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Terminal.Contracts;

/// <summary>
///     An enumeration of status values that can be set for <see cref="TransactionStatusInformation" />
/// </summary>
public sealed record TransactionStatusInformationFlagTypes : EnumObject<ushort>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<ushort, TransactionStatusInformationFlagTypes> _ValueObjectMap;
    public static readonly TransactionStatusInformationFlagTypes CardholderVerificationPerformed;
    public static readonly TransactionStatusInformationFlagTypes CardRiskManagementPerformed;
    public static readonly TransactionStatusInformationFlagTypes IssuerAuthenticationPerformed;
    public static readonly TransactionStatusInformationFlagTypes NotAvailable;
    public static readonly TransactionStatusInformationFlagTypes OfflineDataAuthenticationPerformed;
    public static readonly TransactionStatusInformationFlagTypes ScriptProcessingPerformed;
    public static readonly TransactionStatusInformationFlagTypes TerminalRiskManagementPerformed;

    #endregion

    #region Constructor

    static TransactionStatusInformationFlagTypes()
    {
        const ushort notAvailable = 0;
        const ushort offlineDataAuthenticationPerformed = 0x0800;
        const ushort cardholderVerificationPerformed = 0x0700;
        const ushort cardRiskManagementPerformed = 0x0600;
        const ushort issuerAuthenticationPerformed = 0x0500;
        const ushort terminalRiskManagementPerformed = 0x0400;
        const ushort scriptProcessingPerformed = 0x0300;

        NotAvailable = new TransactionStatusInformationFlagTypes(notAvailable);
        OfflineDataAuthenticationPerformed = new TransactionStatusInformationFlagTypes(offlineDataAuthenticationPerformed);
        CardholderVerificationPerformed = new TransactionStatusInformationFlagTypes(cardholderVerificationPerformed);
        CardRiskManagementPerformed = new TransactionStatusInformationFlagTypes(cardRiskManagementPerformed);
        IssuerAuthenticationPerformed = new TransactionStatusInformationFlagTypes(issuerAuthenticationPerformed);
        TerminalRiskManagementPerformed = new TransactionStatusInformationFlagTypes(terminalRiskManagementPerformed);
        ScriptProcessingPerformed = new TransactionStatusInformationFlagTypes(scriptProcessingPerformed);
        _ValueObjectMap = new Dictionary<ushort, TransactionStatusInformationFlagTypes>
        {
            {notAvailable, NotAvailable},
            {offlineDataAuthenticationPerformed, OfflineDataAuthenticationPerformed},
            {cardholderVerificationPerformed, CardholderVerificationPerformed},
            {cardRiskManagementPerformed, CardRiskManagementPerformed},
            {issuerAuthenticationPerformed, IssuerAuthenticationPerformed},
            {terminalRiskManagementPerformed, TerminalRiskManagementPerformed},
            {scriptProcessingPerformed, ScriptProcessingPerformed}
        }.ToImmutableSortedDictionary();
    }

    private TransactionStatusInformationFlagTypes(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int CompareTo(TransactionStatusInformationFlagTypes other) => _Value.CompareTo(other._Value);

    public static bool TryGet(ushort value, out TransactionStatusInformationFlagTypes? result) =>
        _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(TransactionStatusInformationFlagTypes? other) => !(other is null) && (_Value == other._Value);

    public bool Equals(TransactionStatusInformationFlagTypes x, TransactionStatusInformationFlagTypes y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode() => 470621 * _Value.GetHashCode();
    public int GetHashCode(TransactionStatusInformationFlagTypes obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator ushort(TransactionStatusInformationFlagTypes registeredApplicationProviderIndicators) =>
        registeredApplicationProviderIndicators._Value;

    #endregion
}