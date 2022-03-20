using System;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GetDataResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GetDataResponse));

    private static readonly Tag[] _KnownTags = new Tag[]
    {
        OfflineAccumulatorBalance.Tag, ProtectedDataEnvelope1.Tag, ProtectedDataEnvelope2.Tag, ProtectedDataEnvelope3.Tag,
        ProtectedDataEnvelope4.Tag, ProtectedDataEnvelope5.Tag, UnprotectedDataEnvelope1.Tag, UnprotectedDataEnvelope2.Tag,
        UnprotectedDataEnvelope3.Tag, UnprotectedDataEnvelope1.Tag, UnprotectedDataEnvelope4.Tag, UnprotectedDataEnvelope5.Tag
    };

    #endregion

    #region Constructor

    public GetDataResponse(CorrelationId correlation, TransactionSessionId transactionSessionId, GetDataRApduSignal response) :
        base(correlation, MessageTypeId, transactionSessionId, response)
    { }

    #endregion

    #region Instance Members

    public TagLengthValue GetTagLengthValueResult() => EmvCodec.GetBerCodec().DecodeTagLengthValue(GetRApduSignal().GetData().AsSpan());

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public bool TryGetPrimitiveValue(out PrimitiveValue? result)
    {
        uint tag = _Codec.GetFirstTag(GetData());

        // According to Emv Book C-2 Table 5.16, these are the only possible values that can be returned
        // from a GET DATA RAPDU

        if (TryDecodingOfflineAccumulatorBalance(tag, out result))
            return true;
        if (TryDecodingProtectedDataEnvelope1(tag, out result))
            return true;
        if (TryDecodingProtectedDataEnvelope2(tag, out result))
            return true;
        if (TryDecodingProtectedDataEnvelope3(tag, out result))
            return true;
        if (TryDecodingProtectedDataEnvelope4(tag, out result))
            return true;
        if (TryDecodingProtectedDataEnvelope5(tag, out result))
            return true;
        if (TryDecodingUnprotectedDataEnvelope1(tag, out result))
            return true;
        if (TryDecodingUnprotectedDataEnvelope2(tag, out result))
            return true;
        if (TryDecodingUnprotectedDataEnvelope3(tag, out result))
            return true;
        if (TryDecodingUnprotectedDataEnvelope4(tag, out result))
            return true;
        if (TryDecodingUnprotectedDataEnvelope5(tag, out result))
            return true;

        return false;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingOfflineAccumulatorBalance(Tag tag, out PrimitiveValue? result)
    {
        if (tag != OfflineAccumulatorBalance.Tag)
        {
            result = null;

            return false;
        }

        result = OfflineAccumulatorBalance.Decode(GetData().AsMemory());

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingProtectedDataEnvelope1(Tag tag, out PrimitiveValue? result)
    {
        if (tag != ProtectedDataEnvelope1.Tag)
        {
            result = null;

            return false;
        }

        result = ProtectedDataEnvelope1.Decode(GetData().AsMemory());

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingProtectedDataEnvelope2(Tag tag, out PrimitiveValue? result)
    {
        if (tag != ProtectedDataEnvelope2.Tag)
        {
            result = null;

            return false;
        }

        result = ProtectedDataEnvelope2.Decode(GetData().AsMemory());

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingProtectedDataEnvelope3(Tag tag, out PrimitiveValue? result)
    {
        if (tag != ProtectedDataEnvelope3.Tag)
        {
            result = null;

            return false;
        }

        result = ProtectedDataEnvelope3.Decode(GetData().AsMemory());

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingProtectedDataEnvelope4(Tag tag, out PrimitiveValue? result)
    {
        if (tag != ProtectedDataEnvelope4.Tag)
        {
            result = null;

            return false;
        }

        result = ProtectedDataEnvelope4.Decode(GetData().AsMemory());

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingProtectedDataEnvelope5(Tag tag, out PrimitiveValue? result)
    {
        if (tag != ProtectedDataEnvelope5.Tag)
        {
            result = null;

            return false;
        }

        result = ProtectedDataEnvelope5.Decode(GetData().AsMemory());

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingUnprotectedDataEnvelope1(Tag tag, out PrimitiveValue? result)
    {
        if (tag != UnprotectedDataEnvelope1.Tag)
        {
            result = null;

            return false;
        }

        result = UnprotectedDataEnvelope1.Decode(GetData().AsMemory());

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingUnprotectedDataEnvelope2(Tag tag, out PrimitiveValue? result)
    {
        if (tag != UnprotectedDataEnvelope2.Tag)
        {
            result = null;

            return false;
        }

        result = UnprotectedDataEnvelope2.Decode(GetData().AsMemory());

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingUnprotectedDataEnvelope3(Tag tag, out PrimitiveValue? result)
    {
        if (tag != UnprotectedDataEnvelope3.Tag)
        {
            result = null;

            return false;
        }

        result = UnprotectedDataEnvelope3.Decode(GetData().AsMemory());

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingUnprotectedDataEnvelope4(Tag tag, out PrimitiveValue? result)
    {
        if (tag != UnprotectedDataEnvelope4.Tag)
        {
            result = null;

            return false;
        }

        result = UnprotectedDataEnvelope4.Decode(GetData().AsMemory());

        return true;
    }

    /// <exception cref="Ber.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private bool TryDecodingUnprotectedDataEnvelope5(Tag tag, out PrimitiveValue? result)
    {
        if (tag != UnprotectedDataEnvelope5.Tag)
        {
            result = null;

            return false;
        }

        result = UnprotectedDataEnvelope5.Decode(GetData().AsMemory());

        return true;
    }

    #endregion
}