using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture.Kernel;

using Play.Core.Exceptions;
using Play.Encryption.Certificates;
using Play.Globalization.Time;
using Play.Icc.Exceptions;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilderss
{
    public class PublicKeyExponentBuilder : SpecimenBuilder
    {
        #region Static Metadata

        private static readonly PublicKeyExponent[] _Values = PublicKeyExponent.Empty.GetAll();
        public static readonly SpecimenBuilderId Id = new(nameof(PublicKeyExponentBuilder));

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

            if (type != typeof(PublicKeyExponent))
                return new NoSpecimen();

            return _Values[_Random.Next(0, _Values.Length - 1)];
        }

        #endregion
    }
}