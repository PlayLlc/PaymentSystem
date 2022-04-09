using AutoFixture.Kernel;

using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders.Specimens;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

/// <summary>
///     A builder that builds a list of custom <see cref="ISpecimenBuilder" /> implementations
/// </summary>
public class SpecimenForeman
{
    #region Static Metadata

    private static readonly Dictionary<SpecimenBuilderId, ISpecimenBuilder> _Map;

    #endregion

    #region Instance Values

    public readonly SpecimenBuilderId RegisteredApplicationProviderIndicator = RegisteredApplicationProviderIndicatorSpecimenBuilder.Id;
    private readonly List<ISpecimenBuilder> _Buffer;

    #endregion

    #region Constructor

    static SpecimenForeman()
    {
        _Map = new Dictionary<SpecimenBuilderId, ISpecimenBuilder>()
        {
            {RegisteredApplicationProviderIndicatorSpecimenBuilder.Id, new RegisteredApplicationProviderIndicatorSpecimenBuilder()}
        };
    }

    public SpecimenForeman()
    {
        _Buffer = new List<ISpecimenBuilder>();
    }

    #endregion

    #region Instance Members

    public void Build(params SpecimenBuilderId[] map)
    {
        foreach (SpecimenBuilderId mapItem in map)
            _Buffer.Add(_Map[mapItem]);
    }

    public List<ISpecimenBuilder> Complete() => _Buffer;
    public void Clear() => _Buffer.Clear();

    #endregion
}