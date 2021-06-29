using System;
using System.Linq;
using fabiostefani.io.Vendas.Application.Commands;
using fabiostefani.io.Vendas.Domain;
using Xunit;

namespace fabiostefani.io.Vendas.Application.Tests.Pedidos
{
    public class AtualizarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Atualizar Item Command Válido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AtualizarItemPedidoCommand_ComandoEstaValido_DevePassarNaValidacao()
        {
            //ARRANGE
            var pedidoCommand = new AtualizarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), 2);

            //ACT
            var result = pedidoCommand.EhValido();

            //ASSERT
            Assert.True(result);
        }

        [Fact(DisplayName = "Atualizar Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AtualizarItemPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AtualizarItemPedidoCommand(Guid.Empty, Guid.Empty, 0);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AtualizarItemPedidoValidation.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AtualizarItemPedidoValidation.IdProdutoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));        
            Assert.Contains(AtualizarItemPedidoValidation.QtdMinErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));            
        }

        [Fact(DisplayName = "Atualizar Item Command unidades acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AtualizarItemPedidoCommand_QuantidadeUnidadesSuperiorAoPermitido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AtualizarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), Pedido.MAX_UNIDADES_ITEM + 1 );

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AdicionarItemPedidoValidation.QtdMaxErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }
}