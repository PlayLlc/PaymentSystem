﻿using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationInterchangeProfileTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x1C, 0x00};

    #endregion

    #region Constructor

    public ApplicationInterchangeProfileTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationInterchangeProfileTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => ApplicationInterchangeProfile.Tag;

    #endregion
}