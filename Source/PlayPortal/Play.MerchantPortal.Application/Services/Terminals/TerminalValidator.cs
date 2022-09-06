using FluentValidation;
using Play.MerchantPortal.Contracts.DTO;

namespace Play.MerchantPortal.Application.Services.Terminals;

public class TerminalValidator : AbstractValidator<TerminalDto>
{
    public TerminalValidator()
    {

    }
}
