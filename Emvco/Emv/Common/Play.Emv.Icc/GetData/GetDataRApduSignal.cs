using Play.Ber.DataObjects;
using Play.Emv.Ber;

namespace Play.Emv.Icc.GetData;

public class GetDataRApduSignal : RApduSignal
{
    #region Static Metadata

    private static readonly EmvCodec _Codec = EmvCodec.GetBerCodec();

    #endregion

    #region Constructor

    public GetDataRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public TagLengthValue GetTagLengthValuesResult() => _Codec.DecodeTagLengthValue(_Data);

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}