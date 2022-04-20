using System.Collections.Immutable;

using Play.Core;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber.Enums;

/// <summary>
///     An enumeration of status values that can be set for <see cref="TransactionStatusInformation" />
/// </summary>
public sealed record TransactionStatusInformationFlags : EnumObject<ushort>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<ushort, TransactionStatusInformationFlags> _ValueObjectMap;
    public static readonly TransactionStatusInformationFlags CardholderVerificationPerformed;
    public static readonly TransactionStatusInformationFlags CardRiskManagementPerformed;
    public static readonly TransactionStatusInformationFlags IssuerAuthenticationPerformed;
    public static readonly TransactionStatusInformationFlags NotAvailable;
    public static readonly TransactionStatusInformationFlags OfflineDataAuthenticationPerformed;
    public static readonly TransactionStatusInformationFlags ScriptProcessingPerformed;
    public static readonly TransactionStatusInformationFlags TerminalRiskManagementPerformed;

    #endregion

    #region Constructor

    static TransactionStatusInformationFlags()
    {
        const ushort notAvailable = 0;
        const ushort offlineDataAuthenticationPerformed = 0x0800;
        const ushort cardholderVerificationPerformed = 0x0700;
        const ushort cardRiskManagementPerformed = 0x0600;
        const ushort issuerAuthenticationPerformed = 0x0500;
        const ushort terminalRiskManagementPerformed = 0x0400;
        const ushort scriptProcessingPerformed = 0x0300;

        NotAvailable = new TransactionStatusInformationFlags(notAvailable);
        OfflineDataAuthenticationPerformed = new TransactionStatusInformationFlags(offlineDataAuthenticationPerformed);
        CardholderVerificationPerformed = new TransactionStatusInformationFlags(cardholderVerificationPerformed);
        CardRiskManagementPerformed = new TransactionStatusInformationFlags(cardRiskManagementPerformed);
        IssuerAuthenticationPerformed = new TransactionStatusInformationFlags(issuerAuthenticationPerformed);
        TerminalRiskManagementPerformed = new TransactionStatusInformationFlags(terminalRiskManagementPerformed);
        ScriptProcessingPerformed = new TransactionStatusInformationFlags(scriptProcessingPerformed);
        _ValueObjectMap = new Dictionary<ushort, TransactionStatusInformationFlags>
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

    private TransactionStatusInformationFlags(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static TransactionStatusInformationFlags[] GetAll() => _ValueObjectMap.Values.ToArray();
    public static bool TryGet(ushort value, out TransactionStatusInformationFlags? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(TransactionStatusInformationFlags? other) => !(other is null) && (_Value == other._Value);

    public bool Equals(TransactionStatusInformationFlags x, TransactionStatusInformationFlags y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode() => 470621 * _Value.GetHashCode();
    public int GetHashCode(TransactionStatusInformationFlags obj) => obj.GetHashCode();
    public int CompareTo(TransactionStatusInformationFlags other) => _Value.CompareTo(other._Value);

    #endregion

    #region Operator Overrides

    public static explicit operator ushort(TransactionStatusInformationFlags registeredApplicationProviderIndicators) =>
        registeredApplicationProviderIndicators._Value;

    #endregion
}