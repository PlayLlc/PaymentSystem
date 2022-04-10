using System;

using Play.Ber.InternalFactories;
using Play.Testing.Infrastructure.BaseTestClasses;

namespace Play.Ber.Tests.TagTests.Tags.TagFactories;

public partial class TagTests : TestBase
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