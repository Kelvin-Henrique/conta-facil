using ContaFacil.API.Application.Commands.BankAccounts;
using FluentValidation;

namespace ContaFacil.API.Application.Validators.BankAccounts;

public class CreateBankAccountCommandValidator : AbstractValidator<CreateBankAccountCommand>
{
    public CreateBankAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da conta é obrigatório")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.BankName)
            .NotEmpty().WithMessage("O nome do banco é obrigatório")
            .MaximumLength(100).WithMessage("O nome do banco não pode ter mais de 100 caracteres");

        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(0).WithMessage("O saldo não pode ser negativo");
    }
}

public class UpdateBankAccountCommandValidator : AbstractValidator<UpdateBankAccountCommand>
{
    public UpdateBankAccountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID é obrigatório");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da conta é obrigatório")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.BankName)
            .NotEmpty().WithMessage("O nome do banco é obrigatório")
            .MaximumLength(100).WithMessage("O nome do banco não pode ter mais de 100 caracteres");
    }
}
