using System;
using System.Linq;
using fabiostefani.io.Vendas.Application.Commands;
using fabiostefani.io.Vendas.Domain;
using Xunit;

namespace fabiostefani.io.Vendas.Application.Tests.Pedidos
{
    public class AplicarVoucherPedidoCommandTests
    {
        [Fact(DisplayName = "Aplicar Voucher Command Válido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AplicarVoucherPedidoCommand_ComandoEstaValido_DevePassarNaValidacao()
        {
            //ARRANGE
            var pedidoCommand = new AplicarVoucherPedidoCommand(Guid.NewGuid(), "COD15-OFF");

            //ACT
            var result = pedidoCommand.EhValido();

            //ASSERT
            Assert.True(result);
        }

        [Fact(DisplayName = "AplicarVoucher Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AplicarVoucherPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AplicarVoucherPedidoCommand(Guid.Empty, string.Empty);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AplicarVoucherPedidoValidation.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AplicarVoucherPedidoValidation.CodigoVocuherErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));                    
        }
    }
}