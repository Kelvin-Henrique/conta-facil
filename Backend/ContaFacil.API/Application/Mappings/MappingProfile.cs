using AutoMapper;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Domain.Entities;

namespace ContaFacil.API.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Usuario
        CreateMap<Usuario, UsuarioDto>();
        CreateMap<CriarUsuarioDto, Usuario>();
        CreateMap<AtualizarUsuarioDto, Usuario>();

        // ContaBancaria
        CreateMap<ContaBancaria, ContaBancariaDto>();
        CreateMap<CriarContaBancariaDto, ContaBancaria>();
        CreateMap<AtualizarContaBancariaDto, ContaBancaria>();

        // CartaoCredito
        CreateMap<CartaoCredito, CartaoCreditoDto>();
        CreateMap<CriarCartaoCreditoDto, CartaoCredito>();
        CreateMap<AtualizarCartaoCreditoDto, CartaoCredito>();

        // Compra
        CreateMap<Compra, CompraDto>()
            .ForMember(dest => dest.NomeCartaoCredito, opt => opt.MapFrom(src => src.CartaoCredito.Nome));
        CreateMap<CriarCompraDto, Compra>();
        CreateMap<AtualizarCompraDto, Compra>();

        // TransacaoConta
        CreateMap<TransacaoConta, TransacaoContaDto>()
            .ForMember(dest => dest.NomeContaBancaria, opt => opt.MapFrom(src => src.ContaBancaria.Nome));
        CreateMap<CriarTransacaoContaDto, TransacaoConta>();
        CreateMap<AtualizarTransacaoContaDto, TransacaoConta>();

        // ContaFixa
        CreateMap<ContaFixa, ContaFixaDto>();
        CreateMap<CriarContaFixaDto, ContaFixa>();
        CreateMap<AtualizarContaFixaDto, ContaFixa>();
    }
}
