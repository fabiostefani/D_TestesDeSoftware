using System.Collections.Generic;
using fabiostefani.io.Core.DomainObjects;

namespace fabiostefani.io.Catalogo.Domain
{
    public class Categoria : Entity
    {
        const string ValidacaoNome = "O campo Nome da Categoria não pode estar vazio";
        const string ValidacaoCodigo = "O campo Código não pode ser 0 (zero)";
        
        public string Nome { get; private set; }
        public int Codigo { get; private set; }
        //EF relation
        public ICollection<Produto> Produtos { get; set; }

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;

            Validar();
        }

        protected Categoria() { }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, ValidacaoNome);
            Validacoes.ValidarSeIgual(Codigo, 0, ValidacaoCodigo);
        }
    }
}