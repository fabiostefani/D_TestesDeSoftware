using System;
using fabiostefani.io.Core.DomainObjects;

namespace fabiostefani.io.Vendas.Domain
{
    public class PedidoItem
    {
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            if (quantidade < Pedido.MIN_UNIDADES_ITEM)            
                throw new DomainException($"Mínimo de {Pedido.MIN_UNIDADES_ITEM} unidades por produto.");            
                
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public void AtualizarUnidades(int unidades)
        {
            Quantidade += unidades;
        }

        public decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}