using FluentValidation;
using Opa.Credits.Application.DTOs;

namespace Opa.Credits.Application.Validations;

public class CreateCreditValidator : AbstractValidator<CreateCreditDto>
{
    public CreateCreditValidator()
    {
            
        RuleFor(x => x.AssociateIdentification)
            .NotEmpty().WithMessage("La identificación del asociado es requerida.");
            
        RuleFor(x => x.AssociateName)
            .NotEmpty().WithMessage("El nombre del asociado es requerido.");
            
        RuleFor(x => x.CreditType)
            .NotEmpty().WithMessage("El tipo de crédito es requerido.");
            
        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("La forma de pago es requerida.");
        
        RuleFor(x => x.RequestedValue)
            .GreaterThan(0).WithMessage("El valor solicitado debe ser mayor a 0.");
            
        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0).WithMessage("La tasa de interés no puede ser negativa.");
            
        RuleFor(x => x.NumberOfInstallments)
            .GreaterThan(0).WithMessage("El número de cuotas debe ser mayor a 0.");
    }
}
