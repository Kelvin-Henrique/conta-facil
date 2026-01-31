using ContaFacil.API.Application.Commands.FixedBills;
using FluentValidation;

namespace ContaFacil.API.Application.Validators.FixedBills;

public class CreateFixedBillCommandValidator : AbstractValidator<CriarContaFixaCommand>
{
    public CreateFixedBillCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome da conta é obrigatório")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.Categoria)
            .NotEmpty().WithMessage("A categoria é obrigatória")
            .MaximumLength(50).WithMessage("A categoria não pode ter mais de 50 caracteres");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("O valor deve ser maior que zero");

        RuleFor(x => x.DiaVencimento)
            .InclusiveBetween(1, 31).WithMessage("O dia de vencimento deve estar entre 1 e 31");

        RuleFor(x => x.Mes)
            .InclusiveBetween(0, 11).WithMessage("O mês deve estar entre 0 (Janeiro) e 11 (Dezembro)");

        RuleFor(x => x.Ano)
            .GreaterThanOrEqualTo(2020).WithMessage("O ano deve ser 2020 ou posterior");
    }
}

public class UpdateFixedBillCommandValidator : AbstractValidator<AtualizarContaFixaCommand>
{
    public UpdateFixedBillCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID é obrigatório");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome da conta é obrigatório")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.Categoria)
            .NotEmpty().WithMessage("A categoria é obrigatória")
            .MaximumLength(50).WithMessage("A categoria não pode ter mais de 50 caracteres");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("O valor deve ser maior que zero");

        RuleFor(x => x.DiaVencimento)
            .InclusiveBetween(1, 31).WithMessage("O dia de vencimento deve estar entre 1 e 31");
    }
}
