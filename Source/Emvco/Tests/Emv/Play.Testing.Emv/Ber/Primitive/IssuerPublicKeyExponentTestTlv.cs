﻿using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerPublicKeyExponentTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 48, 55 };

    public IssuerPublicKeyExponentTestTlv() : base(_DefaultContentOctets) { }

    public IssuerPublicKeyExponentTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerPublicKeyExponent.Tag;
}