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

        public async Task<FechamentoV1> GetFechamentoV1PorRede(string rede)
        {
            throw new NotImplementedException();
        }
    }
}