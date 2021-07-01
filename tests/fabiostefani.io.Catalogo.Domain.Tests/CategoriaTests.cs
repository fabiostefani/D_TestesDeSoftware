using fabiostefani.io.Core.DomainObjects;
using Xunit;

namespace fabiostefani.io.Catalogo.Domain.Tests
{
    public class CategoriaTests
    {
        [Fact]
        public void Categoria_Validar_ValidacoesDevemRetornarExceptions()
        {

            //Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Categoria(string.Empty, 1)
            );

            Assert.Equal("O campo Nome da Categoria não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Categoria("Nome", 0)
            );

            Assert.Equal("O campo Código não pode ser 0 (zero)", ex.Message);
        }
    }
}