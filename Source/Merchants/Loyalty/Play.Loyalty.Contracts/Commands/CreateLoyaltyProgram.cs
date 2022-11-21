using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Loyalty.Contracts.Commands;

public record CreateLoyaltyProgram
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; }

    [Required]
    [StringLength(20)]
    public string UserId { get; set; }

    #endregion
}