﻿using Play.Ber.Identifiers;
using Play.Emv.DataElements;

namespace Play.Emv.TestData.Ber.Primitive;

public class CvmResultsTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = new byte[] {0x02, 0x03, 0x00};

    #endregion

    #region Constructor

    public CvmResultsTestTlv() : base(_DefaultContentOctets)
    { }

    public CvmResultsTestTlv(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag()
    {
        return CvmResults.Tag;
    }

    #endregion
}