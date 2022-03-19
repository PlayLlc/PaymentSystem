using System;

using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;

namespace Play.Emv.Identifiers;

/// <summary>
///     A value that can be used to correlate an Application Session for Data Exchange Kernel messages
/// </summary>
/// <example>
///     Example value:
///     0xFF FF FF FF FF FF FFFF
///     |  |  |  |  |  |  |
///     |  |  |  |  |  |  |
///     |  |  |  |  |  |  |
///     |  MM DD HH MM SS Milliseconds
///     Short Kernel ID
/// </example>
public readonly struct KernelSessionId
{
    #region Instance Values

    private readonly TransactionSessionId _TransactionSessionId;
    private readonly ulong _Value;

    #endregion

    #region Constructor

    public KernelSessionId(KernelId kernelId, TransactionSessionId transactionSessionId)
    {
        _Value = GetConstructorValueKernelId(kernelId) | GetUniqueTicks();

        _TransactionSessionId = transactionSessionId;
    }

    #endregion

    #region Instance Members

    public ShortKernelIdTypes GetKernelId() => ShortKernelIdTypes.Get((byte) (_Value >> (7 * 8)));
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;
    public TransactionType GetTransactionType() => _TransactionSessionId.GetTransactionType();
    private static ulong GetConstructorValueKernelId(KernelId kernelId) => (ulong) kernelId << (7 * 8);

    private static ulong GetUniqueTicks()
    {
        const ulong bitMaskValue = 0xFF00000000000000;
        SessionId sessionId = new(DateTime.UtcNow);

        return ((ulong) sessionId).GetMaskedValue(bitMaskValue);
    }

    #endregion

    #region Equality

    public override bool Equals(object? other) => other is KernelSessionId dekCorrelationId && Equals(dekCorrelationId);
    public bool Equals(KernelSessionId other) => _Value == other._Value;
    public override int GetHashCode() => unchecked(1861 * (int) _Value);

    #endregion

    #region Operator Overrides

    public static bool operator ==(KernelSessionId left, KernelSessionId right) => left.Equals(right);
    public static explicit operator ulong(KernelSessionId value) => value._Value;
    public static bool operator !=(KernelSessionId left, KernelSessionId right) => !left.Equals(right);

    #endregion
}