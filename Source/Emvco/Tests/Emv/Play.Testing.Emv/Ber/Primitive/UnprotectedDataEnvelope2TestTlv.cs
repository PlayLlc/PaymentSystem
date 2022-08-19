﻿using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class UnprotectedDataEnvelope2TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = {
        0x42, 0x43, 0x54, 0x45, 0x53, 0x54, 0x20, 0x31, 0x32, 0x33,
        0x34, 0x35, 0x36, 0x37, 0x38
    };

    public UnprotectedDataEnvelope2TestTlv() : base(_DefaultContentOctets) { }

    public UnprotectedDataEnvelope2TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => UnprotectedDataEnvelope2.Tag;
}
