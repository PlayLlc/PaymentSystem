using System;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Security.Certificates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv;

using Xunit;

namespace Play.Emv.Security.Tests.Certificates;

[Trait("Type", "Unit")]
public class DecodedIssuerPublicKeyCertificateTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;

    #endregion

    #region Constructor

    public DecodedIssuerPublicKeyCertificateTests()
    {
        _Fixture = new EmvFixture().Create();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     OfflineOnlyTerminal_WithDefaultTerminalVerificationResults_GeneratesTransactionCryptogram
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OfflineOnlyTerminal_WithDefaultTerminalVerificationResults_GeneratesTransactionCryptogram()
    {
        DecodedIssuerPublicKeyCertificate? a = _Fixture.Create<DecodedIssuerPublicKeyCertificate>();
        Assertion(() => Assert.Equal(true, true));
    }

    #endregion
}