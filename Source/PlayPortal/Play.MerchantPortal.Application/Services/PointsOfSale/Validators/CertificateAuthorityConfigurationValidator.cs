using FluentValidation;
using Play.MerchantPortal.Contracts.DTO.PointOfSale;

namespace Play.MerchantPortal.Application.Services.PointsOfSale.Validators;

public class CertificateAuthorityConfigurationValidator : AbstractValidator<CertificateAuthorityConfigurationDto>
{
    public CertificateAuthorityConfigurationValidator()
    {
        RuleFor(x => x.Certificates)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Certificates)
            .NotNull()
            .ChildRules(certificate =>
            {
                certificate.RuleFor(x => x.RegisteredApplicationProviderIndicator).NotEmpty();
                certificate.RuleFor(x => x.PublicKeyIndex).NotEmpty();
                certificate.RuleFor(x => (int)x.HashAlgorithmIndicator).GreaterThan(0);
                certificate.RuleFor(x => (int)x.PublicKeyAlgorithmIndicator).GreaterThan(0);
                certificate.RuleFor(x => x.ActivationDate).NotEmpty();
                certificate.RuleFor(x => x.ExpirationDate).NotEmpty();
                certificate.RuleFor(x => (int)x.Exponent).GreaterThan(0);
                certificate.RuleFor(x => x.Modulus).NotEmpty();
                certificate.RuleFor(x => x.Checksum).NotEmpty();
                certificate.RuleFor(x => x.CertificateSerialNumber).NotEmpty();
            });
    }
}
