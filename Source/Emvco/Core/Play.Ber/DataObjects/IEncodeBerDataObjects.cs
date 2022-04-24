using Play.Ber.Codecs;

namespace Play.Ber.DataObjects;

public interface IEncodeBerDataObjects : IRetrieveBerDataObjectMetadata
{
    #region Instance Members

    public TagLengthValue AsTagLengthValue(BerCodec codec);

    #endregion

    #region Serialization

    byte[] EncodeValue(BerCodec codec);
    byte[] EncodeTagLengthValue(BerCodec codec);

    #endregion
}