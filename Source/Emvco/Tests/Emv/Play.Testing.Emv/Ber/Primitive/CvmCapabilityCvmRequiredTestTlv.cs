﻿using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CvmCapabilityCvmRequiredTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {13};

    #endregion

    #region Constructor

    public CvmCapabilityCvmRequiredTestTlv() : base(_DefaultContentOctets)
    { }

    public CvmCapabilityCvmRequiredTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => CvmCapabilityCvmRequired.Tag;
}