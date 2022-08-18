﻿using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class CardRiskManagementDataObjectList1TestTlv : TestTlv
{
    #region Static Metadata

    private static readonly byte[] _DefaultContentOctets =
    {
        0x61, 0x62, 0x63, 0x64, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
        0x20, 0x20, 0x20, 0x20, 0x20, 0x20
    };

    #endregion

    #region Constructor

    public CardRiskManagementDataObjectList1TestTlv() : base(_DefaultContentOctets)
    { }

    public CardRiskManagementDataObjectList1TestTlv(byte[] contentOctets) : base(contentOctets)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => CardRiskManagementDataObjectList1.Tag;

    #endregion
}