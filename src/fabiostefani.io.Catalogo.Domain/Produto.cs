using System;
using fabiostefani.io.Core.DomainObjects;

namespace fabiostefani.io.Catalogo.Domain
{
    public class Produto : Entity, IAggregateRoot
    {
        const string ValidacaoNome = "O campo Nome do produto não pode estar vazio";
        const string ValidacaoDescricao = "O campo Descricao do produto não pode estar vazio";
        const string ValidacaoCategoriaId = "O campo CategoriaId do produto não pode estar vazio";
        const string ValidacaoValorZero = "O campo Valor do produto não pode ser menor que 0 (zero)";
        const string ValidacaoImagem = "O campo Imagem do produto não pode estar vazio";
        const string ValidacaoEstoqueInsuficiente = "Estoque insuficiente";

        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Imagem { get; private set; }
        public int QuantidadeEstoque { get; private set; }

        public Guid CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; }

        public Dimensoes Dimensoes { get; private set; }

        protected Produto() { }
        public Produto(string nome, string descricao, bool ativo, decimal valor, Guid categoriaId, DateTime dataCadastro, string imagem, Dimensoes dimensoes)
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            DataCadastro = dataCadastro;
            Imagem = imagem;
            CategoriaId = categoriaId;
            Dimensoes = dimensoes;

            Validar();
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            CategoriaId = categoria.Id;
        }

        public void AlterarDescricao(string descricao)
        {
            Validacoes.ValidarSeVazio(descricao, ValidacaoDescricao);
            Descricao = descricao;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            if (!PossuiEstoque(quantidade)) throw new DomainException(ValidacaoEstoqueInsuficiente);

            QuantidadeEstoque -= quantidade;
        }

        public void ReporEstoque(int quantidade)
        {
            QuantidadeEstoque += quantidade;
        }

        public bool PossuiEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, ValidacaoNome);
            Validacoes.ValidarSeVazio(Descricao, ValidacaoDescricao);
            Validacoes.ValidarSeIgual(CategoriaId, Guid.Empty, ValidacaoCategoriaId);
            Validacoes.ValidarSeMenorQue(Valor, 1, ValidacaoValorZero);
            Validacoes.ValidarSeVazio(Imagem, ValidacaoImagem);

        }
    }
}