using System.Collections.Generic;
using System.Collections.Immutable;

namespace Play.Icc.Messaging.Apdu;

internal readonly struct Instruction
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Instruction> _ValueObjectMap;

    /// <value>Hexadecimal: 0xE2</value>
    public static readonly Instruction AppendRecord;

    /// <value>Hexadecimal: 0xC2</value>
    public static readonly Instruction Envelope;

    /// <value>Hexadecimal: 0x0E</value>
    public static readonly Instruction EraseBinary;

    /// <value>Hexadecimal: 0x82</value>
    public static readonly Instruction ExternalAuthenticate;

    /// <value>Hexadecimal: 0x84</value>
    public static readonly Instruction GetChallenge;

    /// <value>Hexadecimal: 0xCA</value>
    public static readonly Instruction GetData;

    /// <value>Hexadecimal: 0xC0</value>
    public static readonly Instruction GetResponse;

    /// <value>Hexadecimal: 0x88</value>
    public static readonly Instruction InternalAuthenticate;

    /// <value>Hexadecimal: 0x70</value>
    public static readonly Instruction ManageChannel;

    /// <value>Hexadecimal: 0xDA</value>
    public static readonly Instruction PutData;

    /// <value>Hexadecimal: 0xB0</value>
    public static readonly Instruction ReadBinary;

    /// <value>Hexadecimal: 0xB2</value>
    public static readonly Instruction ReadRecord;

    /// <value>Hexadecimal: 0xA4</value>
    public static readonly Instruction SelectFile;

    /// <value>Hexadecimal: 0xD6</value>
    public static readonly Instruction UpdateBinary;

    /// <value>Hexadecimal: 0xDC</value>
    public static readonly Instruction UpdateData;

    /// <value>Hexadecimal: 0x20</value>
    public static readonly Instruction Verify;

    /// <value>Hexadecimal: 0xD0</value>
    public static readonly Instruction WriteBinary;

    /// <value>Hexadecimal: 0xD2</value>
    public static readonly Instruction WriteRecord;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static Instruction()
    {
        const byte eraseBinary = 0x0E;
        const byte verify = 0x20;
        const byte manageChannel = 0x70;
        const byte externalAuthenticate = 0x82;
        const byte getChallenge = 0x84;
        const byte internalAuthenticate = 0x88;
        const byte selectFile = 0xA4;
        const byte readBinary = 0xB0;
        const byte readRecord = 0xB2;
        const byte getResponse = 0xC0;
        const byte envelope = 0xC2;
        const byte getData = 0xCA;
        const byte writeBinary = 0xD0;
        const byte writeRecord = 0xD2;
        const byte updateBinary = 0xD6;
        const byte putData = 0xDA;
        const byte updateData = 0xDC;
        const byte appendRecord = 0xE2;

        EraseBinary = new Instruction(eraseBinary);
        Verify = new Instruction(verify);
        ManageChannel = new Instruction(manageChannel);
        ExternalAuthenticate = new Instruction(externalAuthenticate);
        GetChallenge = new Instruction(getChallenge);
        InternalAuthenticate = new Instruction(internalAuthenticate);
        SelectFile = new Instruction(selectFile);
        ReadBinary = new Instruction(readBinary);
        ReadRecord = new Instruction(readRecord);
        GetResponse = new Instruction(getResponse);
        Envelope = new Instruction(envelope);
        GetData = new Instruction(getData);
        WriteBinary = new Instruction(writeBinary);
        WriteRecord = new Instruction(writeRecord);
        UpdateBinary = new Instruction(updateBinary);
        PutData = new Instruction(putData);
        UpdateData = new Instruction(updateData);
        AppendRecord = new Instruction(appendRecord);

        _ValueObjectMap = new Dictionary<byte, Instruction>
        {
            {eraseBinary, EraseBinary},
            {verify, Verify},
            {manageChannel, ManageChannel},
            {externalAuthenticate, ExternalAuthenticate},
            {getChallenge, GetChallenge},
            {internalAuthenticate, InternalAuthenticate},
            {selectFile, SelectFile},
            {readBinary, ReadBinary},
            {readRecord, ReadRecord},
            {getResponse, GetResponse},
            {envelope, Envelope},
            {getData, GetData},
            {writeBinary, WriteBinary},
            {writeRecord, WriteRecord},
            {updateBinary, UpdateBinary},
            {putData, PutData},
            {updateData, UpdateData},
            {appendRecord, AppendRecord}
        }.ToImmutableSortedDictionary();
    }

    private Instruction(byte value)
    {
        //if (value > 3)
        //    throw new ArgumentOutOfRangeException(nameof(value));

        _Value = value;
    }

    #endregion

    #region Instance Members

    public static Instruction Get(byte value) => _ValueObjectMap[value];

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is Instruction logicalChannel && Equals(logicalChannel);
    public bool Equals(Instruction other) => _Value == other._Value;
    public bool Equals(Instruction x, Instruction y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 658379;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Instruction left, Instruction right) => left._Value == right._Value;
    public static bool operator ==(Instruction left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Instruction right) => left == right._Value;

    // logical channel values are from 0 to 3 so casting to sbyte will not truncate any meaningful information
    public static explicit operator sbyte(Instruction value) => (sbyte) value._Value;
    public static explicit operator short(Instruction value) => value._Value;
    public static explicit operator ushort(Instruction value) => value._Value;
    public static explicit operator int(Instruction value) => value._Value;
    public static explicit operator uint(Instruction value) => value._Value;
    public static explicit operator long(Instruction value) => value._Value;
    public static explicit operator ulong(Instruction value) => value._Value;
    public static implicit operator byte(Instruction value) => value._Value;
    public static bool operator !=(Instruction left, Instruction right) => !(left == right);
    public static bool operator !=(Instruction left, byte right) => !(left == right);
    public static bool operator !=(byte left, Instruction right) => !(left == right);

    #endregion
}