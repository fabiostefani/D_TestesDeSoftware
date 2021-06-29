using System;
using System.Collections.Generic;
using fabiostefani.io.Core.DomainObjects;
using FluentValidation.Results;

namespace fabiostefani.io.Vendas.Domain.Vouchers
{
    public class Voucher : Entity
    {
        public string Codigo { get; private set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }
        public decimal? ValorDesconto { get; private set; }                 
        public decimal? PercentualDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }

        public ICollection<Pedido> Pedidos { get; set; }

        public Voucher(string codigo, TipoDescontoVoucher tipoDescontoVoucher, decimal? valorDesconto, decimal? percentualDesconto, int quantidade, DateTime dataValidade, bool ativo, bool utilizado)
        {
            Codigo = codigo;
            TipoDescontoVoucher = tipoDescontoVoucher;
            ValorDesconto = valorDesconto;
            PercentualDesconto = percentualDesconto;
            Quantidade = quantidade;
            DataValidade = dataValidade;
            Ativo = ativo;
            Utilizado = utilizado;
        }
  

        public ValidationResult ValidarSeAplicavel()
        {
            return new VoucherAplicavelValidation().Validate(this);
        }

    }

    public enum TipoDescontoVoucher
    {
        Porcentagem = 0,
        Valor = 1
    }
}