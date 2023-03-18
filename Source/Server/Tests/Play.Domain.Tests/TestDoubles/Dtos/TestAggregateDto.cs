using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.ValueObjects;

namespace Play.Domain.Tests.TestDoubles.Dtos;

public class TestAggregateDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; }

    public Name? Name { get; set; }

    #endregion
}