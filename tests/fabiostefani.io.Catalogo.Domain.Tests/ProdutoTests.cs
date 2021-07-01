using System;
using fabiostefani.io.Core.DomainObjects;
using Xunit;

namespace fabiostefani.io.Catalogo.Domain.Tests
{
    public class ProdutoTests
    {
        [Fact]
        public void Produto_Validar_ValidacoesDevemRetornarExceptions()
        {

            //Arrange & Act & Assert

            var ex = Assert.Throws<DomainException>(() =>
                new Produto(string.Empty, "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Nome do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", string.Empty, false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Descricao do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 0, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Valor do produto não pode ser menor que 0 (zero)", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.Empty, DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo CategoriaId do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, string.Empty, new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Imagem do produto não pode estar vazio", ex.Message);

            // ex = Assert.Throws<DomainException>(() =>
            //     new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(0, 1, 1))
            // );

            // Assert.Equal("O campo Altura não pode ser menor ou igual a 0", ex.Message);
        }

        [Fact]
        public void Produto_PossuiEstoque_DeveRetornarTrueQuandoPossuiEstoque()
        {
            //Given
            var produto = new Produto("Nome", "Descricao", false, 1, Guid.NewGuid(), DateTime.Now, "imagem", new Dimensoes(1, 1, 1));

            produto.ReporEstoque(10);

            Assert.True(produto.PossuiEstoque(1));
        }

        [Fact]
        public void Produto_PossuiEstoque_DeveRetornarFalseQuandoPossuiEstoque()
        {
            //Given
            var produto = new Produto("Nome", "Descricao", false, 1, Guid.NewGuid(), DateTime.Now, "imagem", new Dimensoes(1, 1, 1));

            produto.ReporEstoque(10);

            Assert.False(produto.PossuiEstoque(22));
        }

        [Fact]
        public void Produto_DebitarEstoque_DeveRetornarExceptionQuandoNaoTiverEstoqueDisponivel()
        {
            //arrange
            var produto = new Produto("Nome", "Descricao", false, 1, Guid.NewGuid(), DateTime.Now, "imagem", new Dimensoes(1, 1, 1));
            produto.ReporEstoque(10);
            // act & assert
            Assert.Throws<DomainException>(() => produto.DebitarEstoque(11));
        }

        [Fact]
        public void Produto_DebitarEstoque_NaoDeveRetornarExceptionQuandoTiverEstoqueDisponivel()
        {
            //arrange
            var produto = new Produto("Nome", "Descricao", false, 1, Guid.NewGuid(), DateTime.Now, "imagem", new Dimensoes(1, 1, 1));
            produto.ReporEstoque(10);
            // act & assert
            produto.DebitarEstoque(9);
            Assert.Equal(1, produto.QuantidadeEstoque);
        }

        [Fact]
        public void Produto_ReporEstoque_DeveIncrementarAQuantidadeEstoque()
        {
            //arrange
            var produto = new Produto("Nome", "Descricao", false, 1, Guid.NewGuid(), DateTime.Now, "imagem", new Dimensoes(1, 1, 1));
            produto.ReporEstoque(10);
            // act & assert            
            Assert.Equal(10, produto.QuantidadeEstoque);
        }

        [Fact]
        public void Produto_AlterarDescricao_DeveRetornarExceptionQuandoDescricaoNulo()
        {
            //arrange
            var produto = new Produto("Nome", "Descricao", false, 1, Guid.NewGuid(), DateTime.Now, "imagem", new Dimensoes(1, 1, 1));

            Assert.Throws<DomainException>(() => produto.AlterarDescricao(string.Empty));

            Assert.Throws<DomainException>(() => produto.AlterarDescricao(""));
        }

        [Fact]
        public void Produto_AlterarDescricao_DeveAtualizarOCampoDescricao()
        {
            //arrange
            var produto = new Produto("Nome", "Descricao", false, 1, Guid.NewGuid(), DateTime.Now, "imagem", new Dimensoes(1, 1, 1));

            produto.AlterarDescricao("Nova Descricao");

            Assert.Equal("Nova Descricao", produto.Descricao);
        }

        [Fact]
        public void Produto_AlterarCategoria_DeveAtualizarACategoria()
        {
            //arrange
            var produto = new Produto("Nome", "Descricao", false, 1, Guid.NewGuid(), DateTime.Now, "imagem", new Dimensoes(1, 1, 1));
            var categoria = new Categoria("Categoria", 10);

            produto.AlterarCategoria(categoria);

            Assert.Equal(categoria.Id, produto.CategoriaId);
        }
    }
}