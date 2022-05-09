using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture.Kernel;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Randoms;

namespace Play.Testing.Emv.AutoFixture.Builders.DataElements
{
    public class ProcessingOptionsBuilder : ConstructedValueSpecimenBuilder<ProcessingOptions>
    {
        #region Static Metadata

        public static readonly SpecimenBuilderId Id = new(nameof(ProcessingOptionsBuilder));

        #endregion

        #region Constructor

        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        public ProcessingOptionsBuilder() : base(new DefaultConstructedValueSpecimen<ProcessingOptions>(ProcessingOptions.Decode(temp()), GetContentOctets()))
        { }

        #endregion

        #region Instance Members

        private static byte[] temp()
        {
            List<byte> buffer = new();
            buffer.AddRange(EmvFixture.ApplicationFileLocatorBuilder.GetDefaultEncodedTagLengthValue());
            buffer.AddRange(EmvFixture.ApplicationInterchangeProfileBuilder.GetDefaultEncodedTagLengthValue());

            var testTlv = new TagLengthValue(ProcessingOptions.Tag, buffer.ToArray());
            var encodedTagLengthValue = testTlv.EncodeTagLengthValue();
            var processingOptions = ProcessingOptions.Decode(encodedTagLengthValue);

            return processingOptions.EncodeTagLengthValue();
        }

        private static byte[] GetContentOctets() => new byte[] {0x02, 0x03, 0x00};
        public override SpecimenBuilderId GetId() => Id;

        /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
        public override object Create(object request, ISpecimenContext context)
        {
            Type? type = request as Type;

            if (type == null)
                return new NoSpecimen();

            if (type != typeof(ProcessingOptions))
                return new NoSpecimen();

            return new ProcessingOptions(null);
        }

        #endregion
    }
}