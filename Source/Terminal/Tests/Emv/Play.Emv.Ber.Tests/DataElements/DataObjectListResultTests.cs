using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Ber.Primitive;

using Xunit;

namespace Play.Emv.Ber.Tests.DataElements;

public class DataObjectListResultTests
{
    #region Instance Members

    [Fact]
    public void DataObjectListResult_InstantiateEmpty_NotNull()
    {
        DataObjectListResult sut = new DataObjectListResult(Array.Empty<TagLengthValue>());

        Assert.NotNull(sut);
    }

    [Fact]
    public void DataObjectListResult_AsTagLengthValues_ReturnsExpectedResult()
    {
        CvmResultsTestTlv cvmResultTlvs = new();
        TagLengthValue tlv = new(CvmResults.Tag, cvmResultTlvs.EncodeValue());

        CardholderNameTestTlv cardholderTlv = new();
        TagLengthValue tlv2 = new(CardholderName.Tag, cardholderTlv.EncodeValue());

        TransactionDateTestTlv transactionDateTlv = new();
        TagLengthValue tlv3 = new(TransactionDate.Tag, transactionDateTlv.EncodeValue());

        TagLengthValue[] expected = { tlv, tlv2, tlv3 };

        DataObjectListResult sut = new DataObjectListResult(tlv, tlv2, tlv3);

        Assert.Equal(expected, sut.AsTagLengthValues());
    }

    [Fact]
    public void DataObjectListResult_AsByteArray_ReturnsExpectedResult()
    {
        CvmResultsTestTlv cvmResultTlvs = new();
        TagLengthValue tlv = new(CvmResults.Tag, cvmResultTlvs.EncodeValue());

        CardholderNameTestTlv cardholderTlv = new();
        TagLengthValue tlv2 = new(CardholderName.Tag, cardholderTlv.EncodeValue());

        TransactionDateTestTlv transactionDateTlv = new();
        TagLengthValue tlv3 = new(TransactionDate.Tag, transactionDateTlv.EncodeValue());

        DataObjectListResult sut = new DataObjectListResult(tlv, tlv2, tlv3);

        TagLengthValue[] tlvs = { tlv, tlv2, tlv3 };
        byte[] expected = tlvs.SelectMany(x => x.EncodeTagLengthValue()).ToArray();

        Assert.Equal(expected, sut.AsByteArray());
    }

    [Fact]
    public void DataObjectListResult_ByteCount_ReturnsExpectedResult()
    {
        CvmResultsTestTlv cvmResultTlvs = new();
        TagLengthValue tlv = new(CvmResults.Tag, cvmResultTlvs.EncodeValue());

        CardholderNameTestTlv cardholderTlv = new();
        TagLengthValue tlv2 = new(CardholderName.Tag, cardholderTlv.EncodeValue());

        TransactionDateTestTlv transactionDateTlv = new();
        TagLengthValue tlv3 = new(TransactionDate.Tag, transactionDateTlv.EncodeValue());

        DataObjectListResult sut = new DataObjectListResult(tlv, tlv2, tlv3);

        TagLengthValue[] tlvs = { tlv, tlv2, tlv3 };
        int expectedByteCount = (int) tlvs.Sum(x => x.GetTagLengthValueByteCount());

        Assert.Equal(expectedByteCount, sut.ByteCount());
    }

    [Fact]
    public void DataObjectListResult_AsCommandTemplate_ReturnsExpectedResult()
    {
        CvmResultsTestTlv cvmResultTlvs = new();
        TagLengthValue tlv = new(CvmResults.Tag, cvmResultTlvs.EncodeValue());

        CardholderNameTestTlv cardholderTlv = new();
        TagLengthValue tlv2 = new(CardholderName.Tag, cardholderTlv.EncodeValue());

        TransactionDateTestTlv transactionDateTlv = new();
        TagLengthValue tlv3 = new(TransactionDate.Tag, transactionDateTlv.EncodeValue());

        DataObjectListResult sut = new DataObjectListResult(tlv, tlv2, tlv3);

        TagLengthValue[] tlvs = { tlv, tlv2, tlv3 };

        List<byte> buffer = new();
        for (int i = 0; i < tlvs.Length; i++)
            buffer.AddRange(tlvs[i].EncodeValue());

        CommandTemplate expected = new(new BigInteger(buffer.ToArray()));
        CommandTemplate actual = sut.AsCommandTemplate();
        Assert.Equal(expected, actual);
    }

    #endregion
}
