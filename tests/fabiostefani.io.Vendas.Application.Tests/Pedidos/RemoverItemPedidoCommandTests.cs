using System;
using System.Linq;
using fabiostefani.io.Vendas.Application.Commands;
using fabiostefani.io.Vendas.Domain;
using Xunit;

namespace fabiostefani.io.Vendas.Application.Tests.Pedidos
{
    public class RemoverItemPedidoCommandTests
    {
        [Fact(DisplayName = "Remover Item Command Válido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void RemoverItemPedidoCommand_ComandoEstaValido_DevePassarNaValidacao()
        {
            //ARRANGE
            var pedidoCommand = new RemoverItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid());

            //ACT
            var result = pedidoCommand.EhValido();

            //ASSERT
            Assert.True(result);
        }

        [Fact(DisplayName = "Remover Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void RemoverItemPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new RemoverItemPedidoCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(RemoverItemPedidoValidation.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(RemoverItemPedidoValidation.IdProdutoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));                    
        }
    }
}