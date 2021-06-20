﻿using System;
using System.Collections.Generic;
using System.Linq;
using fabiostefani.io.Core.DomainObjects;

namespace fabiostefani.io.Vendas.Domain
{
    public class Pedido
    {
        public static int MAX_UNIDADES_ITEM => 15;
        public static int MIN_UNIDADES_ITEM => 1;
        
        public decimal ValorTotal { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }
        public Guid ClienteId { get; private set; }

        private readonly List<PedidoItem> _pedidoItens;
        public IReadOnlyCollection<PedidoItem> PedidoItens => _pedidoItens;

        protected Pedido()
        {
            _pedidoItens = new List<PedidoItem>();
        }
        public void AdicionarItem(PedidoItem pedidoItem)
        {
            if (pedidoItem.Quantidade > MAX_UNIDADES_ITEM)            
                throw new DomainException($"Máximo de {MAX_UNIDADES_ITEM} unidades por produto.");            

            if (_pedidoItens.Any(x=>x.ProdutoId == pedidoItem.ProdutoId))
            {
                var itemExistente = _pedidoItens.FirstOrDefault(x => x.ProdutoId == pedidoItem.ProdutoId);
                itemExistente.AtualizarUnidades(pedidoItem.Quantidade);
                pedidoItem = itemExistente;
                _pedidoItens.Remove(itemExistente);
            }


            _pedidoItens.Add(pedidoItem);
            CalcularValorPedido();
        }

        private void CalcularValorPedido()
        {
            ValorTotal = PedidoItens.Sum(x => x.CalcularValor());
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido()
                {
                    ClienteId = clienteId
                };
                pedido.TornarRascunho();
                return pedido;
            }
        }
    }

    public enum PedidoStatus
    {
        Rascunho = 0,
        Iniciado = 1,
        Pago = 4,
        Entregue = 5,
        Cancelado = 6
    }    
}