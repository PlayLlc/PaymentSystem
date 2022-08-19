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
public record DisplayMessageIdentifiers : EnumObject<byte>
{
    #region Static Metadata

    public static readonly DisplayMessageIdentifiers Empty = new();
    private static readonly ImmutableSortedDictionary<byte, DisplayMessageIdentifiers> _ValueObjectMap;
    public static readonly DisplayMessageIdentifiers Amount;
    public static readonly DisplayMessageIdentifiers AmountOk;
    public static readonly DisplayMessageIdentifiers Approved;
    public static readonly DisplayMessageIdentifiers ApprovedPleaseSign;
    public static readonly DisplayMessageIdentifiers AuthorizingPleaseWait;
    public static readonly DisplayMessageIdentifiers CallYourBank;
    public static readonly DisplayMessageIdentifiers CancelOrEnter;
    public static readonly DisplayMessageIdentifiers CardError;
    public static readonly DisplayMessageIdentifiers CardReadOkRemoveCard;
    public static readonly DisplayMessageIdentifiers Declined;
    public static readonly DisplayMessageIdentifiers EnterAmount;
    public static readonly DisplayMessageIdentifiers EnterPin;
    public static readonly DisplayMessageIdentifiers IncorrectPin;
    public static readonly DisplayMessageIdentifiers InsertCard;
    public static readonly DisplayMessageIdentifiers ErrorUseAnotherCard;
    public static readonly DisplayMessageIdentifiers ClearDisplay;
    public static readonly DisplayMessageIdentifiers NotAccepted;
    public static readonly DisplayMessageIdentifiers NotAvailable;
    public static readonly DisplayMessageIdentifiers PinOk;
    public static readonly DisplayMessageIdentifiers PleaseInsertCard;
    public static readonly DisplayMessageIdentifiers PleaseInsertOrSwipeCard;
    public static readonly DisplayMessageIdentifiers PleasePresentOneCardOnly;
    public static readonly DisplayMessageIdentifiers PleaseWait;
    public static readonly DisplayMessageIdentifiers PresentCard;
    public static readonly DisplayMessageIdentifiers PresentCardAgain;
    public static readonly DisplayMessageIdentifiers Processing;
    public static readonly DisplayMessageIdentifiers ProcessingError;
    public static readonly DisplayMessageIdentifiers RemoveCard;
    public static readonly DisplayMessageIdentifiers SeePhoneForInstructions;
    public static readonly DisplayMessageIdentifiers TryAgain;
    public static readonly DisplayMessageIdentifiers UseChipReader;
    public static readonly DisplayMessageIdentifiers UseMagStripe;
    public static readonly DisplayMessageIdentifiers Welcome;

    #endregion

    #region Constructor

    public DisplayMessageIdentifiers()
    { }

