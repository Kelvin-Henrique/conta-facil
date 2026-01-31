using ContaFacil.API.Application.Commands.Purchases;
using FluentValidation;

namespace ContaFacil.API.Application.Validators.Purchases;

public class CreatePurchaseCommandValidator : AbstractValidator<CriarCompraCommand>
{
    public CreatePurchaseCommandValidator()
    {
        RuleFor(x => x.CartaoCreditoId)
            .NotEmpty().WithMessage("O ID do cartão de crédito é obrigatório");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória")
            .MaximumLength(200).WithMessage("A descrição não pode ter mais de 200 caracteres");

        RuleFor(x => x.Categoria)
            .NotEmpty().WithMessage("A categoria é obrigatória")
            .MaximumLength(50).WithMessage("A categoria não pode ter mais de 50 caracteres");

        RuleFor(x => x.ValorTotal)
            .GreaterThan(0).WithMessage("O valor total deve ser maior que zero");

        RuleFor(x => x.Parcelas)
            .InclusiveBetween(1, 24).WithMessage("O número de parcelas deve estar entre 1 e 24");

        RuleFor(x => x.Data)
            .NotEmpty().WithMessage("A data da compra é obrigatória");
    }
}

public class UpdatePurchaseCommandValidator : AbstractValidator<AtualizarCompraCommand>
{
    public UpdatePurchaseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID é obrigatório");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória")
            .MaximumLength(200).WithMessage("A descrição não pode ter mais de 200 caracteres");

        RuleFor(x => x.Categoria)
            .NotEmpty().WithMessage("A categoria é obrigatória")
            .MaximumLength(50).WithMessage("A categoria não pode ter mais de 50 caracteres");

        RuleFor(x => x.ValorTotal)
            .GreaterThan(0).WithMessage("O valor total deve ser maior que zero");

        RuleFor(x => x.Parcelas)
            .InclusiveBetween(1, 24).WithMessage("O número de parcelas deve estar entre 1 e 24");

        RuleFor(x => x.Data)
            .NotEmpty().WithMessage("A data da compra é obrigatória");
    }
}
