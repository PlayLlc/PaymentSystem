using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs;
using Play.Codecs.Integers;
using Play.Codecs.Strings;
using Play.Codecs.Stringsddd;
using Play.Core;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.Codecs;

public class CodecMapper
{
    #region Instance Values

    private readonly Dictionary<string, Encoding> _CodecMap = new()
    {
        {nameof(Alphabetic), PlayEncoding.Alphabetic},
        {nameof(AlphaNumeric), PlayEncoding.AlphaNumeric},
        {nameof(AlphaNumericSpecial), PlayEncoding.AlphaNumericSpecial},
        {nameof(AlphaSpecial), PlayEncoding.AlphaSpecial},

        // TODO: Needs to be UnsignedBinaryCodec. Move logic from Play.Emv.Codecs to Play.Codecs
        {nameof(Binary), PlayEncoding.Binary},
        {nameof(CompressedNumeric), PlayEncoding.CompressedNumeric},
        {nameof(Numeric), PlayEncoding.Numeric},
        {nameof(NumericSpecial), PlayEncoding.NumericSpecial},
        {nameof(SignedNumeric), PlayEncoding.SignedNumeric}
    };

    #endregion
}

public record DataFieldMetadata(int? MinLength, int? MaxLength);

internal class InterchangeCodec
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<DataFieldIdTypes, (DataFieldMetadata, PlayEncoding)> _CodecMap;

    #endregion

    #region Constructor

    static InterchangeCodec()
    {
        _CodecMap = new Dictionary<DataFieldIdTypes, (DataFieldMetadata, PlayEncoding)>()
        {
            {DataFieldIdTypes.PrimaryAccountNumber, (new DataFieldMetadata(null, 19), PlayEncoding.Numeric)}

            // .. etc
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public void GetBytes(List<byte> buffer, int offset, DataField dataField)
    {
        var codec = _CodecMap[dataField.GetDataFieldId()];

        // Logic for max & min length validation?

        dataField.CopyTo(buffer);
    }

    #endregion
}