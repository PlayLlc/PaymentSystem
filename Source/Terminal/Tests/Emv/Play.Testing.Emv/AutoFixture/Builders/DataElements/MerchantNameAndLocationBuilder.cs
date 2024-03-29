﻿using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv;

public class MerchantNameAndLocationBuilder : PrimitiveValueSpecimenBuilder<MerchantNameAndLocation>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(MerchantNameAndLocationBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public MerchantNameAndLocationBuilder() : base(
        new DefaultPrimitiveValueSpecimen<MerchantNameAndLocation>(MerchantNameAndLocation.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() =>
        new byte[]
        {
            0x61, 0x62, 0x63, 0x64, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
            0x20, 0x20, 0x20, 0x20, 0x20
        };

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(MerchantNameAndLocation))
            return new NoSpecimen();

        return new MerchantNameAndLocation(Randomize.AlphaNumericSpecial.Chars(_Random.Next(10, 100)));
    }

    #endregion
}