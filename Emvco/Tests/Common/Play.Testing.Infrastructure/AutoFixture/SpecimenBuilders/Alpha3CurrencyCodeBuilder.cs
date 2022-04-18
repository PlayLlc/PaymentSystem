using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture.Kernel;

using Play.Globalization.Currency;
using Play.Globalization.Language;
using Play.Icc.Exceptions;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders
{
    public class Alpha3CurrencyCodeBuilder : SpecimenBuilder
    {
        #region Static Metadata

        private static readonly List<Currency> _Currencies = CurrencyCodeRepository.GetAll();
        public static readonly SpecimenBuilderId Id = new(nameof(Alpha3CurrencyCodeBuilder));

        #endregion

        #region Instance Members

        public override SpecimenBuilderId GetId() => Id;

        /// <exception cref="IccProtocolException"></exception>
        /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
        public override object Create(object request, ISpecimenContext context)
        {
            Type? type = request as Type;

            if (type == null)
                return new NoSpecimen();

            if (type != typeof(Alpha3CurrencyCode))
                return new NoSpecimen();

            return _Currencies.ElementAt(_Random.Next(0, _Currencies.Count - 1)).GetAlpha3Code();
        }

        #endregion
    }
}