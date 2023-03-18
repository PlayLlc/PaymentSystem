using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Tests.TestDoubles.Aggregates.Brian.Rules;
using Play.Domain.Tests.TestDoubles.Aggregates.Greg.Rules;
using Play.Domain.Tests.TestDoubles.Aggregates.Shared.Rules;
using Play.Domain.Tests.TestDoubles.Dtos;

namespace Play.Domain.Tests.TestDoubles.Aggregates.Greg;

public class Greg : Aggregate<SimpleStringId>
{
    #region Instance Values

    private Name? _Name;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    public Greg()
    {
        Id = new SimpleStringId(GenerateSimpleStringId());
    }

    #endregion

    #region Instance Members

    public void UpdateName(Name name)
    {
        Enforce(new NameMustBeGreg(name));
        Enforce(new FirstCharacterMustBeCapitalized<Greg>(name));
        _Name = name;
    }

    public override SimpleStringId GetId() => Id;

    public override TestAggregateDto AsDto() =>
        new()
        {
            Id = Id,
            Name = _Name
        };

    #endregion
}