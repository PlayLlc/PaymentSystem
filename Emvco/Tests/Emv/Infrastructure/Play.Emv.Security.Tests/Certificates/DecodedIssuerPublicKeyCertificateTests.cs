using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Emv.Security.Tests.Certificates
{
    [Trait("Type", "Unit")]
    public partial class DecodedIssuerPublicKeyCertificateTests : TestBase
    {
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
            //DecodedIssuerPublicKeyCertificate? test = null;
            //TerminalActionAnalysisService sut = new();
            //TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults(0));

            //CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOfflineOnly());
            //Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); });
        }

        #endregion
    }
}