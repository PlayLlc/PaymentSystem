using System;

using Play.Ber.InternalFactories;

namespace Play.Ber.Tests.TagTests.Tags.TagFactories;

public partial class TagTests
{
    #region Static Metadata

    private static readonly Random _Random = new();
    private static readonly TagLengthFactory _TagFactory;

    #endregion

    #region Constructor

    static TagTests()
    {
        _TagFactory = new TagLengthFactory();
    }

    #endregion
}