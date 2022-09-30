using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.PointOfSale.Domain.Entities;

public class PointOfSale
{
    #region Instance Values

    private readonly int _CompanyId;
    private readonly int _MerchantId;
    private readonly int _StoreId;
    private readonly int _TerminalId;

    #endregion

    #region Constructor

    public PointOfSale(int companyId, int merchantId, int storeId, int terminalId)
    {
        _CompanyId = companyId;
        _MerchantId = merchantId;
        _StoreId = storeId;
        _TerminalId = terminalId;
    }

    #endregion

    #region Instance Members

    public void Create(object command)
    {
        // Raise(new DomainEvent());
    }

    #endregion
}