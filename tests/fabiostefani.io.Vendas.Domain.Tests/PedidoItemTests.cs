using System.Linq;
using System;
using Xunit;
using fabiostefani.io.Core.DomainObjects;

namespace fabiostefani.io.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {        
        [Fact(DisplayName = "Novo Item Pedido com Unidades abaixo do permitido")]
        [Trait("CATEGORIA", "Vendas - Pedido")]
        public void NovoItemPedido_UnidadesItensAbaixoDoPermitido_DeveRetornarException()
        {
            //ARRANGE //ACT //ASSERT            
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MIN_UNIDADES_ITEM - 1, 100));            
        }
    }
}