﻿using Play.Ber.Identifiers;
using Play.Emv.Ber.Templates;
using Play.Testing.Emv.Infrastructure.Ber.Primitive;

namespace Play.Testing.Emv.Infrastructure.Ber.Constructed;

public class ProcessingOptionsTestTlv : ConstructedTlv
{
    #region Static Metadata

    private static readonly TestTlv[] _DefaultChildren = {new ApplicationFileLocatorTestTlv(), new ApplicationInterchangeProfileTestTlv()};
    private static readonly Tag[] _ChildIndex = ProcessingOptions.ChildTags;

    #endregion

    #region Constructor

    public ProcessingOptionsTestTlv() : base(_ChildIndex, _DefaultChildren)
    { }

    public ProcessingOptionsTestTlv(TestTlv[] children) : base(_ChildIndex, children)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => ResponseMessageTemplate.Tag;

    #endregion
}