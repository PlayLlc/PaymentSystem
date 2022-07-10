using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Globalization.Language;
using Play.Randoms;

namespace Play.Testing.Emv
{
    public class LanguagePreferenceBuilder : PrimitiveValueSpecimenBuilder<LanguagePreference>
    {
        #region Static Metadata

        public static readonly SpecimenBuilderId Id = new(nameof(LanguagePreferenceBuilder));

        #endregion

        #region Constructor

        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        public LanguagePreferenceBuilder() : base(
            new DefaultPrimitiveValueSpecimen<LanguagePreference>(new LanguagePreference(new Alpha2LanguageCode(new char[] {'E', 'N'})), GetContentOctets()))
        { }

        #endregion

        #region Instance Members

        private static byte[] GetContentOctets() => new byte[] {(byte) 'E', (byte) 'N'};
        public override SpecimenBuilderId GetId() => Id;

        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public override object Create(object request, ISpecimenContext context)
        {
            Type? type = request as Type;

            if (type == null)
                return new NoSpecimen();

            if (type != typeof(LanguagePreference))
                return new NoSpecimen();

            var a = new Alpha2LanguageCode(Randomize.Alpha.Chars(2));

            return new LanguagePreference(a);
        }

        #endregion
    }
}