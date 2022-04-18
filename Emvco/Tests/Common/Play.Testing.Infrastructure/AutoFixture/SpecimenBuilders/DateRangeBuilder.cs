using AutoFixture.Kernel;

using Play.Globalization.Time;
using Play.Icc.Exceptions;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders
{
    public class DateRangeBuilder : SpecimenBuilder
    {
        #region Static Metadata

        public static readonly SpecimenBuilderId Id = new(nameof(DateRangeBuilder));

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

            if (type != typeof(DateRange))
                return new NoSpecimen();

            return new DateRange(ShortDate.Today, new ShortDate(new DateTimeUtc(DateTime.Now.ToUniversalTime().AddYears(_Random.Next(1, 6)))));
        }

        #endregion
    }
}