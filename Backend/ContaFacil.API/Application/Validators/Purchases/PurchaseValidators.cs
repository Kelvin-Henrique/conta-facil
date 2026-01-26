using ContaFacil.API.Application.Commands.Purchases;
using FluentValidation;

namespace ContaFacil.API.Application.Validators.Purchases;

public class CreatePurchaseCommandValidator : AbstractValidator<CreatePurchaseCommand>
{
    public CreatePurchaseCommandValidator()
    {
        RuleFor(x => x.CreditCardId)
            .NotEmpty().WithMessage("O ID do cartão de crédito é obrigatório");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória")
            .MaximumLength(200).WithMessage("A descrição não pode ter mais de 200 caracteres");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("A categoria é obrigatória")
            .MaximumLength(50).WithMessage("A categoria não pode ter mais de 50 caracteres");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("O valor total deve ser maior que zero");

        RuleFor(x => x.Installments)
            .InclusiveBetween(1, 24).WithMessage("O número de parcelas deve estar entre 1 e 24");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("A data da compra é obrigatória");
    }
}

public class UpdatePurchaseCommandValidator : AbstractValidator<UpdatePurchaseCommand>
{
    public UpdatePurchaseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID é obrigatório");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória")
            .MaximumLength(200).WithMessage("A descrição não pode ter mais de 200 caracteres");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("A categoria é obrigatória")
            .MaximumLength(50).WithMessage("A categoria não pode ter mais de 50 caracteres");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("O valor total deve ser maior que zero");

        RuleFor(x => x.Installments)
            .InclusiveBetween(1, 24).WithMessage("O número de parcelas deve estar entre 1 e 24");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("A data da compra é obrigatória");
    }
}
