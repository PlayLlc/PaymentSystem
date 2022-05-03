﻿using AutoFixture.Kernel;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Randoms;
using Play.Testing.Emv;

namespace Play.Testing.Emv;

public class FileControlInformationAdfBuilder : ConstructedValueSpecimenBuilder<FileControlInformationAdf>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationAdfBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public FileControlInformationAdfBuilder() : base(
        new DefaultConstructedValueSpecimen<FileControlInformationAdf>(FileControlInformationAdf.Decode(GetContentOctets().AsMemory()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() =>
        new byte[]
        {
            0x6F, 0x3A,

            // DedicatedFileName
            0x84, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10, 0x10,

            // FCI Proprietary
            0xA5, 0x2F,

            // Application Label
            0x50, 0x0C, 0x56, 0x49, 0x53, 0x41, 0x20, 0x50, 0x52, 0x45,
            0x50, 0x41, 0x49, 0x44,

            // FCI Discretionary Data
            0xBF, 0x0C, 0x00,

            // PDOL
            0x9F, 0x38, 0x1B, 0x9F, 0x66, 0x04, 0x9F, 0x02, 0x06, 0x9F,
            0x03, 0x06, 0x9F, 0x1A, 0x02, 0x95, 0x05, 0x5F, 0x2A, 0x02,
            0x9A, 0x03, 0x9C, 0x01, 0x9F, 0x37, 0x04, 0x9F, 0x4E, 0x14
        };

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(FileControlInformationAdf))
            return new NoSpecimen();

        return FileControlInformationAdf.Decode(GetContentOctets());
    }

    #endregion
}