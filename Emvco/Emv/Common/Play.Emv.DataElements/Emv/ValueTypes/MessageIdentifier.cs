using System.Collections.Immutable;
using System.Numerics;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the text string to be displayed, with the different standard messages defined in  Book A
///     section 9.4.
///     If the Message Identifier is not recognized, the reader should ignore it and the message currently
///     displayed should not be changed as a result of the User Interface Request.
/// </summary>
public readonly struct MessageIdentifier
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, MessageIdentifier> _ValueObjectMap;
    public static readonly MessageIdentifier Amount;
    public static readonly MessageIdentifier AmountOk;
    public static readonly MessageIdentifier Approved;
    public static readonly MessageIdentifier ApprovedPleaseSign;
    public static readonly MessageIdentifier AuthorizingPleaseWait;
    public static readonly MessageIdentifier CallYourBank;
    public static readonly MessageIdentifier CancelOrEnter;
    public static readonly MessageIdentifier CardError;
    public static readonly MessageIdentifier CardReadOkRemoveCard;
    public static readonly MessageIdentifier Declined;
    public static readonly MessageIdentifier EnterAmount;
    public static readonly MessageIdentifier EnterPin;
    public static readonly MessageIdentifier IncorrectPin;
    public static readonly MessageIdentifier InsertCard;
    public static readonly MessageIdentifier InsertSwipeOrTryAnotherCard;
    public static readonly MessageIdentifier NoMessageDisplayed;
    public static readonly MessageIdentifier NotAccepted;
    public static readonly MessageIdentifier NotAvailable;
    public static readonly MessageIdentifier PinOk;
    public static readonly MessageIdentifier PleaseInsertCard;
    public static readonly MessageIdentifier PleaseInsertOrSwipeCard;
    public static readonly MessageIdentifier PleasePresentOneCardOnly;
    public static readonly MessageIdentifier PleaseWait;
    public static readonly MessageIdentifier PresentCard;
    public static readonly MessageIdentifier PresentCardAgain;
    public static readonly MessageIdentifier Processing;
    public static readonly MessageIdentifier ProcessingError;
    public static readonly MessageIdentifier RemoveCard;
    public static readonly MessageIdentifier SeePhoneForInstructions;
    public static readonly MessageIdentifier TryAgain;
    public static readonly MessageIdentifier UseChipReader;
    public static readonly MessageIdentifier UseMagStripe;
    public static readonly MessageIdentifier Welcome;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static MessageIdentifier()
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
        const byte noMessageDisplayed = 30;
        const byte seePhoneForInstructions = 32;
        const byte presentCardAgain = 33;

        #endregion

        #endregion

        #region Initialization

        NotAvailable = new MessageIdentifier(notAvailable);
        Amount = new MessageIdentifier(amount);
        AmountOk = new MessageIdentifier(amountOk);
        Approved = new MessageIdentifier(approved);
        CallYourBank = new MessageIdentifier(callYourBank);
        CancelOrEnter = new MessageIdentifier(cancelOrEnter);
        CardError = new MessageIdentifier(cardError);
        Declined = new MessageIdentifier(declined);
        EnterAmount = new MessageIdentifier(enterAmount);
        EnterPin = new MessageIdentifier(enterPin);
        IncorrectPin = new MessageIdentifier(incorrectPin);
        InsertCard = new MessageIdentifier(insertCard);
        NotAccepted = new MessageIdentifier(notAccepted);
        PinOk = new MessageIdentifier(pinOk);
        PleaseWait = new MessageIdentifier(pleaseWait);
        ProcessingError = new MessageIdentifier(processingError);
        RemoveCard = new MessageIdentifier(removeCard);
        UseChipReader = new MessageIdentifier(useChipReader);
        UseMagStripe = new MessageIdentifier(useMagStripe);
        TryAgain = new MessageIdentifier(tryAgain);
        Welcome = new MessageIdentifier(welcome);
        PresentCard = new MessageIdentifier(presentCard);
        Processing = new MessageIdentifier(processing);
        CardReadOkRemoveCard = new MessageIdentifier(cardReadOkRemoveCard);
        PleaseInsertOrSwipeCard = new MessageIdentifier(pleaseInsertOrSwipeCard);
        PleasePresentOneCardOnly = new MessageIdentifier(pleasePresentOneCardOnly);
        ApprovedPleaseSign = new MessageIdentifier(approvedPleaseSign);
        AuthorizingPleaseWait = new MessageIdentifier(authorizingPleaseWait);
        InsertSwipeOrTryAnotherCard = new MessageIdentifier(insertSwipeOrTryAnotherCard);
        PleaseInsertCard = new MessageIdentifier(pleaseInsertCard);
        NoMessageDisplayed = new MessageIdentifier(noMessageDisplayed);
        SeePhoneForInstructions = new MessageIdentifier(seePhoneForInstructions);
        PresentCardAgain = new MessageIdentifier(presentCardAgain);

        #endregion

        #region Mapping

        _ValueObjectMap = new Dictionary<byte, MessageIdentifier>
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
            {insertSwipeOrTryAnotherCard, InsertSwipeOrTryAnotherCard},
            {pleaseInsertCard, PleaseInsertCard},
            {noMessageDisplayed, NoMessageDisplayed},
            {seePhoneForInstructions, SeePhoneForInstructions},
            {seePhoneForInstructions, SeePhoneForInstructions},
            {presentCardAgain, PresentCardAgain}
        }.ToImmutableSortedDictionary();

        #endregion
    }

    private MessageIdentifier(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static MessageIdentifier Get(byte value) =>
        _ValueObjectMap.TryGetValue(value, out MessageIdentifier result) ? result : NotAvailable;

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is MessageIdentifier MessageIdentifier && Equals(MessageIdentifier);
    public bool Equals(MessageIdentifier other) => _Value == other._Value;
    public bool Equals(MessageIdentifier x, MessageIdentifier y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 297581;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(MessageIdentifier left, MessageIdentifier right) => left._Value == right._Value;
    public static bool operator ==(MessageIdentifier left, byte right) => left._Value == right;
    public static bool operator ==(byte left, MessageIdentifier right) => left == right._Value;
    public static explicit operator short(MessageIdentifier value) => value._Value;
    public static explicit operator ushort(MessageIdentifier value) => value._Value;
    public static explicit operator int(MessageIdentifier value) => value._Value;
    public static explicit operator uint(MessageIdentifier value) => value._Value;
    public static explicit operator long(MessageIdentifier value) => value._Value;
    public static explicit operator ulong(MessageIdentifier value) => value._Value;
    public static explicit operator BigInteger(MessageIdentifier value) => value._Value;
    public static implicit operator byte(MessageIdentifier value) => value._Value;
    public static bool operator !=(MessageIdentifier left, MessageIdentifier right) => !(left == right);
    public static bool operator !=(MessageIdentifier left, byte right) => !(left == right);
    public static bool operator !=(byte left, MessageIdentifier right) => !(left == right);

    #endregion
}