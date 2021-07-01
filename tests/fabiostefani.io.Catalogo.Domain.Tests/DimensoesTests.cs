using fabiostefani.io.Core.DomainObjects;
using Xunit;

namespace fabiostefani.io.Catalogo.Domain.Tests
{
    public class DimensoesTests
    {
        [Fact]
        public void Dimensoes_Construtor_ConstrutorDevemRetornarExceptions()
        {

            //Arrange & Act & Assert

            var ex = Assert.Throws<DomainException>(() =>
                new Dimensoes(0, 1, 1)
            );

            Assert.Equal("O campo Altura não pode ser menor ou igual a 0", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Dimensoes(1, 0, 1)
            );

            Assert.Equal("O campo Largura não pode ser menor ou igual a 0", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Dimensoes(1, 1, 0)
            );

            Assert.Equal("O campo Profundidade não pode ser menor ou igual a 0", ex.Message);

        }
    }
}