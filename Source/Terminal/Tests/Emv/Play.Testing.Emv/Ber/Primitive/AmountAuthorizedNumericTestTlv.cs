﻿using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class AmountAuthorizedNumericTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x00, 0x00, 0x00, 0x00, 0x00, 0x01};

    #endregion

    #region Constructor

    public AmountAuthorizedNumericTestTlv() : base(_DefaultContentOctets)
    { }

    public AmountAuthorizedNumericTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => AmountAuthorizedNumeric.Tag;

    #endregion
}