using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Metadata;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.DataElements.Emv;

// TODO: there needs to be some service that reads all the files referenced by AFL. If the following are not
// TODO: present after reading, then something fucked up - Book 3 table 26
//'5F24' Application Expiration Date M
//'5A' Application Primary Account Number (PAN) M
//'8C' Card Risk Management Data Object List 1 M
//'8D' Card Risk Management Data Object List 2 M

/// <summary>
///     Indicates the location (SFI, range of records) of the AEFs related to a given application
/// </summary>
public record ApplicationFileLocator : DataElement<byte[]>, IEqualityComparer<ApplicationFileLocator>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = VariableDataElementCodec.Identifier;
    public static readonly Tag Tag = 0x94;
    private const byte _MaxByteLength = 248;
    private const byte _ByteLengthMultiple = 4;

    #endregion

    #region Constructor

    public ApplicationFileLocator(byte[] value) : base(value)
    {
        Validate(value);
    }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;

    /// <summary>
    ///     Gets a list containing Elementary Files and their relevant range of records that correspond to an Application
    ///     Dedicated File
    ///     Name
    /// </summary>
    /// <returns></returns>
    public RecordRange[] GetRecordRanges()
    {
        if (_Value.Length == 0)
            return Array.Empty<RecordRange>();

        RecordRange[] result = new RecordRange[_Value.Length / 4];

        for (int i = 0; i < _Value.Length; i++)
            result[i] = new RecordRange(_Value[i], _Value[++i], _Value[++i], _Value[++i]);

        return result;
    }

    public override Tag GetTag() => Tag;

    private static void Validate(ReadOnlySpan<byte> value)
    {
        // TODO: These are mandatory in the ICC
        //
        //'5F24' Application Expiration Date M
        //'5A' Application Primary Account Number
        //(PAN)ArgumentOutOfRangeException
        //    M
        //'8C' Card Risk Management Data Object List 1 M
        //'8D' Card Risk Management Data Object List 2 M

        if (value.Length > _MaxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationFileLocator)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {_MaxByteLength} bytes in length");
        }

        if ((value.Length % _ByteLengthMultiple) != 0)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationFileLocator)} must be a multiple of {_ByteLengthMultiple} to be correctly decoded");
        }
    }

    #endregion

    #region Serialization

    public static ApplicationFileLocator Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationFileLocator Decode(ReadOnlySpan<byte> value)
    {
        Validate(value);

        DecodedResult<byte[]> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<byte[]>
            ?? throw new InvalidOperationException(
                $"The {nameof(ApplicationFileLocator)} could not be initialized because the {nameof(VariableDataElementCodec)} returned a null {nameof(DecodedResult<byte[]>)}");

        return new ApplicationFileLocator(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, _Value, GetValueByteCount());
    public new byte[] EncodeTagLengthValue() => _Codec.EncodeTagLengthValue(this, GetValueByteCount());

    public override byte[] EncodeValue(BerCodec codec) =>
        codec.EncodeValue(BerEncodingId, _Value, _Value.Length + (_Value.Length % _ByteLengthMultiple));

    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(ApplicationFileLocator? x, ApplicationFileLocator? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationFileLocator obj) => obj.GetHashCode();

    #endregion
}