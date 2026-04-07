using System.IO.Compression;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoExport : IServicoExport
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicoExport(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public byte[] ExportarFechamentoV1(List<FechamentoV1> fechamentos)
        {
            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach(var fechamento in fechamentos)
                {
                    var zipEntry = archive.CreateEntry($"Incidencia {fechamento.NomeRede}.xlsx", CompressionLevel.Optimal);
                    using var entryStream = zipEntry.Open();
                    using var excelStream = new MemoryStream(fechamento.Fechamento);
                    excelStream.CopyTo(entryStream);
                };
            }
            return memoryStream.ToArray();
        }

        public async Task<List<ExportacaoV1>> GetFechamentoV1PorRede(string rede)
        {
            return await _unitOfWork.repositorioExportacao.BuscarRelatorioV1(rede);
        }

        public async Task<List<FechamentoV1>> GerarFechamentoV1(string pathArquivoBase)
        {
            var fechamentos = new List<FechamentoV1>();
            var redes = await _unitOfWork.repositorioRede.GetAll();
            
            foreach(var rede in redes)
            {
                int row = 2;
                var linhaFechamento = await GetFechamentoV1PorRede(rede.Nome);
                using(var excel = new ClosedXML.Excel.XLWorkbook(pathArquivoBase))
                {
                    var planilha = excel.Worksheet(1);
                    int totalRows = linhaFechamento.Count + 1;
                    planilha.Row(2).InsertRowsBelow(totalRows);
                    foreach (var linha in linhaFechamento)
                    {
                        planilha.Cell(row, 1).Value = linha.AnoMes;
                        planilha.Cell(row, 2).Value = linha.UF;
                        planilha.Cell(row, 3).Value = linha.Cidade;
                        planilha.Cell(row, 4).Value = linha.CNPJ;
                        planilha.Cell(row, 5).Value = linha.Restaurante;
                        planilha.Cell(row, 6).Value = linha.PedidosTotal;
                        planilha.Cell(row, 7).Value = linha.PedidosCoca;
                        planilha.Cell(row, 8).Value = linha.IncidenciaReal;
                        planilha.Cell(row, 9).Value = linha.Meta;
                        planilha.Cell(row, 10).Value = linha.PrecoUnitario;
                        planilha.Cell(row, 11).Value = linha.PedidosNaoCapiturados;
                        planilha.Cell(row, 12).Value = linha.ReceitaBruta;
                        planilha.Cell(row, 13).Value = linha.Telefone;
                        row++;
                    }
                    using var memoryStream = new MemoryStream();
                    excel.SaveAs(memoryStream);
                    fechamentos.Add(new FechamentoV1(rede.Nome, memoryStream.ToArray()));
                    var dadosPlanilha = memoryStream.ToArray();
                }
            }
            return fechamentos;            
        }
    }
}