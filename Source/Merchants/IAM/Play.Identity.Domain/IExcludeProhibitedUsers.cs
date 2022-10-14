using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Identity.Application.Services.Registration.Users
{
    public interface IExcludeProhibitedUsers
    {
        #region Instance Members

        public bool IsUserProhibited(Address address, ContactInfo contactInfo);

        #endregion
    }
}