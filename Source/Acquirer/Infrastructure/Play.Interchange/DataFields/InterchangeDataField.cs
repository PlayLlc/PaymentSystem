﻿using Play.Codecs;
using Play.Interchange.Codecs.Concrete;
using Play.Interchange.Codecs.Dynamic;

namespace Play.Interchange.DataFields;

public abstract record InterchangeDataField : IRetrieveInterchangeFieldMetadata, IEncodeInterchangeFields
{
    #region Static Metadata

    protected static readonly InterchangeCodec _Codec = new();

    #endregion

    #region Instance Members

    public abstract DataFieldId GetDataFieldId();
    public abstract ushort GetByteCount();
    public abstract PlayEncodingId GetEncodingId();
    public abstract byte[] Encode();
    DataFieldSpan IEncodeInterchangeFields.AsDataField() => new(GetDataFieldId(), Encode());
    public abstract void Encode(Memory<byte> buffer, ref int offset);

    #endregion

    #region Serialization

    public abstract InterchangeDataField Decode(ReadOnlyMemory<byte> value);

    #endregion
}