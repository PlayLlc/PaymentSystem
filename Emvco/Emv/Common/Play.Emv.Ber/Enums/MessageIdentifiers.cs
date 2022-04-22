using System.Collections.Immutable;
using System.Numerics;

using Play.Core;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.Enums;

/// <summary>
///     Indicates the text string to be displayed, with the different standard messages defined in  Book A section 9.4. If
///     the Message EncodingId is not recognized, the reader should ignore it and the message currently displayed should
///     not be changed as a result of the User Interface Request.
/// </summary>
public record MessageIdentifiers : EnumObject<byte> { public override MessageIdentifiers[] GetAll() => _ValueObjectMap.Values.ToArray(); public override bool TryGet(byte value, out EnumObject<byte>? result) { if (_ValueObjectMap.TryGetValue(value, out MessageIdentifiers? enumResult)) { result = enumResult; return true; } result = null; return false; }
 public MessageIdentifiers() : base() { } public static readonly MessageIdentifiers Empty = new(); 
#region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, MessageIdentifiers> _ValueObjectMap;
    public static readonly MessageIdentifiers Amount;
    public static readonly MessageIdentifiers AmountOk;
    public static readonly MessageIdentifiers Approved;
    public static readonly MessageIdentifiers ApprovedPleaseSign;
    public static readonly MessageIdentifiers AuthorizingPleaseWait;
    public static readonly MessageIdentifiers CallYourBank;
    public static readonly MessageIdentifiers CancelOrEnter;
    public static readonly MessageIdentifiers CardError;
    public static readonly MessageIdentifiers CardReadOkRemoveCard;
    public static readonly MessageIdentifiers Declined;
    public static readonly MessageIdentifiers EnterAmount;
    public static readonly MessageIdentifiers EnterPin;
    public static readonly MessageIdentifiers IncorrectPin;
    public static readonly MessageIdentifiers InsertCard;
    public static readonly MessageIdentifiers ErrorUseAnotherCard;
    public static readonly MessageIdentifiers ClearDisplay;
    public static readonly MessageIdentifiers NotAccepted;
    public static readonly MessageIdentifiers NotAvailable;
    public static readonly MessageIdentifiers PinOk;
    public static readonly MessageIdentifiers PleaseInsertCard;
    public static readonly MessageIdentifiers PleaseInsertOrSwipeCard;
    public static readonly MessageIdentifiers PleasePresentOneCardOnly;
    public static readonly MessageIdentifiers PleaseWait;
    public static readonly MessageIdentifiers PresentCard;
    public static readonly MessageIdentifiers PresentCardAgain;
    public static readonly MessageIdentifiers Processing;
    public static readonly MessageIdentifiers ProcessingError;
    public static readonly MessageIdentifiers RemoveCard;
    public static readonly MessageIdentifiers SeePhoneForInstructions;
    public static readonly MessageIdentifiers TryAgain;
    public static readonly MessageIdentifiers UseChipReader;
    public static readonly MessageIdentifiers UseMagStripe;
    public static readonly MessageIdentifiers Welcome;

    #endregion

    #region Constructor

