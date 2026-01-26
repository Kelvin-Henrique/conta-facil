using ContaFacil.API.Application.Commands.CreditCards;
using FluentValidation;

namespace ContaFacil.API.Application.Validators.CreditCards;

public class CreateCreditCardCommandValidator : AbstractValidator<CreateCreditCardCommand>
{
    public CreateCreditCardCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do cartão é obrigatório")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.DueDay)
            .InclusiveBetween(1, 31).WithMessage("O dia de vencimento deve estar entre 1 e 31");

        RuleFor(x => x.ClosingDay)
            .InclusiveBetween(1, 31).WithMessage("O dia de fechamento deve estar entre 1 e 31");
    }
}

public class UpdateCreditCardCommandValidator : AbstractValidator<UpdateCreditCardCommand>
{
    public UpdateCreditCardCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID é obrigatório");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do cartão é obrigatório")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.DueDay)
            .InclusiveBetween(1, 31).WithMessage("O dia de vencimento deve estar entre 1 e 31");

        RuleFor(x => x.ClosingDay)
            .InclusiveBetween(1, 31).WithMessage("O dia de fechamento deve estar entre 1 e 31");
    }
}
