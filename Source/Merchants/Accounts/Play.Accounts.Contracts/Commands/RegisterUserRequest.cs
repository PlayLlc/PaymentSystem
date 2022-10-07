using System.ComponentModel.DataAnnotations;

using Play.Globalization.Time;

namespace Play.Accounts.Contracts.Commands
{
    public class RegisterUserRequest
    {
        #region Instance Values

        /// <summary>
        ///     The street address of the user's home
        /// </summary>
        [Required]
        public string StreetAddress { get; set; }

        /// <summary>
        ///     The apartment number of the user's home
        /// </summary>
        public string ApartmentNumber { get; set; }

        /// <summary>
        ///     The zipcode of the user's home
        /// </summary>
        [Required]
        [MinLength(5)]
        [MaxLength(10)]
        public string Zipcode { get; set; }

        /// <summary>
        ///     The zipcode of the user's home
        /// </summary>
        [Required]
        [StringLength(2)]
        public string StateAbbreviation { get; set; }

        /// <summary>
        ///     The city where the user lives
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        ///     The user's first name
        /// </summary>
        [Required]
        [RegularExpression("[\x32-\x7E]{2,26}")]
        public string FirstName { get; set; }

        /// <summary>
        ///     The user's last name
        /// </summary>
        [Required]
        [RegularExpression("[\x32-\x7E]{2,26}")]
        public string LastName { get; set; }

        /// <summary>
        ///     The user's mobile phone number
        /// </summary>
        [Required]
        [Phone]
        public string Phone { get; set; }

        /// <summary>
        ///     The user's email address
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     The last four digits of the user's Social Security Number
        /// </summary>
        [Required]
        [StringLength(4)]
        [RegularExpression("[\\d]{4}")]
        public string LastFourOfSocial { get; set; }

        [Required]
        public DateTimeUtc DateOfBirth { get; set; }

        #endregion
    }
}