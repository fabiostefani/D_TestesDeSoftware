using System;
using fabiostefani.io.Vendas.Application.Commands;
using Xunit;

namespace fabiostefani.io.Vendas.Application.Tests.Pedidos
{
    public class AdicionarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Adicionar Item Command Válido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AdicionarItemPedidoCommand_ComandoEstaValido_DevePassarNaValidacao()
        {
            //ARRANGE
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto teste", 2, 100);

            //ACT
            var result = pedidoCommand.EhValido();

            //ASSERT
            Assert.True(result);
        }
    }
}