using Marketing.Domain.DTOs;
using Marketing.Domain.Entidades;

namespace Marketing.Application.Validation
{
    public class ValidationPlanilhaDominos : ValidationPlanilha
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
            var grupo = dadosPlanilhas.GroupBy(g=> new {g.Cnpj, g.Restaurante, g.Cidade, g.Uf});
            var duplicado = grupo.GroupBy(g=> g.Key.Cnpj)
                                 .Select(group => new { Cnpj = group.Key, Qtde = group.Count()})
                                 .Where(x=>x.Qtde > 1);
            foreach(var cnpj in duplicado)
            {
                foreach(var local in grupo.Where(x=>x.Key.Cnpj == cnpj.Cnpj)){
                    var linha = dadosPlanilhas.FirstOrDefault(x=>x.Cnpj == local.Key.Cnpj && 
                                                                 x.Restaurante.ToUpper() == local.Key.Restaurante.ToUpper() &&
                                                                 x.Cidade.ToUpper() == local.Key.Cidade.ToUpper() &&
                                                                 x.Uf.ToUpper() == local.Key.Uf.ToUpper())?.Linha ?? 0;
                    var planilha = dadosPlanilhas.FirstOrDefault(x=>x.Cnpj == local.Key.Cnpj && 
                                                                 x.Restaurante.ToUpper() == local.Key.Restaurante.ToUpper() &&
                                                                 x.Cidade.ToUpper() == local.Key.Cidade.ToUpper() &&
                                                                 x.Uf.ToUpper() == local.Key.Uf.ToUpper())?.Planilha ?? "";
                    responseValidation.AdicionarErro(planilha, linha, "Cadastro duplicado",
                                    $"Restaurante CNPJ: {cnpj} duplicados com dados diferentes. Restaurante: {local.Key.Restaurante}, Cidade: {local.Key.Cidade}, UF: {local.Key.Uf}");  
                }
            }
        }

        private void ValidateDuplicateExtrato(List<DadosPlanilha> dadosPlanilhas)
        {
            var grupo = dadosPlanilhas.GroupBy(g=> new {g.Cnpj, g.AnoMes, g.TotalPedidos, g.PedidosComCocaCola, g.Meta});
            var duplicado = grupo.GroupBy(g=> new {g.Key.Cnpj, g.Key.AnoMes})
                                 .Select(group => new { Cnpj = group.Key.Cnpj, group.Key.AnoMes, Qtde = group.Count()})
                                 .Where(x=>x.Qtde > 1);
            foreach(var cnpj in duplicado)
            {
                foreach(var local in grupo.Where(x=>x.Key.Cnpj == cnpj.Cnpj && x.Key.AnoMes == cnpj.AnoMes )){
                    var linha = dadosPlanilhas.FirstOrDefault(x=>x.Cnpj == local.Key.Cnpj && 
                                                                 x.AnoMes == local.Key.AnoMes &&
                                                                 x.TotalPedidos == local.Key.TotalPedidos &&
                                                                 x.PedidosComCocaCola == local.Key.PedidosComCocaCola &&
                                                                 x.Meta == local.Key.Meta)?.Linha ?? 0;
                    var planilha = dadosPlanilhas.FirstOrDefault(x=>x.Cnpj == local.Key.Cnpj && 
                                                                 x.AnoMes == local.Key.AnoMes &&
                                                                 x.TotalPedidos == local.Key.TotalPedidos &&
                                                                 x.PedidosComCocaCola == local.Key.PedidosComCocaCola &&
                                                                 x.Meta == local.Key.Meta)?.Planilha ?? "";
                    responseValidation.AdicionarErro(planilha, linha, "Incidência mensal duplicada",
                                    $"Restaurante CNPJ: {cnpj} duplicados com dados diferentes. Competência: {local.Key.AnoMes}, Total Pedidos: {local.Key.TotalPedidos}, Pedidos com Coca: {local.Key.PedidosComCocaCola}, Meta: {local.Key.Meta}");  
                }
            }
        }
    }
}