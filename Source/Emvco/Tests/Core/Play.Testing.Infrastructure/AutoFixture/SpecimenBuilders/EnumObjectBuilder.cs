using AutoFixture.Kernel;

using Play.Core;
using Play.Core.Exceptions;
using Play.Globalization.Language;
using Play.Icc.Exceptions;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

public class EnumObjectBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(EnumObjectBuilder));

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

        if (type != typeof(Alpha2LanguageCode))
            return new NoSpecimen();

        Array a = (Array) typeof(EnumObject<>).GetMethod("GetAll").Invoke(null, null);

        return a.GetValue(_Random.Next(0, a.Length - 1));
    }

    #endregion
}