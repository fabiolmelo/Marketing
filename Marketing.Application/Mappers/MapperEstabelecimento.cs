
using Marketing.Application.DTOs;
using Marketing.Domain.Entidades;

namespace Marketing.Application.Mappers
{
    public static class MapperEstabelecimento
    {
        public static EstabelecimentoDto MapToDto(this Estabelecimento estabelecimento)
        {
            return new EstabelecimentoDto()
            {
                Cnpj = estabelecimento.Cnpj,
                Rede = estabelecimento.RedeNome,
                RazaoSocial = estabelecimento.RazaoSocial,
                Endereco = estabelecimento.Endereco,
                Numero = estabelecimento.Numero,
                Complemento = estabelecimento.Complemento,
                Bairro = estabelecimento.Bairro,
                Cidade = estabelecimento.Cidade,
                Uf = estabelecimento.Uf,
                Cep = estabelecimento.Cep
            };
        }
    }
}