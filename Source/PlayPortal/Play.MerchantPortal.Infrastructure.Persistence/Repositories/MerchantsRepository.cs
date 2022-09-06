﻿using MerchantPortal.Core.Entities;
using MerchantPortal.Infrastructure.Persistence.Sql;

using Microsoft.EntityFrameworkCore;

using Play.MerchantPortal.Application.Contracts.Persistence;

namespace MerchantPortal.Infrastructure.Persistence.Repositories;

internal class MerchantsRepository : Repository<MerchantEntity>, IMerchantsRepository
{
    #region Constructor

    public MerchantsRepository(MerchantPortalDbContext dbContext) : base(dbContext)
    { }

    #endregion

    #region Instance Members

    public async Task<MerchantEntity?> SelectById(long id)
    {
        return await _DbContext.Merchants.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    #endregion
}