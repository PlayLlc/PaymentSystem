﻿using Play.Ber.Identifiers;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class AmountAuthorizedNumericTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x01};

    #endregion

    #region Constructor

    public AmountAuthorizedNumericTestTlv() : base(_DefaultContentOctets)
    { }

    public AmountAuthorizedNumericTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return AmountAuthorizedNumeric.Tag;
    }

    #endregion
}