using Marketing.Domain.DTOs;
using Marketing.Domain.Entidades;

namespace Marketing.Application.Validation
{
    public class ValidationPlanilhaGrupoTrigo : ValidationPlanilha
    {
        private ResponseValidation responseValidation = new ResponseValidation();
        public override ResponseValidation Validate(List<DadosPlanilha> dadosPlanilhas)
        {
            ValidateDuplicateCnpj(dadosPlanilhas);
            ValidateDuplicateExtrato(dadosPlanilhas);
            return responseValidation;
        }

        private void ValidateDuplicateCnpj(List<DadosPlanilha> dadosPlanilhas)
        {
            var grupo = dadosPlanilhas.GroupBy(g=> new {g.Cnpj, g.Restaurante, g.Cidade, g.Uf, g.Rede});
            var duplicado = grupo.GroupBy(g=> g.Key.Cnpj)
                                 .Select(group => new { Cnpj = group.Key, Qtde = group.Count()})
                                 .Where(x=>x.Qtde > 1);
            foreach(var cnpj in duplicado)
            {
                foreach(var local in grupo.Where(x=>x.Key.Cnpj == cnpj.Cnpj)){
                    var linha = dadosPlanilhas.FirstOrDefault(x=>x.Cnpj == local.Key.Cnpj && 
                                                                 x.Restaurante.ToUpper() == local.Key.Restaurante.ToUpper() &&
                                                                 x.Cidade.ToUpper() == local.Key.Cidade.ToUpper() &&
                                                                 x.Uf.ToUpper() == local.Key.Uf.ToUpper() &&
                                                                 x.Rede.ToUpper() == local.Key.Rede.ToUpper())?.Linha ?? 0;
                    responseValidation.AdicionarErro(local.Key.Rede, linha, "Cadastro duplicado",
                                    $"Restaurante CNPJ: {cnpj.Cnpj} duplicados com dados diferentes. Rede: {local.Key.Rede}, Restaurante: {local.Key.Restaurante}, Cidade: {local.Key.Cidade}, UF: {local.Key.Uf}");  
                }
            }
        }

        private void ValidateDuplicateExtrato(List<DadosPlanilha> dadosPlanilhas)
        {
            var grupo = dadosPlanilhas.GroupBy(g=> new {g.Cnpj, g.Rede, g.AnoMes, g.TotalPedidos, g.PedidosComCocaCola, g.Meta});
            var duplicado = grupo.GroupBy(g=> new {g.Key.Cnpj, g.Key.Rede, g.Key.AnoMes})
                                 .Select(group => new { Cnpj = group.Key.Cnpj, group.Key.AnoMes, Qtde = group.Count()})
                                 .Where(x=>x.Qtde > 1);
            foreach(var cnpj in duplicado)
            {
                foreach(var local in grupo.Where(x=>x.Key.Cnpj == cnpj.Cnpj && x.Key.AnoMes == cnpj.AnoMes )){
                    var linha = dadosPlanilhas.FirstOrDefault(x=>x.Cnpj == local.Key.Cnpj && 
                                                                 x.AnoMes == local.Key.AnoMes &&
                                                                 x.TotalPedidos == local.Key.TotalPedidos &&
                                                                 x.PedidosComCocaCola == local.Key.PedidosComCocaCola &&
                                                                 x.Rede == local.Key.Rede &&
                                                                 x.Meta == local.Key.Meta)?.Linha ?? 0;
                    responseValidation.AdicionarErro(local.Key.Rede, linha, "Incidência mensal duplicada",
                                    $"Restaurante CNPJ: {cnpj.Cnpj} duplicados com dados diferentes. Rede: {local.Key.Rede}, Competência: {local.Key.AnoMes}, Total Pedidos: {local.Key.TotalPedidos}, Pedidos com Coca: {local.Key.PedidosComCocaCola}, Meta: {local.Key.Meta}");  
                }
            }
        }
    }
}