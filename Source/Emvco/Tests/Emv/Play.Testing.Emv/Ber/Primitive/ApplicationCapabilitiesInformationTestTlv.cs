﻿using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationCapabilitiesInformationTestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets = {0x56, 0x49, 0x53};

    #endregion

    #region Constructor

    public ApplicationCapabilitiesInformationTestTlv() : base(_DefaultContentOctets)
    { }

    public ApplicationCapabilitiesInformationTestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    public override Tag GetTag() => ApplicationCapabilitiesInformation.Tag;
}