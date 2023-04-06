using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.Registration.Domain.ValueObjects;

namespace Play.Registration.Persistence.Sql.Configuration;

public static class DomainValueConverters
{
    #region Instance Members

    public static void ConfigureCommonConverters(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<BusinessType>().HaveConversion<BusinessTypeConverter>();
        configurationBuilder.Properties<MerchantCategoryCode>().HaveConversion<MerchantCategoryCodeConverter>();
        configurationBuilder.Properties<MerchantRegistrationStatus>().HaveConversion<MerchantRegistrationStatusConverter>();
        configurationBuilder.Properties<UserRegistrationStatus>().HaveConversion<UserRegistrationStatusConverter>();
    }

    #endregion

    public class BusinessTypeConverter : ValueConverter<BusinessType, string>
    {
        #region Constructor

        public BusinessTypeConverter() : base(x => x, y => new BusinessType(y))
        { }

        #endregion
    }

    public class MerchantCategoryCodeConverter : ValueConverter<MerchantCategoryCode, ushort>
    {
        #region Constructor

        public MerchantCategoryCodeConverter() : base(x => x, y => new MerchantCategoryCode(y))
        { }

        #endregion
    }

    public class MerchantRegistrationStatusConverter : ValueConverter<MerchantRegistrationStatus, string>
    {
        #region Constructor

        public MerchantRegistrationStatusConverter() : base(x => x, y => new MerchantRegistrationStatus(y))
        { }

        #endregion
    }

    public class UserRegistrationStatusConverter : ValueConverter<UserRegistrationStatus, string>
    {
        #region Constructor

        public UserRegistrationStatusConverter() : base(x => x, y => new UserRegistrationStatus(y))
        { }

        #endregion
    }
}