using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture.Kernel;

using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Randoms;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders.Specimens;

namespace Play.Testing.Emv.Infrastructure.AutoFixture;

internal class TerminalVerificationResultCodesBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(TerminalVerificationResultCodesBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(TerminalVerificationResultCodes))
            return new NoSpecimen();

        TerminalVerificationResultCodes[]? all = TerminalVerificationResultCodes.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}