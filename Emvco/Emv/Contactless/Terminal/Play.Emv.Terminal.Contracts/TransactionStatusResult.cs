using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Terminal.Contracts;

/// <summary>
///     An enumeration of status values that can be set for <see cref="TransactionStatusInformation" />
/// </summary>
public sealed record TransactionStatusResult : EnumObject<ushort>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<ushort, TransactionStatusResult> _ValueObjectMap;
    public static readonly TransactionStatusResult CardholderVerificationPerformed;
    public static readonly TransactionStatusResult CardRiskManagementPerformed;
    public static readonly TransactionStatusResult IssuerAuthenticationPerformed;
    public static readonly TransactionStatusResult NotAvailable;
    public static readonly TransactionStatusResult OfflineDataAuthenticationPerformed;
    public static readonly TransactionStatusResult ScriptProcessingPerformed;
    public static readonly TransactionStatusResult TerminalRiskManagementPerformed;

    #endregion

    #region Constructor

    static TransactionStatusResult()
    {
        const ushort notAvailable = 0;
        const ushort offlineDataAuthenticationPerformed = 0x0800;
        const ushort cardholderVerificationPerformed = 0x0700;
        const ushort cardRiskManagementPerformed = 0x0600;
        const ushort issuerAuthenticationPerformed = 0x0500;
        const ushort terminalRiskManagementPerformed = 0x0400;
        const ushort scriptProcessingPerformed = 0x0300;

        NotAvailable = new TransactionStatusResult(notAvailable);
        OfflineDataAuthenticationPerformed = new TransactionStatusResult(offlineDataAuthenticationPerformed);
        CardholderVerificationPerformed = new TransactionStatusResult(cardholderVerificationPerformed);
        CardRiskManagementPerformed = new TransactionStatusResult(cardRiskManagementPerformed);
        IssuerAuthenticationPerformed = new TransactionStatusResult(issuerAuthenticationPerformed);
        TerminalRiskManagementPerformed = new TransactionStatusResult(terminalRiskManagementPerformed);
        ScriptProcessingPerformed = new TransactionStatusResult(scriptProcessingPerformed);
        _ValueObjectMap = new Dictionary<ushort, TransactionStatusResult>
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

    private TransactionStatusResult(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int CompareTo(TransactionStatusResult other) => _Value.CompareTo(other._Value);
    public static bool TryGet(ushort value, out TransactionStatusResult? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(TransactionStatusResult? other) => !(other is null) && (_Value == other._Value);

    public bool Equals(TransactionStatusResult x, TransactionStatusResult y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode() => 470621 * _Value.GetHashCode();
    public int GetHashCode(TransactionStatusResult obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator ushort(TransactionStatusResult registeredApplicationProviderIndicators) =>
        registeredApplicationProviderIndicators._Value;

    #endregion
}