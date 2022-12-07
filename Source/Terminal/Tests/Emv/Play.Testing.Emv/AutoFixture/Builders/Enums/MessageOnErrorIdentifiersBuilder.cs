using AutoFixture.Kernel;

using Play.Emv.Ber.Enums;

namespace Play.Testing.Emv;

public class MessageOnErrorIdentifiersBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(MessageOnErrorIdentifiersBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(DisplayMessageOnErrorIdentifiers))
            return new NoSpecimen();

        DisplayMessageIdentifiers[] all = DisplayMessageIdentifiers.Empty.GetAll();

        return (DisplayMessageOnErrorIdentifiers) all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}