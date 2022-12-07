using Play.Domain.Common.Attributes;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Globalization.Time;

namespace Play.Payroll.Contracts.Commands;

public record RemoveEmployer
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    #endregion
}