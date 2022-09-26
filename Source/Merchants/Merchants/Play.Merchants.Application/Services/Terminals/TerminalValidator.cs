using FluentValidation;

using Play.Merchants.Contracts.DTO;

namespace Play.Merchants.Application.Services;

public class TerminalValidator : AbstractValidator<TerminalDto>
{
    #region Constructor

    public TerminalValidator()
    { }

    #endregion
}