using System.Collections.Immutable;

namespace Play.Emv.Icc;

public readonly struct Level2Error
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Level2Error> _ValueObjectMap;
    public static readonly Level2Error CamFailed;
    public static readonly Level2Error CardDataError;
    public static readonly Level2Error CardDataMissing;
    public static readonly Level2Error EmptyCandidateList;
    public static readonly Level2Error IdsDataError;
    public static readonly Level2Error IdsNoMatchingAc;
    public static readonly Level2Error IdsReaderError;
    public static readonly Level2Error IdsWriterError;
    public static readonly Level2Error MagStripeNotSupported;
    public static readonly Level2Error MaxLimitExceeded;
    public static readonly Level2Error NoPpse;
    public static readonly Level2Error Ok;
    public static readonly Level2Error ParsingError;
    public static readonly Level2Error PpseFault;
    public static readonly Level2Error StatusBytes;
    public static readonly Level2Error TerminalDataError;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static Level2Error()
    {
        const byte camFailed = 2;
        const byte cardDataError = 6;
        const byte cardDataMissing = 1;
        const byte emptyCandidateList = 10;
        const byte idsDataError = 13;
        const byte idsNoMatchingAc = 14;
        const byte idsReaderError = 11;
        const byte idsWriterError = 12;
        const byte magStripeNotSupported = 7;
        const byte maxLimitExceeded = 5;
        const byte noPpse = 8;
        const byte ok = 0;
        const byte parsingError = 4;
        const byte ppseFault = 9;
        const byte statusBytes = 3;
        const byte terminalDataError = 15;

        CamFailed = new Level2Error(camFailed);
        CardDataError = new Level2Error(cardDataError);
        CardDataMissing = new Level2Error(cardDataMissing);
        EmptyCandidateList = new Level2Error(emptyCandidateList);
        IdsDataError = new Level2Error(idsDataError);
        IdsNoMatchingAc = new Level2Error(idsNoMatchingAc);
        IdsReaderError = new Level2Error(idsReaderError);
        IdsWriterError = new Level2Error(idsWriterError);
        MagStripeNotSupported = new Level2Error(magStripeNotSupported);
        MaxLimitExceeded = new Level2Error(maxLimitExceeded);
        NoPpse = new Level2Error(noPpse);
        Ok = new Level2Error(ok);
        ParsingError = new Level2Error(parsingError);
        PpseFault = new Level2Error(ppseFault);
        StatusBytes = new Level2Error(statusBytes);
        TerminalDataError = new Level2Error(terminalDataError);
        _ValueObjectMap = new Dictionary<byte, Level2Error>
        {
            {camFailed, CamFailed},
            {cardDataError, CardDataError},
            {cardDataMissing, CardDataMissing},
            {emptyCandidateList, EmptyCandidateList},
            {idsDataError, IdsDataError},
            {idsNoMatchingAc, IdsNoMatchingAc},
            {idsReaderError, IdsReaderError},
            {idsWriterError, IdsWriterError},
            {magStripeNotSupported, MagStripeNotSupported},
            {maxLimitExceeded, MaxLimitExceeded},
            {noPpse, NoPpse},
            {ok, Ok},
            {parsingError, ParsingError},
            {ppseFault, PpseFault},
            {statusBytes, StatusBytes},
            {terminalDataError, TerminalDataError}
        }.ToImmutableSortedDictionary();
    }

    private Level2Error(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static Level2Error Get(byte value) => _ValueObjectMap[value];

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is Level2Error l2 && Equals(l2);
    public bool Equals(Level2Error other) => _Value == other._Value;
    public bool Equals(Level2Error x, Level2Error y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 619841;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Level2Error left, Level2Error right) => left._Value == right._Value;
    public static bool operator ==(Level2Error left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Level2Error right) => left == right._Value;
    public static explicit operator byte(Level2Error value) => value._Value;
    public static explicit operator short(Level2Error value) => value._Value;
    public static explicit operator ushort(Level2Error value) => value._Value;
    public static explicit operator int(Level2Error value) => value._Value;
    public static explicit operator uint(Level2Error value) => value._Value;
    public static explicit operator long(Level2Error value) => value._Value;
    public static explicit operator ulong(Level2Error value) => value._Value;
    public static bool operator !=(Level2Error left, Level2Error right) => !(left == right);
    public static bool operator !=(Level2Error left, byte right) => !(left == right);
    public static bool operator !=(byte left, Level2Error right) => !(left == right);

    #endregion
}