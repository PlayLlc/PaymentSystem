using Play.Domain.Common.Attributes;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Payroll.Contracts.Commands;

public record CreateEmployer
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    #endregion
}