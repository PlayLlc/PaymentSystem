using AutoFixture.Kernel;

using Play.Core.Extensions;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Randoms;

namespace Play.Emv.Ber.TestData.AutoFixture
{
    internal class RegisteredApplicationProviderIndicatorBuilder : Builder
    {
        #region Static Metadata

        public static readonly SpecimenBuilderId Id = new(nameof(RegisteredApplicationProviderIndicatorBuilder));

        #endregion

        #region Instance Members

        public override SpecimenBuilderId GetId() => new(nameof(RegisteredApplicationProviderIndicatorBuilder));

        /// <exception cref="Icc.Exceptions.IccProtocolException"></exception>
        public override object Create(object request, ISpecimenContext context)
        {
            Type? type = request as Type;

            if (type == null)
                return new NoSpecimen();

            if (type != typeof(RegisteredApplicationProviderIndicator))
                return new NoSpecimen();

            return new RegisteredApplicationProviderIndicator(CreateRandomFiveByteUlong());
        }

        private static ulong CreateRandomFiveByteUlong()
        {
            const ulong unrelatedValues = 0xFFFFFF0000000000;

            return Randomize.Integers.ULong().GetMaskedValue(unrelatedValues);
        }

        #endregion
    }
}