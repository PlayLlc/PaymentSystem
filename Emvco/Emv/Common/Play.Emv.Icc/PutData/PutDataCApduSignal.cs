using System.Collections.Immutable;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Core;
using Play.Emv.Ber;
using Play.Icc.Exceptions;
using Play.Icc.Messaging.Apdu;
using Play.Icc.Messaging.Apdu.PutData;

namespace Play.Emv.Icc;

/// <summary>
///     Stores primitive data that is not encapsulated in an Elementary File record
/// </summary>
public class PutDataCApduSignal : CApduSignal
{
    #region Constructor

    private PutDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1, parameter2)
    { }

    private PutDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) : base(@class, instruction, parameter1, parameter2,
        le)
    { }

    private PutDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class, instruction, parameter1,
        parameter2, data)
    { }

    private PutDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint? le) : base(@class, instruction,
        parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     The only valid <see cref="Tag" /> values that are valid for this method are UnprotectedDataEnvelope1 -
    ///     UnprotectedDataEnvelope5. In other words, Tags: 0x9F75, 0x9F76, 0x9F77, 0x9F78, 0x9F79
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="IccProtocolException"></exception>
    public static PutDataCApduSignal Create(PrimitiveValue value)
    {
        if (!DataObject.Exists(value.GetTag()))
        {
            throw new IccProtocolException(
                $"The {nameof(PutDataApduCommand)} could not be initialized because the {nameof(Tag)} value in the argument: [{value}] could not be recognized");
        }

        PutDataApduCommand? command = PutDataApduCommand.Create(ProprietaryMessageIdentifier._8x, (ushort) value.GetTag(),
            value.EncodeValue(EmvCodec.GetBerCodec()).AsSpan());

        return new PutDataCApduSignal(command.GetClass(), command.GetInstruction(), command.GetParameter1(), command.GetParameter2(), command.GetData());
    }

    #endregion

    /// <summary>
    ///     Supported values for the Get Data command
    /// </summary>
    public sealed record DataObject : EnumObject<Tag>
    {
        #region Static Metadata

        public static readonly DataObject Empty = new();
        private static readonly ImmutableSortedDictionary<Tag, DataObject> _ValueObjectMap;
        public static readonly DataObject UnprotectedDataEnvelope1 = new(0x9F75);
        public static readonly DataObject UnprotectedDataEnvelope2 = new(0x9F76);
        public static readonly DataObject UnprotectedDataEnvelope3 = new(0x9F77);
        public static readonly DataObject UnprotectedDataEnvelope4 = new(0x9F78);
        public static readonly DataObject UnprotectedDataEnvelope5 = new(0x9F79);

        #endregion

        #region Constructor

        public DataObject() : base()
        { }

        static DataObject()
        {
            _ValueObjectMap = new Dictionary<Tag, DataObject>()
            {
                {UnprotectedDataEnvelope1, UnprotectedDataEnvelope1},
                {UnprotectedDataEnvelope2, UnprotectedDataEnvelope2},
                {UnprotectedDataEnvelope3, UnprotectedDataEnvelope3},
                {UnprotectedDataEnvelope4, UnprotectedDataEnvelope4},
                {UnprotectedDataEnvelope5, UnprotectedDataEnvelope5}
            }.ToImmutableSortedDictionary();
        }

        private DataObject(Tag value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override DataObject[] GetAll() => _ValueObjectMap.Values.ToArray();

        public override bool TryGet(Tag value, out EnumObject<Tag>? result)
        {
            if (_ValueObjectMap.TryGetValue(value, out DataObject? enumResult))
            {
                result = enumResult;

                return true;
            }

            result = null;

            return false;
        }

        public static bool Exists(Tag value) => _ValueObjectMap.ContainsKey(value);
        public Tag GetTag() => _Value;

        #endregion
    }
}