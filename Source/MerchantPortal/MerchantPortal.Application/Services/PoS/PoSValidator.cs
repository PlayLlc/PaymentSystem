using FluentValidation;
using MerchantPortal.Application.DTO.PointOfSale;

namespace MerchantPortal.Application.Services.PoS;

public class PoSValidator : AbstractValidator<PoSConfigurationDto>
{
    public PoSValidator()
    {
        RuleFor(x => x.TerminalConfiguration)
            .NotNull();

        RuleFor(x => x.KernelConfiguration)
            .NotNull();

        RuleFor(x => x.DisplayConfiguration)
            .NotNull();

        RuleForEach(x => x.Combinations)
            .NotNull();

        RuleForEach(x => x.CertificateAuthorityConfiguration.Certificates)
            .NotNull();
    }
}
