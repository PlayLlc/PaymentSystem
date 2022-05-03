﻿using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv;

public class MerchantIdentifierBuilder : PrimitiveValueSpecimenBuilder<MerchantIdentifier>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(MerchantIdentifierBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public MerchantIdentifierBuilder() : base(
        new DefaultPrimitiveValueSpecimen<MerchantIdentifier>(MerchantIdentifier.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() => new byte[] {0x03};
    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(MerchantIdentifier))
            return new NoSpecimen();

        return new MerchantIdentifier(Randomize.AlphaNumericSpecial.Chars(15));
    }

    #endregion
}