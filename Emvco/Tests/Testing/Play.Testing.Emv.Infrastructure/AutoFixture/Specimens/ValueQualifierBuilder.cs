﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders.Specimens;

namespace Play.Testing.Emv.Infrastructure.AutoFixture.Specimens
{
    internal class ValueQualifierBuilder : SpecimenBuilder
    {
        #region Static Metadata

        public static readonly SpecimenBuilderId Id = new(nameof(ValueQualifierBuilder));

        #endregion

        #region Instance Members

        public override SpecimenBuilderId GetId() => Id;

        /// <exception cref="Icc.Exceptions.IccProtocolException"></exception>
        public override object Create(object request, ISpecimenContext context)
        {
            Type? type = request as Type;

            if (type == null)
                return new NoSpecimen();

            if (type != typeof(ValueQualifier))
                return new NoSpecimen();

            ValueQualifier[] all = ValueQualifier.GetAll();

            return all[new Random().Next(0, all.Length - 1)];
        }

        #endregion
    }
}