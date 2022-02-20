using System.Collections.Immutable;

using Play.Core;
using Play.Emv.Acquirer.Contracts;

namespace Play.Acquirer.Contracts;

/*
 permutations:

    Single Message Network
    {
        Online
        {
            Sale - 1200
        }

        Offline
        {
            Completion - 1220
        }
    }

    Dual Message Network
    {
        Online
        {
            Authorization - 1100
            Completion 1200
        }

        Offline
        {
            Authorization - 1100
            OfflineCompletion - 1240
        } 
    }

 */
public sealed record MessageTypeIndicatorTypes : EnumObject<MessageTypeIndicator>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<MessageTypeIndicator, MessageTypeIndicatorTypes> _ValueObjectMap;
    private static readonly MessageTypeIndicatorTypes _AuthorizationRequest;
    private static readonly MessageTypeIndicatorTypes _AuthorizationResponse;
    private static readonly MessageTypeIndicatorTypes _AuthorizationAdviceRequest;
    private static readonly MessageTypeIndicatorTypes _AuthorizationAdviceResponse;
    private static readonly MessageTypeIndicatorTypes _FinancialRequest;
    private static readonly MessageTypeIndicatorTypes _FinancialResponse;
    private static readonly MessageTypeIndicatorTypes _FinancialAdviceRequest;
    private static readonly MessageTypeIndicatorTypes _FinancialAdviceResponse;
    private static readonly MessageTypeIndicatorTypes _ReversalRequest;
    private static readonly MessageTypeIndicatorTypes _ReversalResponse;
    private static readonly MessageTypeIndicatorTypes _ReversalAdviceRequest;
    private static readonly MessageTypeIndicatorTypes _ReversalAdviceResponse;
    private static readonly MessageTypeIndicatorTypes _ReconciliationRequest;
    private static readonly MessageTypeIndicatorTypes _ReconciliationResponse;
    private static readonly MessageTypeIndicatorTypes _ReconciliationAdviceRequest;
    private static readonly MessageTypeIndicatorTypes _ReconciliationAdviceResponse;
    private static readonly MessageTypeIndicatorTypes _ManagementRequest;
    private static readonly MessageTypeIndicatorTypes _ManagementResponse;
    private static readonly MessageTypeIndicatorTypes _ManagementAdviceRequest;
    private static readonly MessageTypeIndicatorTypes _ManagementAdviceResponse;

    #endregion

    #region Constructor

    static MessageTypeIndicatorTypes()
    {
        _AuthorizationRequest = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1100));
        _AuthorizationResponse = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1110));
        _AuthorizationAdviceRequest = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1120));
        _AuthorizationAdviceResponse = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1130));

        _FinancialRequest = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1200));
        _FinancialResponse = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1210));
        _FinancialAdviceRequest = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1220));
        _FinancialAdviceResponse = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1230));

        _ReversalRequest = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1400));
        _ReversalResponse = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1410));
        _ReversalAdviceRequest = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1420));
        _ReversalAdviceResponse = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1430));

        _ReconciliationRequest = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1400));
        _ReconciliationResponse = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1410));
        _ReconciliationAdviceRequest = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1420));
        _ReconciliationAdviceResponse = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1430));

        _ManagementRequest = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1800));
        _ManagementResponse = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1810));
        _ManagementAdviceRequest = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1820));
        _ManagementAdviceResponse = new MessageTypeIndicatorTypes(new MessageTypeIndicator(1830));

        _ValueObjectMap = new Dictionary<MessageTypeIndicator, MessageTypeIndicatorTypes>
        {
            {_AuthorizationRequest, _AuthorizationRequest},
            {_AuthorizationResponse, _AuthorizationResponse},
            {_AuthorizationAdviceRequest, _AuthorizationAdviceRequest},
            {_AuthorizationAdviceResponse, _AuthorizationAdviceResponse},
            {_FinancialRequest, _FinancialRequest},
            {_FinancialResponse, _FinancialResponse},
            {_FinancialAdviceRequest, _FinancialAdviceRequest},
            {_FinancialAdviceResponse, _FinancialAdviceResponse},
            {_ReversalRequest, _ReversalRequest},
            {_ReversalResponse, _ReversalResponse},
            {_ReversalAdviceRequest, _ReversalAdviceRequest},
            {_ReversalAdviceResponse, _ReversalAdviceResponse},
            {_ReconciliationRequest, _ReconciliationRequest},
            {_ReconciliationResponse, _ReconciliationResponse},
            {_ReconciliationAdviceRequest, _ReconciliationAdviceRequest},
            {_ReconciliationAdviceResponse, _ReconciliationAdviceResponse},
            {_ManagementRequest, _ManagementRequest},
            {_ManagementResponse, _ManagementResponse},
            {_ManagementAdviceRequest, _ManagementAdviceRequest},
            {_ManagementAdviceResponse, _ManagementAdviceResponse}
        }.ToImmutableSortedDictionary();
    }

    private MessageTypeIndicatorTypes(MessageTypeIndicator value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int CompareTo(MessageTypeIndicatorTypes other) => ((ushort) _Value).CompareTo(other._Value);

    #endregion

    public class Authorization
    { }

    //Purchase
    //• Purchase with cashback
    //• Cash Advance(cash withdrawal)
    //    • Refund

    // One Way
    public class Financial
    { }

    public class Reversal
    { }

    public class Reconciliation
    {
        #region Static Metadata

        public static readonly MessageTypeIndicatorTypes ReconciliationRequest = _ReconciliationRequest;
        public static readonly MessageTypeIndicatorTypes ReconciliationResponse = _ReconciliationResponse;
        public static readonly MessageTypeIndicatorTypes ReconciliationAdviceRequest = _ReconciliationAdviceRequest;
        public static readonly MessageTypeIndicatorTypes ReconciliationAdviceResponse = _ReconciliationAdviceResponse;

        #endregion
    }

    public class Management
    { }
}