    static DisplayMessageIdentifiers()
    {
        #region Common

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

        #endregion

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

        #region Initialization

        NotAvailable = new DisplayMessageIdentifiers(notAvailable);
        Amount = new DisplayMessageIdentifiers(amount);
        AmountOk = new DisplayMessageIdentifiers(amountOk);
        Approved = new DisplayMessageIdentifiers(approved);
        CallYourBank = new DisplayMessageIdentifiers(callYourBank);
        CancelOrEnter = new DisplayMessageIdentifiers(cancelOrEnter);
        CardError = new DisplayMessageIdentifiers(cardError);
        Declined = new DisplayMessageIdentifiers(declined);
        EnterAmount = new DisplayMessageIdentifiers(enterAmount);
        EnterPin = new DisplayMessageIdentifiers(enterPin);
        IncorrectPin = new DisplayMessageIdentifiers(incorrectPin);
        InsertCard = new DisplayMessageIdentifiers(insertCard);
        NotAccepted = new DisplayMessageIdentifiers(notAccepted);
        PinOk = new DisplayMessageIdentifiers(pinOk);
        PleaseWait = new DisplayMessageIdentifiers(pleaseWait);
        ProcessingError = new DisplayMessageIdentifiers(processingError);
        RemoveCard = new DisplayMessageIdentifiers(removeCard);
        UseChipReader = new DisplayMessageIdentifiers(useChipReader);
        UseMagStripe = new DisplayMessageIdentifiers(useMagStripe);
        TryAgain = new DisplayMessageIdentifiers(tryAgain);
        Welcome = new DisplayMessageIdentifiers(welcome);
        PresentCard = new DisplayMessageIdentifiers(presentCard);
        Processing = new DisplayMessageIdentifiers(processing);
        CardReadOkRemoveCard = new DisplayMessageIdentifiers(cardReadOkRemoveCard);
        PleaseInsertOrSwipeCard = new DisplayMessageIdentifiers(pleaseInsertOrSwipeCard);
        PleasePresentOneCardOnly = new DisplayMessageIdentifiers(pleasePresentOneCardOnly);
        ApprovedPleaseSign = new DisplayMessageIdentifiers(approvedPleaseSign);
        AuthorizingPleaseWait = new DisplayMessageIdentifiers(authorizingPleaseWait);
        ErrorUseAnotherCard = new DisplayMessageIdentifiers(insertSwipeOrTryAnotherCard);
        PleaseInsertCard = new DisplayMessageIdentifiers(pleaseInsertCard);
        ClearDisplay = new DisplayMessageIdentifiers(clearDisplay);
        SeePhoneForInstructions = new DisplayMessageIdentifiers(seePhoneForInstructions);
        PresentCardAgain = new DisplayMessageIdentifiers(presentCardAgain);

        #endregion

        #region Mapping

        _ValueObjectMap = new Dictionary<byte, DisplayMessageIdentifiers>
        {
            {NotAvailable, NotAvailable},
            {Amount, Amount},
            {AmountOk, AmountOk},
            {Approved, Approved},
            {CallYourBank, CallYourBank},
            {CancelOrEnter, CancelOrEnter},
            {CardError, CardError},
            {Declined, Declined},
            {EnterAmount, EnterAmount},
            {EnterPin, EnterPin},
            {IncorrectPin, IncorrectPin},
            {InsertCard, InsertCard},
            {NotAccepted, NotAccepted},
            {PinOk, PinOk},
            {PleaseWait, PleaseWait},
            {ProcessingError, ProcessingError},
            {RemoveCard, RemoveCard},
            {UseChipReader, UseChipReader},
            {UseMagStripe, UseMagStripe},
            {TryAgain, TryAgain},
            {Welcome, Welcome},
            {PresentCard, PresentCard},
            {Processing, Processing},
            {CardReadOkRemoveCard, CardReadOkRemoveCard},
            {PleaseInsertOrSwipeCard, PleaseInsertOrSwipeCard},
            {PleasePresentOneCardOnly, PleasePresentOneCardOnly},
            {ApprovedPleaseSign, ApprovedPleaseSign},
            {AuthorizingPleaseWait, AuthorizingPleaseWait},
            {ErrorUseAnotherCard, ErrorUseAnotherCard},
            {PleaseInsertCard, PleaseInsertCard},
            {ClearDisplay, ClearDisplay},
            {SeePhoneForInstructions, SeePhoneForInstructions},
            {PresentCardAgain, PresentCardAgain}
        }.ToImmutableSortedDictionary();

        #endregion
    }

    protected DisplayMessageIdentifiers(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DisplayMessageIdentifiers[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out DisplayMessageIdentifiers? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static DisplayMessageIdentifiers Get(byte value) =>
        _ValueObjectMap.TryGetValue(value, out DisplayMessageIdentifiers? result) ? result : NotAvailable;

    public static DisplayMessageIdentifiers Get(DisplayMessageIdentifier value) =>
        _ValueObjectMap.TryGetValue((byte) value, out DisplayMessageIdentifiers? result) ? result : NotAvailable;

    #endregion

    #region Operator Overrides

    public static explicit operator BigInteger(DisplayMessageIdentifiers value) => value._Value;
    public static implicit operator DisplayMessageIdentifier(DisplayMessageIdentifiers value) => new(value._Value);

    #endregion
}