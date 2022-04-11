using AutoFixture.Kernel;

namespace Play.Testing;

public abstract class SpecimenBuilderFactory
{
    #region Instance Values

    private readonly Dictionary<SpecimenBuilderId, ISpecimenBuilder> _Map;
    private readonly List<ISpecimenBuilder> _Buffer;

    #endregion

    #region Constructor

    protected SpecimenBuilderFactory(List<SpecimenBuilder> map)
    {
        _Buffer = new List<ISpecimenBuilder>();
        _Map = map.ToDictionary(a => a.GetId(), b => (ISpecimenBuilder) b);
    }

    #endregion

    #region Instance Members

    public void Build(params SpecimenBuilderId[] map)
    {
        foreach (SpecimenBuilderId mapItem in map)
            _Buffer.Add(_Map[mapItem]);
    }

    public List<ISpecimenBuilder> Create() => _Buffer;
    public void Clear() => _Buffer.Clear();

    #endregion
}