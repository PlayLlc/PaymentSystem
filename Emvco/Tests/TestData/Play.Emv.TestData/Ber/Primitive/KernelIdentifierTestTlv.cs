﻿using Play.Ber.Identifiers;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class KernelIdentifierTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = new byte[] {0x03};

    #endregion

    #region Constructor

    public KernelIdentifierTestTlv() : base(_DefaultContentOctets)
    { }

    public KernelIdentifierTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return KernelIdentifier.Tag;
    }

    #endregion
}