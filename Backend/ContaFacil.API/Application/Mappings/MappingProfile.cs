using AutoMapper;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Domain.Entities;

namespace ContaFacil.API.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();

        // BankAccount
        CreateMap<BankAccount, BankAccountDto>();
        CreateMap<CreateBankAccountDto, BankAccount>();
        CreateMap<UpdateBankAccountDto, BankAccount>();

        // CreditCard
        CreateMap<CreditCard, CreditCardDto>();
        CreateMap<CreateCreditCardDto, CreditCard>();
        CreateMap<UpdateCreditCardDto, CreditCard>();

        // Purchase
        CreateMap<Purchase, PurchaseDto>()
            .ForMember(dest => dest.CreditCardName, opt => opt.MapFrom(src => src.CreditCard.Name));
        CreateMap<CreatePurchaseDto, Purchase>();
        CreateMap<UpdatePurchaseDto, Purchase>();

        // AccountTransaction
        CreateMap<AccountTransaction, AccountTransactionDto>()
            .ForMember(dest => dest.BankAccountName, opt => opt.MapFrom(src => src.BankAccount.Name));
        CreateMap<CreateAccountTransactionDto, AccountTransaction>();
        CreateMap<UpdateAccountTransactionDto, AccountTransaction>();

        // FixedBill
        CreateMap<FixedBill, FixedBillDto>();
        CreateMap<CreateFixedBillDto, FixedBill>();
        CreateMap<UpdateFixedBillDto, FixedBill>();
    }
}
