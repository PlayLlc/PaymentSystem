using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ES.PurchaseAdjustment.Configuration
{
    public class SwaggerConfiguration
    {
        #region Instance Values

        public string[] Versions { get; set; } = Array.Empty<string>();
        public string ApplicationTitle { get; set; } = string.Empty;
        public string ApplicationDescription { get; set; } = string.Empty;

        #endregion
    }
}