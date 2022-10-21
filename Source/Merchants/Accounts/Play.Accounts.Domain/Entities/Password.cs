using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Contracts.Dtos;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Entities
{
    public class Password : Entity<string>
    {
        #region Instance Values

        public string HashedPassword;
        public DateTime CreatedOn;

        public string Id { get; }

        #endregion

        #region Constructor

        /// <exception cref="ValueObjectException"></exception>
        public Password(string id, string hashedPassword, DateTime createdOn)
        {
            if (createdOn.Kind != DateTimeKind.Utc)
                throw new ValueObjectException($"The {nameof(createdOn)} date must be in UTC format");

            Id = id;
            HashedPassword = hashedPassword;
            CreatedOn = createdOn;
        }

        #endregion

        #region Instance Members

        public bool IsExpired(TimeSpan validityPeriod)
        {
            return (DateTimeUtc.Now - CreatedOn) <= validityPeriod;
        }

        public override string GetId()
        {
            return Id;
        }

        public override PasswordDto AsDto()
        {
            return new PasswordDto
            {
                Id = Id,
                CreatedOn = CreatedOn,
                HashedPassword = HashedPassword
            };
        }

        #endregion
    }
}