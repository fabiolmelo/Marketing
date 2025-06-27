using System;
using System.Collections.Generic;

namespace Marketing.Domain.Entidades
{
    public class Rede
    {
        public string Nome { get; set; }
        public DateTime DataCadastro { get; } = DateTime.Now;
        public virtual List<Estabelecimento> Estabelecimentos { get; set; } = new List<Estabelecimento>();
        public Rede(string nome)
        {
            Nome = nome;
        }
    }
}