using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Tests.Dtos;
using Play.Inventory.Domain.Aggregates;

namespace Play.Domain.Tests.Aggregates;

public class TestAggregate : Aggregate<SimpleStringId>
{
    #region Instance Values

    private Name? _Name;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    public TestAggregate()
    {
        Id = new SimpleStringId(GenerateSimpleStringId());
    }

    #endregion

    #region Instance Members

    public void UpdateName(Name name)
    {
        Enforce(new NameMustBeBrian(name));
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