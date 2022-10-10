﻿using Play.Globalization.Time;

using System.ComponentModel.DataAnnotations;

namespace Play.Accounts.Contracts.Dtos
{
    public class PersonalInfoDto
    {
        #region Instance Values

        /// <summary>
        ///     The last four digits of the user's Social Security Number
        /// </summary>
        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        [RegularExpression("[\\d]{4}")]
        public string LastFourOfSocial { get; set; } = string.Empty;

        /// <summary>
        ///     The user's date of birth
        /// </summary>
        [Required]
        public DateTimeUtc DateOfBirth { get; set; }

        #endregion
    }
}