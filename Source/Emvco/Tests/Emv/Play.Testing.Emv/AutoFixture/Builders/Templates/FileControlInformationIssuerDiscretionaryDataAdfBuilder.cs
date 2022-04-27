﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture.Kernel;

using Play.Emv.Ber.Templates;
using Play.Testing.Icc.Apdu;

namespace Play.Testing.Emv.AutoFixture.Builders.Templates;

public class FileControlInformationIssuerDiscretionaryDataAdfBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly byte[] RawTagLengthValue = new byte[] {0xBF, 0x0C, 0x00};
    public static readonly byte[] RawValue = new byte[] { };
    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationIssuerDiscretionaryDataAdfBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(FileControlInformationIssuerDiscretionaryDataAdf))
            return new NoSpecimen();

        return FileControlInformationIssuerDiscretionaryDataAdf.Decode(RawTagLengthValue);
    }

    #endregion
}