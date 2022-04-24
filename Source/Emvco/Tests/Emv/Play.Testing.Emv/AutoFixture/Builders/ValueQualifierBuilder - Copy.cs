using AutoFixture.Kernel;

using Play.Ber.DataObjects;
using Play.Emv.Ber.Templates;
using Play.Emv.Ber.ValueTypes;
using Play.Icc.Exceptions;

namespace Play.Testing.Emv;

public class SetOfDirectoryEntryBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(SetOfDirectoryEntryBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="IccProtocolException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(SetOf<DirectoryEntry>))
            return new NoSpecimen();

        ValueQualifier[] all = ValueQualifier.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}