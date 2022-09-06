using FluentValidation;
using Play.MerchantPortal.Contracts.DTO;

namespace Play.MerchantPortal.Application.Services.Terminals;

internal class TerminalValidator : AbstractValidator<TerminalDto>
{
    public TerminalValidator()
    {

    }
}
