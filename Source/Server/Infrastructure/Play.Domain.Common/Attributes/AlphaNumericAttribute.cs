﻿using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Play.Codecs;

namespace Play.Domain.Common.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class AlphaNumericAttribute : ValidationAttribute
{
    #region Static Metadata

    private const string _Message = "The field must only contain Alphabetic and Numeric characters.";

    #endregion

    #region Constructor

    public AlphaNumericAttribute() : base(_Message)
    { }

    #endregion

    #region Instance Members

    public override string FormatErrorMessage(string name) => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, 1);

    public override bool IsValid(object? value)
    {
        if (value is not string str)
            return false;

        return PlayCodec.AlphaNumericCodec.IsValid(str);
    }

    #endregion
}