    static MessageIdentifiers()
    {
        #region consts

        const byte notAvailable = 0;
        const byte amount = 1;
        const byte amountOk = 2;
        const byte approved = 3;
        const byte callYourBank = 4;
        const byte cancelOrEnter = 5;
        const byte cardError = 6;
        const byte declined = 7;
        const byte enterAmount = 8;
        const byte enterPin = 9;
        const byte incorrectPin = 10;
        const byte insertCard = 11;
        const byte notAccepted = 12;
        const byte pinOk = 13;
        const byte pleaseWait = 14;
        const byte processingError = 15;
        const byte removeCard = 16;
        const byte useChipReader = 17;
        const byte useMagStripe = 18;
        const byte tryAgain = 19;

        #region Contactless Only

        const byte welcome = 20;
        const byte presentCard = 21;
        const byte processing = 22;
        const byte cardReadOkRemoveCard = 23;
        const byte pleaseInsertOrSwipeCard = 24;
        const byte pleasePresentOneCardOnly = 25;
        const byte approvedPleaseSign = 26;
        const byte authorizingPleaseWait = 27;
        const byte insertSwipeOrTryAnotherCard = 28;
        const byte pleaseInsertCard = 29;
        const byte clearDisplay = 30;
        const byte seePhoneForInstructions = 32;
        const byte presentCardAgain = 33;

        #endregion

        #endregion

        #region Initialization

        NotAvailable = new MessageIdentifiers(notAvailable);
        Amount = new MessageIdentifiers(amount);
        AmountOk = new MessageIdentifiers(amountOk);
        Approved = new MessageIdentifiers(approved);
        CallYourBank = new MessageIdentifiers(callYourBank);
        CancelOrEnter = new MessageIdentifiers(cancelOrEnter);
        CardError = new MessageIdentifiers(cardError);
        Declined = new MessageIdentifiers(declined);
        EnterAmount = new MessageIdentifiers(enterAmount);
        EnterPin = new MessageIdentifiers(enterPin);
        IncorrectPin = new MessageIdentifiers(incorrectPin);
        InsertCard = new MessageIdentifiers(insertCard);
        NotAccepted = new MessageIdentifiers(notAccepted);
        PinOk = new MessageIdentifiers(pinOk);
        PleaseWait = new MessageIdentifiers(pleaseWait);
        ProcessingError = new MessageIdentifiers(processingError);
        RemoveCard = new MessageIdentifiers(removeCard);
        UseChipReader = new MessageIdentifiers(useChipReader);
        UseMagStripe = new MessageIdentifiers(useMagStripe);
        TryAgain = new MessageIdentifiers(tryAgain);
        Welcome = new MessageIdentifiers(welcome);
        PresentCard = new MessageIdentifiers(presentCard);
        Processing = new MessageIdentifiers(processing);
        CardReadOkRemoveCard = new MessageIdentifiers(cardReadOkRemoveCard);
        PleaseInsertOrSwipeCard = new MessageIdentifiers(pleaseInsertOrSwipeCard);
        PleasePresentOneCardOnly = new MessageIdentifiers(pleasePresentOneCardOnly);
        ApprovedPleaseSign = new MessageIdentifiers(approvedPleaseSign);
        AuthorizingPleaseWait = new MessageIdentifiers(authorizingPleaseWait);
        ErrorUseAnotherCard = new MessageIdentifiers(insertSwipeOrTryAnotherCard);
        PleaseInsertCard = new MessageIdentifiers(pleaseInsertCard);
        ClearDisplay = new MessageIdentifiers(clearDisplay);
        SeePhoneForInstructions = new MessageIdentifiers(seePhoneForInstructions);
        PresentCardAgain = new MessageIdentifiers(presentCardAgain);

        #endregion

        #region Mapping

        _ValueObjectMap = new Dictionary<byte, MessageIdentifiers>
        {
            {amount, Amount},
            {amountOk, AmountOk},
            {approved, Approved},
            {callYourBank, CallYourBank},
            {cancelOrEnter, CancelOrEnter},
            {cardError, CardError},
            {declined, Declined},
            {enterAmount, EnterAmount},
            {enterPin, EnterPin},
            {incorrectPin, IncorrectPin},
            {insertCard, InsertCard},
            {notAccepted, NotAccepted},
            {pinOk, PinOk},
            {pleaseWait, PleaseWait},
            {processingError, ProcessingError},
            {removeCard, RemoveCard},
            {useChipReader, UseChipReader},
            {useMagStripe, UseMagStripe},
            {tryAgain, TryAgain},
            {welcome, Welcome},
            {presentCard, PresentCard},
            {processing, Processing},
            {cardReadOkRemoveCard, CardReadOkRemoveCard},
            {pleaseInsertOrSwipeCard, PleaseInsertOrSwipeCard},
            {pleasePresentOneCardOnly, PleasePresentOneCardOnly},
            {approvedPleaseSign, ApprovedPleaseSign},
            {authorizingPleaseWait, AuthorizingPleaseWait},
            {insertSwipeOrTryAnotherCard, ErrorUseAnotherCard},
            {pleaseInsertCard, PleaseInsertCard},
            {clearDisplay, ClearDisplay},
            {seePhoneForInstructions, SeePhoneForInstructions},
            {seePhoneForInstructions, SeePhoneForInstructions},
            {presentCardAgain, PresentCardAgain}
        }.ToImmutableSortedDictionary();

        #endregion
    }

    protected MessageIdentifiers(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static MessageIdentifiers[] GetAll() => _ValueObjectMap.Values.ToArray();
    public static MessageIdentifiers Get(byte value) => _ValueObjectMap.TryGetValue(value, out MessageIdentifiers? result) ? result : NotAvailable;

    public static MessageIdentifiers Get(MessageIdentifier value) =>
        _ValueObjectMap.TryGetValue((byte) value, out MessageIdentifiers? result) ? result : NotAvailable;

    #endregion

    #region Operator Overrides

    public static explicit operator BigInteger(MessageIdentifiers value) => value._Value;
    public static implicit operator MessageIdentifier(MessageIdentifiers value) => new(value._Value);

    #endregion
}