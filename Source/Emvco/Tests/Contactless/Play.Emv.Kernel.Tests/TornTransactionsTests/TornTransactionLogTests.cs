using System;
using System.Linq;
using System.Threading;

using AutoFixture;

using Moq;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.Services;
using Play.Globalization.Time;
using Play.Randoms;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

namespace Play.Emv.Kernel.Tests.TornTransactionsTests;

public class TornTransactionLogTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;

    private readonly ITlvReaderAndWriter _Database;
    private readonly Mock<IWriteToDek> _DataExchangeKernel;

    private readonly MaxNumberOfTornTransactionLogRecords _MaxRecords;
    private readonly MaxLifetimeOfTornTransactionLogRecords _MaxLifetime;

    private readonly IManageTornTransactions _SystemUnderTest;

    public TornTransactionLogTests()
    {
        _Fixture = new ContactlessFixture().Create();

        _Database = ContactlessFixture.CreateDefaultDatabase(_Fixture);
        _DataExchangeKernel = new Mock<IWriteToDek>(MockBehavior.Strict);

        _MaxRecords = new MaxNumberOfTornTransactionLogRecords(1);
        _MaxLifetime = new MaxLifetimeOfTornTransactionLogRecords(new Seconds(10));

        _SystemUnderTest = new TornTransactionLog(_MaxRecords, _MaxLifetime);
    }

    #endregion

    [Fact]
    public void TornRecord_InstantiateEmptyTornRecord_MissingPanExceptionIsThrown()
    {
        Assert.Throws<TerminalDataException>(() =>
        {
            TornRecord tornRecord = new TornRecord();
        });
    }

    [Fact]
    public void TornTransactionLog_AddLog_LogIsAddedSuccessfully()
    {
        //Arrange & Setup
        TornTransactionLogFixture.RegisterApplicationPan(_Fixture);
        TornTransactionLogFixture.RegisterApplicationPanSequenceNumber(_Fixture);
        TornRecord record = TornTransactionLogFactory.CreateDefaultTornRecord(_Fixture);

        //Act
        _SystemUnderTest.Add(record, _Database);
        bool exists =_SystemUnderTest.TryGet(record.GetKey(), out TornRecord? expected);

        //Assert
        Assert.True(exists);
        Assert.NotNull(expected);
        Assert.Equal(expected, record);
    }

    [Fact]
    public void TornTransactionLog_AddTornRecordWithMultiplePrimitives_LogIsAddedAndRetrievedSuccesfully()
    {
        //Arrange
        TornTransactionLogFixture.RegisterApplicationPan(_Fixture);
        TornTransactionLogFixture.RegisterApplicationPanSequenceNumber(_Fixture);

        ReadOnlySpan<byte> paymentAccountReference = Enumerable.Range(0, 29).Select(_ => (byte)Randomize.AlphaNumeric.Char()).ToArray();
        PaymentAccountReference primitive1 = PaymentAccountReference.Decode(paymentAccountReference);

        ReadOnlySpan<byte> encodedCardHolderName = stackalloc byte[] {
            0x54, 0x44, 0x43, 0x20, 0x42, 0x4C, 0x41, 0x43, 0x4B, 0x20,
            0x55, 0x4E, 0x4C, 0x49, 0x4D, 0x49, 0x54, 0x45, 0x44, 0x20,
            0x56, 0x49, 0x53, 0x41, 0x20, 0x20
        };

        CardholderName primitive2 = CardholderName.Decode(encodedCardHolderName);

        TornRecord tornRecord = TornTransactionLogFactory.CreateTornRecordWithPrimitives(_Fixture, new PrimitiveValue[]
        {
            primitive1, primitive2
        });

        _SystemUnderTest.Add(tornRecord, _Database);

        //Act

        _SystemUnderTest.TryGet(tornRecord.GetKey(), out TornRecord? expected);

        //Assert
        Assert.NotNull(expected);
        Assert.Equal(tornRecord, expected);
    }

    [Fact]
    public void TornTransactionLog_AddMoreThenMaximumNumberOfLogs_OldestRecordIsReturnedToTheKernel()
    {
        //Arrange & Setup
        TornTransactionLogFixture.RegisterApplicationPan(_Fixture);
        TornTransactionLogFixture.RegisterApplicationPanSequenceNumber(_Fixture);
        TornRecord record = TornTransactionLogFactory.CreateDefaultTornRecord(_Fixture);
        TornRecord secondRecord = TornTransactionLogFactory.CreateDefaultTornRecord(_Fixture);

        //Act
        _SystemUnderTest.Add(record, _Database);
        _SystemUnderTest.Add(secondRecord, _Database);

        TornRecord expected = _Database.Get<TornRecord>(TornRecord.Tag);

        //Assert
        Assert.NotNull(expected);
        Assert.Equal(expected, record);
    }

    [Fact]
    public void TornTransactionLog_AddMoreThenMaxRecordsWithDifferentPans_OldestRecordIsReturned()
    {
        //Arrange & Setup
        TornTransactionLogFixture.RegisterApplicationPan(_Fixture);
        TornTransactionLogFixture.RegisterApplicationPanSequenceNumber(_Fixture);
        TornRecord record = TornTransactionLogFactory.CreateDefaultTornRecord(_Fixture);

        byte[] applicationPanEncodedValue = new byte[] { 12, 13, 14, 15 };
        byte[] panSequenceNumberEncodedValue = new byte[] { 36 };

        TornRecord secondRecord = TornTransactionLogFactory.CreateDefaultTornRecordFromEncodedValues(applicationPanEncodedValue, panSequenceNumberEncodedValue);

        //Act
        _SystemUnderTest.Add(record, _Database);
        _SystemUnderTest.Add(secondRecord, _Database);

        TornRecord expected = _Database.Get<TornRecord>(TornRecord.Tag);

        //Assert
        Assert.NotNull(expected);
        Assert.Equal(expected, record);
    }

    [Fact]
    public void TornTransactionLog_AddMoreThenMaxRecordsWithDifferentPans_OldestRecordIsReturnedToTheKernelÁndDeletedFromLog()
    {
        //Arrange & Setup
        TornTransactionLogFixture.RegisterApplicationPan(_Fixture);
        TornTransactionLogFixture.RegisterApplicationPanSequenceNumber(_Fixture);
        TornRecord record = TornTransactionLogFactory.CreateDefaultTornRecord(_Fixture);

        byte[] applicationPanEncodedValue = new byte[] { 12, 13, 14, 15 };
        byte[] panSequenceNumberEncodedValue = new byte[] { 36 };

        TornRecord secondRecord = TornTransactionLogFactory.CreateDefaultTornRecordFromEncodedValues(applicationPanEncodedValue, panSequenceNumberEncodedValue);

        //Act
        _SystemUnderTest.Add(record, _Database);
        _SystemUnderTest.Add(secondRecord, _Database);

        bool exists = _SystemUnderTest.TryGet(record.GetKey(), out TornRecord? expected);

        //Assert
        Assert.False(exists);
        Assert.Null(expected);
    }

    [Fact]
    public void TornTransactionLog_AddingTornRecordCleansTheStaleOnes_RecordIsAddedSuccessfully()
    {
        //Arrange & Setup
        TornTransactionLogFixture.RegisterApplicationPan(_Fixture);
        TornTransactionLogFixture.RegisterApplicationPanSequenceNumber(_Fixture);
        TornRecord record = TornTransactionLogFactory.CreateDefaultTornRecord(_Fixture);
        _SystemUnderTest.Add(record, _Database);

        Thread.Sleep(10000);

        byte[] applicationPanEncodedValue = new byte[] { 12, 13, 14, 15 };
        byte[] panSequenceNumberEncodedValue = new byte[] { 36 };

        TornRecord secondRecord = TornTransactionLogFactory.CreateDefaultTornRecordFromEncodedValues(applicationPanEncodedValue, panSequenceNumberEncodedValue);
        _SystemUnderTest.Add(secondRecord, _Database);

        //Act
        bool exists = _SystemUnderTest.TryGet(record.GetKey(), out TornRecord? expected);

        //Assert
        Assert.False(exists);
        Assert.Null(expected);
    }

    [Fact]
    public void TornTransactionLog_AddedRecordBecomesStale_RecordIsCleanedUp()
    {
        //Arrange & Setup
        TornTransactionLogFixture.RegisterApplicationPan(_Fixture);
        TornTransactionLogFixture.RegisterApplicationPanSequenceNumber(_Fixture);
        TornRecord record = TornTransactionLogFactory.CreateDefaultTornRecord(_Fixture);

        _SystemUnderTest.Add(record, _Database);
        Thread.Sleep(11000);

        //Act
        bool exists = _SystemUnderTest.TryGet(record.GetKey(), out TornRecord? expected);

        //Assert
        Assert.False(exists);
        Assert.Null(expected);
    }

    [Fact]
    public void TornTransactionLog_RemoveAddedTornRecord_RecordIsCleanedUp()
    {
        //Arrange & Setup
        TornTransactionLogFixture.RegisterApplicationPan(_Fixture);
        TornTransactionLogFixture.RegisterApplicationPanSequenceNumber(_Fixture);
        TornRecord record = TornTransactionLogFactory.CreateDefaultTornRecord(_Fixture);

        _DataExchangeKernel.Setup(m => m.Enqueue(DekResponseType.TornRecord, It.IsAny<PrimitiveValue>()));

        _SystemUnderTest.Add(record, _Database);
        //Act

        _SystemUnderTest.Remove(_DataExchangeKernel.Object, record.GetKey());
        bool exists = _SystemUnderTest.TryGet(record.GetKey(), out TornRecord? expected);

        //Assert
        _DataExchangeKernel.Verify(m => m.Enqueue(DekResponseType.TornRecord, It.IsAny<PrimitiveValue>()), Times.Once);
        Assert.False(exists);
        Assert.Null(expected);
    }

    [Fact]
    public void TornTransactionLog_RemoveNonExistingLog_DataExchangeMessageNoTEnqueued()
    {
        //Arrange & Setup
        TornTransactionLogFixture.RegisterApplicationPan(_Fixture);
        TornTransactionLogFixture.RegisterApplicationPanSequenceNumber(_Fixture);
        TornRecord record = TornTransactionLogFactory.CreateDefaultTornRecord(_Fixture);

        //Act
        _SystemUnderTest.Remove(_DataExchangeKernel.Object, record.GetKey());
        bool exists = _SystemUnderTest.TryGet(record.GetKey(), out TornRecord? expected);

        //Assert
        _DataExchangeKernel.Verify(m => m.Enqueue(DekResponseType.TornRecord, It.IsAny<PrimitiveValue>()), Times.Never);
        Assert.False(exists);
        Assert.Null(expected);
    }
}
