using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture.Kernel;

using Play.Core.Exceptions;
using Play.Globalization.Language;
using Play.Icc.Exceptions;
using Play.Randoms;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders
{
    public class ReadOnlySpanBuilder : SpecimenBuilder
    {
        #region Static Metadata

        private static readonly List<Language> _Languages = LanguageCodeRepository.GetAll();
        public static readonly SpecimenBuilderId Id = new(nameof(ReadOnlySpanBuilder));

        #endregion

        #region Instance Members

        public override SpecimenBuilderId GetId() => Id;

        /// <exception cref="IccProtocolException"></exception>
        /// <exception cref="PlayInternalException"></exception>
        public override object Create(object request, ISpecimenContext context)
        {
            Type? type = request as Type;

            if (type == null)
                return new NoSpecimen();

            if (type == typeof(ReadOnlySpan<byte>))
                return CreateReadOnlySpanBytes();
            if (type == typeof(ReadOnlySpan<char>))
                return CreateReadOnlySpanChars();

            return new NoSpecimen();
        }

        private byte[] CreateReadOnlySpanBytes()
        {
            byte[] array = new byte[_Random.Next(0, 20)];
            _Random.NextBytes(array);

            return array;
        }

        private char[] CreateReadOnlySpanChars() => Randomize.AlphaNumeric.Chars(_Random.Next(0, 20));

        #endregion
    }
}