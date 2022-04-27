//using System;

//using AutoFixture;

//using Play.Ber.Exceptions;
//using Play.Emv.Ber.DataElements;
//using Play.Emv.Kernel.Databases;
//using Play.Testing.Emv.Contactless.AutoFixture;

//using Xunit;

//using Play.Testing.BaseTestClasses;

//namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

//[Trait("Type", "Unit")]
//public partial class TerminalActionAnalysisServiceTests : TestBase
//{
//    #region Instance Values

//    private readonly IFixture _Fixture;

//    #endregion

//    #region Constructor

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    public TerminalActionAnalysisServiceTests()
//    {
//        _Fixture = new ContactlessFixture().Create();
//    }

//    #endregion

//    #region Instance Members

//    internal static KernelDatabase GetKernelDatabaseForOfflineOnly(IFixture fixture)
//    {
//        KernelDatabase database = ContactlessFixture.CreateDefaultDatabase(fixture);
//        database.Update(new TerminalType(TerminalType.CommunicationType.OnlineOnly));

//        return database;
//    }

//    internal KernelDatabase GetKernelDatabaseForOnlineOnly(IFixture fixture)
//    {
//        KernelDatabase database = ContactlessFixture.CreateDefaultDatabase(fixture);
//        database.Update(new TerminalType(TerminalType.CommunicationType.OnlineOnly));

//        return database;
//    }

//    internal KernelDatabase GetKernelDatabaseForOnlineAndOfflineCapable(IFixture fixture)
//    {
//        KernelDatabase database = ContactlessFixture.CreateDefaultDatabase(fixture);
//        database.Update(new TerminalType(TerminalType.CommunicationType.OnlineAndOfflineCapable));

//        return database;
//    }

//    #endregion
//}

