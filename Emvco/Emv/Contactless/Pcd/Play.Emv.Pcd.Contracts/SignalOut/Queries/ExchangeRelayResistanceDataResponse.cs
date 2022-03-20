using System;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Icc.Exceptions;
using Play.Icc.Messaging.Apdu;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ExchangeRelayResistanceDataResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GenerateApplicationCryptogramResponse));

    #endregion

    #region Instance Values

    private readonly PrimitiveValue[] _RelayResistanceResponseData;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="correlation"></param>
    /// <param name="transactionSessionId"></param>
    /// <param name="response"></param>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public ExchangeRelayResistanceDataResponse(
        CorrelationId correlation,
        TransactionSessionId transactionSessionId,
        ExchangeRelayResistanceDataRApduSignal response) : base(correlation, MessageTypeId, transactionSessionId, response)
    {
        if (GetStatusWords() == StatusWords._9000)
            _RelayResistanceResponseData = DecodeData(response);
        else
            _RelayResistanceResponseData = Array.Empty<PrimitiveValue>();
    }

    #endregion

    #region Instance Members

    public DeviceRelayResistanceEntropy GetDeviceRelayResistanceEntropy() => (DeviceRelayResistanceEntropy) _RelayResistanceResponseData[0];

    public MinTimeForProcessingRelayResistanceApdu GetMinTimeForProcessingRelayResistanceApdu() =>
        (MinTimeForProcessingRelayResistanceApdu) _RelayResistanceResponseData[1];

    public MaxTimeForProcessingRelayResistanceApdu GetMaxTimeForProcessingRelayResistanceApdu() =>
        (MaxTimeForProcessingRelayResistanceApdu) _RelayResistanceResponseData[2];

    public DeviceEstimatedTransmissionTimeForRelayResistanceRapdu GetDeviceEstimatedTransmissionTimeForRelayResistanceRapdu() =>
        (DeviceEstimatedTransmissionTimeForRelayResistanceRapdu) _RelayResistanceResponseData[3];

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private static PrimitiveValue[] DecodeData(ExchangeRelayResistanceDataRApduSignal rapdu)
    {
        if (rapdu.GetStatusWords() != StatusWords._9000)
        {
            throw new
                IccProtocolException($"The {nameof(GenerateApplicationCryptogramResponse)} was attempted to be read but the status code returned restricts that from happening");
        }

        PrimitiveValue[] primitiveValues = new PrimitiveValue[4];
        TagLengthValue[] tlv = ResponseMessageTemplate.DecodeData(rapdu);

        primitiveValues[0] = DeviceRelayResistanceEntropy.Decode(tlv.First(a => a.GetTag() == DeviceRelayResistanceEntropy.Tag)
                                                                     .EncodeValue().AsSpan());

        primitiveValues[1] =
            MinTimeForProcessingRelayResistanceApdu.Decode(tlv.First(a => a.GetTag() == MinTimeForProcessingRelayResistanceApdu.Tag)
                                                               .EncodeValue().AsSpan());

        primitiveValues[2] =
            MaxTimeForProcessingRelayResistanceApdu.Decode(tlv.First(a => a.GetTag() == MaxTimeForProcessingRelayResistanceApdu.Tag)
                                                               .EncodeValue().AsSpan());

        primitiveValues[3] =
            DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Decode(tlv.First(a => a.GetTag()
                                                                                        == DeviceEstimatedTransmissionTimeForRelayResistanceRapdu
                                                                                            .Tag).EncodeValue().AsSpan());

        return primitiveValues.ToArray();
    }

    #endregion
}