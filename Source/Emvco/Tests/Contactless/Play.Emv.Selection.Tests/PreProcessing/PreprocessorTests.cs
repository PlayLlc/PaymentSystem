using AutoFixture;

using Play.Emv.Selection.Start;
using Play.Testing.Emv.Contactless.AutoFixture;

namespace Play.Emv.Selection.Tests.PreProcessing;

public class PreprocessorTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly Preprocessor _SystemUnderTest;

    #endregion

    #region Constructor

    public PreprocessorTests()
    {
        _Fixture = new ContactlessFixture().Create();
        _Fixture.RegisterGlobalizationProperties();

        _SystemUnderTest = new Preprocessor();
    }

    #endregion

    #region Instance Members



    #endregion
}
