﻿using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationEffectiveDateTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {22, 07, 07};

    #endregion

    #region Constructor

    public ApplicationEffectiveDateTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationEffectiveDateTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ApplicationEffectiveDate.Tag;
}