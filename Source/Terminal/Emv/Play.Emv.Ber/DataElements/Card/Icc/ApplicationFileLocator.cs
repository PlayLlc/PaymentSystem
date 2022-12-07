using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.Ber.DataElements;

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

    public static readonly PlayEncodingId EncodingId = HexadecimalCodec.EncodingId;
    public static readonly Tag Tag = 0x94;
    private const byte _MaxByteLength = 248;
    private const byte _ByteLengthMultiple = 4;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public ApplicationFileLocator(ReadOnlySpan<byte> value) : base(value.ToArray())
    {
        Validate(value);
    }

    #endregion

    #region Serialization

    public static ApplicationFileLocator Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override ApplicationFileLocator Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static ApplicationFileLocator Decode(ReadOnlySpan<byte> value)
    {
        Validate(value);

        return new ApplicationFileLocator(value);
    }

    public override byte[] EncodeValue() => _Value;
    public override byte[] EncodeValue(int length) => PlayCodec.HexadecimalCodec.Encode(_Value, length);

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

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;

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

    public bool IsOptimizedForEmv(KernelConfiguration kernelConfiguration)
    {
        if (!kernelConfiguration.IsMagstripeModeSupported())
            return false;

        if (_Value.AsSpan().Length < 4)
            return false;

        if (PlayCodec.BinaryCodec.DecodeToUInt32(_Value[..4]) != 0x08010100)
            return false;

        return true;
    }

    public bool IsOptimizedForMagstripe()
    {
        if (_Value.AsSpan().Length < 4)
            return false;

        if (PlayCodec.BinaryCodec.DecodeToUInt32(_Value[..4]) != 0x08010100)
            return false;

        return true;
    }

    public override Tag GetTag() => Tag;

    /// <exception cref="DataElementParsingException"></exception>
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
            throw new DataElementParsingException(
                $"The Primitive Value {nameof(ApplicationFileLocator)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {_MaxByteLength} bytes in length");
        }

        if ((value.Length % _ByteLengthMultiple) != 0)
        {
            throw new DataElementParsingException(
                $"The Primitive Value {nameof(ApplicationFileLocator)} must be a multiple of {_ByteLengthMultiple} to be correctly decoded");
        }
    }

    #endregion
}