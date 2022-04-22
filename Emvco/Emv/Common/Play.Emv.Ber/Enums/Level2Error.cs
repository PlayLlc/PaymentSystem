using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.Enums;

public record Level2Error : EnumObject<byte> { public override Level2Error[] GetAll() => _ValueObjectMap.Values.ToArray(); public override bool TryGet(byte value, out EnumObject<byte>? result) { if (_ValueObjectMap.TryGetValue(value, out Level2Error? enumResult)) { result = enumResult; return true; } result = null; return false; }
 public Level2Error() : base() { } public static readonly Level2Error Empty = new(); 
#region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Level2Error> _ValueObjectMap;
    public static readonly Level2Error CryptographicAuthenticationMethodFailed;
    public static readonly Level2Error CardDataError;
    public static readonly Level2Error CardDataMissing;
    public static readonly Level2Error EmptyCandidateList;
    public static readonly Level2Error IdsDataError;
    public static readonly Level2Error IdsNoMatchingAc;
    public static readonly Level2Error IdsReadError;
    public static readonly Level2Error IdsWriterError;
    public static readonly Level2Error MagstripeNotSupported;
    public static readonly Level2Error MaxLimitExceeded;
    public static readonly Level2Error NoPpse;
    public static readonly Level2Error Ok;
    public static readonly Level2Error ParsingError;
    public static readonly Level2Error PpseFault;
    public static readonly Level2Error StatusBytes;
    public static readonly Level2Error TerminalDataError;

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

        CryptographicAuthenticationMethodFailed = new Level2Error(camFailed);
        CardDataError = new Level2Error(cardDataError);
        CardDataMissing = new Level2Error(cardDataMissing);
        EmptyCandidateList = new Level2Error(emptyCandidateList);
        IdsDataError = new Level2Error(idsDataError);
        IdsNoMatchingAc = new Level2Error(idsNoMatchingAc);
        IdsReadError = new Level2Error(idsReaderError);
        IdsWriterError = new Level2Error(idsWriterError);
        MagstripeNotSupported = new Level2Error(magStripeNotSupported);
        MaxLimitExceeded = new Level2Error(maxLimitExceeded);
        NoPpse = new Level2Error(noPpse);
        Ok = new Level2Error(ok);
        ParsingError = new Level2Error(parsingError);
        PpseFault = new Level2Error(ppseFault);
        StatusBytes = new Level2Error(statusBytes);
        TerminalDataError = new Level2Error(terminalDataError);
        _ValueObjectMap = new Dictionary<byte, Level2Error>
        {
            {camFailed, CryptographicAuthenticationMethodFailed},
            {cardDataError, CardDataError},
            {cardDataMissing, CardDataMissing},
            {emptyCandidateList, EmptyCandidateList},
            {idsDataError, IdsDataError},
            {idsNoMatchingAc, IdsNoMatchingAc},
            {idsReaderError, IdsReadError},
            {idsWriterError, IdsWriterError},
            {magStripeNotSupported, MagstripeNotSupported},
            {maxLimitExceeded, MaxLimitExceeded},
            {noPpse, NoPpse},
            {ok, Ok},
            {parsingError, ParsingError},
            {ppseFault, PpseFault},
            {statusBytes, StatusBytes},
            {terminalDataError, TerminalDataError}
        }.ToImmutableSortedDictionary();
    }

    private Level2Error(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static Level2Error[] GetAll() => _ValueObjectMap.Values.ToArray();
    public static Level2Error Get(byte value) => _ValueObjectMap[value];

    #endregion
}