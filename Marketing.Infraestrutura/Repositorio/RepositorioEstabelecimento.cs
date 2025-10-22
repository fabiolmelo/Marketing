﻿using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.PagedResponse;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioEstabelecimento : Repository<Estabelecimento>, IRepositorioEstabelecimento
    {
        private readonly DataContext _context;
        public RepositorioEstabelecimento(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public async Task<Estabelecimento?> FindEstabelecimentoIncludeContatoRede(string cnpj)
        {
            var estabelecimentos = await _context.Set<Estabelecimento>()
                                                .Include(x => x.ContatoEstabelecimentos)
                                                .Include(x => x.Rede)
                                                .Include(x => x.ExtratoVendas)
                                                .ToListAsync();
            var estabelecimento = estabelecimentos.Find(x => x.Cnpj == cnpj);
            return estabelecimento;
        }

        public async Task<Estabelecimento?> FindEstabelecimentoPorCnpj(string cnpj)
        {
            var estabelecimentos = await _context.Set<Estabelecimento>()
                                 .Include(x => x.Rede)
                                 .Include(x => x.ContatoEstabelecimentos)
                                 .Include(x => x.ExtratoVendas)
                                 .ToListAsync();
            var estabelecimento = estabelecimentos.Find(x => x.Cnpj == cnpj);
            return estabelecimento;
        }

        public async Task<List<Estabelecimento>> GetAllEstabelecimentoPorContato(string telefone)
        {
            IQueryable<Estabelecimento> query = from C in _context.Contatos.Where(x => x.Telefone == telefone)
                                                join CE in _context.ContatoEstabelecimento on C.Telefone equals CE.ContatoTelefone
                                                join E in _context.Estabelecimentos on CE.EstabelecimentoCnpj equals E.Cnpj
                                                select E;
            return await query.ToListAsync();
        }

        public async Task<List<Estabelecimento>> GetAllEstabelecimentoPorContatoQuePossuiCompetenciaVigente(string telefone)
        {
            var movimento = _context.Set<ExtratoVendas>().Any();
            var competenciaVigente = movimento ?
                                     await _context.Set<ExtratoVendas>().MaxAsync(x => x.Competencia) :
                                     new DateTime(1900, 1, 1);
            IQueryable<Estabelecimento> query = from C in _context.Contatos.Where(x => x.Telefone == telefone)
                                                join CE in _context.ContatoEstabelecimento on C.Telefone equals CE.ContatoTelefone
                                                join E in _context.Estabelecimentos on CE.EstabelecimentoCnpj equals E.Cnpj
                                                join EXV in _context.ExtratosVendas on E.Cnpj equals EXV.EstabelecimentoCnpj 
                                                where EXV.Competencia == competenciaVigente
                                                select E;
            return await query.ToListAsync();
        }

        public async Task<PagedResponse<List<Estabelecimento>>> GetAllEstabelecimentos(int? pageNumber, int? pageSize, string? filtro)
        {
            var query = _context.Set<Estabelecimento>().AsQueryable();
            var page = pageNumber ?? 1;
            var size = pageSize ?? 5;
            if (filtro != null)
            {
                query = query.Where(x => x.RazaoSocial.Contains(filtro) ||
                                         x.Cnpj.Contains(filtro) ||
                                         x.Cidade.Contains(filtro));
            }
            query = query.OrderBy(x => x.Cnpj);
            
            var totalRecords = await query.CountAsync();
            query = query.Skip((page - 1) * size)
                         .Take(size)
                         .Include(x => x.Rede)
                         .Include(x => x.ContatoEstabelecimentos)
                         .ThenInclude(X=>X.Contato); 
            return new PagedResponse<List<Estabelecimento>>(await query.ToListAsync(), page, size, totalRecords);
        }
    }
}
