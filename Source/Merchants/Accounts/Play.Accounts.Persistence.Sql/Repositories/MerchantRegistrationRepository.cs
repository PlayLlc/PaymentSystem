using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Persistence.Sql.Persistence;
using Play.Persistence.Sql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Persistence.Sql.Repositoriesd
{
    public class MerchantRegistrationRepository : Repository<MerchantRegistration, string>
    {
        #region Instance Values

        private readonly DbSet<MerchantRegistration> _Set;

        #endregion

        #region Constructor

        public MerchantRegistrationRepository(UserIdentityDbContext dbContext) : base(dbContext)
        {
            _Set = dbContext.Set<MerchantRegistration>();
        }

        #endregion

        #region Instance Members

        /// <exception cref="EntityFrameworkRepositoryException"></exception>
        public override MerchantRegistration? GetById(string id)
        {
            try
            {
                return _DbSet.AsNoTracking().Include("_Address").Include("_BusinessInfo").FirstOrDefault(a => a.GetId().Equals(id));
            }
            catch (Exception ex)
            {
                // logging
                throw new EntityFrameworkRepositoryException(
                    $"The {nameof(MerchantRegistrationRepository)} encountered an exception retrieving {nameof(UserRegistration)} with the Identifier: [{id}]",
                    ex);
            }
        }

        /// <exception cref="EntityFrameworkRepositoryException"></exception>
        public override async Task<MerchantRegistration?> GetByIdAsync(string id)
        {
            try
            {
                return await _DbSet.Include("_Address").Include("_BusinessInfo").FirstOrDefaultAsync(a => a.GetId()!.Equals(id)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // logging
                throw new EntityFrameworkRepositoryException(
                    $"The {nameof(MerchantRegistrationRepository)} encountered an exception retrieving {nameof(UserRegistration)} with the Identifier: [{id}]",
                    ex);
            }
        }

        #endregion
    }
}