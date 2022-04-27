using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv;
using Play.Testing.Emv.Ber.Constructed;

using Xunit;

namespace Play.Emv.Ber.Tests.Templates.FileControlInformation
{
    public class FileControlInformationAdfTests : TestBase
    {
        #region Instance Values

        private readonly IFixture _Fixture;

        #endregion

        #region Constructor

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="BerParsingException"></exception>
        public FileControlInformationAdfTests()
        {
            _Fixture = new EmvFixture().Create();
        }

        #endregion

        #region Instance Members

        /// <exception cref="BerParsingException"></exception>
        [Fact]
        public void Template_DecodingTemplate_CreatesConstructedValue()
        {
            FileControlInformationAdf fci = _Fixture.Create<FileControlInformationAdf>();

            FileControlInformationProprietaryAdf proprietary = fci.GetFileControlInformationProprietary();
            var encodedProprietary = proprietary.EncodeTagLengthValue();

            byte[] encodedTagLengthValue = fci.EncodeTagLengthValue();
            FileControlInformationProprietaryAdf proprietaryDecoded = FileControlInformationProprietaryAdf.Decode(encodedTagLengthValue);
        }

        #endregion
    }
}