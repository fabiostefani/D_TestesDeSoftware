using System;
using System.Linq;
using fabiostefani.io.Vendas.Domain.Vouchers;
using Xunit;

namespace fabiostefani.io.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar Voucher Tipo Valor Valido")]
        [Trait("Categoria", "Vendas - Vocuher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            //ARRANGE
            var voucher = new Voucher("PROMO-15-REAIS", TipoDescontoVoucher.Valor, 15, null, 1, DateTime.Now.AddDays(30), true, false);
            
            //ACT
            var result = voucher.ValidarSeAplicavel();

            //ASSERT
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar Voucher Tipo Valor Inválido")]
        [Trait("Categoria", "Vendas - Vocuher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarInvalido()
        {
            //ARRANGE
            var voucher = new Voucher("", TipoDescontoVoucher.Valor, null, null, 0, DateTime.Now.AddDays(-1), false, true);
            
            //ACT
            var result = voucher.ValidarSeAplicavel();

            //ASSERT
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherAplicavelValidation.AtivoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.CodigoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.DataValidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.QuantidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.UtilizadoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.ValorDescontoErroMsg, result.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Validar Voucher Porcentagem Válido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherPorcentagem_DeveEstarValido()
        {
            var voucher = new Voucher("PROMO-15-OFF", TipoDescontoVoucher.Porcentagem, null, 15, 1, DateTime.Now.AddDays(30), true, false);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar Voucher Porcentagem Inválido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherPorcentagem_DeveEstarInvalido()
        {
            var voucher = new Voucher("", TipoDescontoVoucher.Porcentagem, null, null, 0, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherAplicavelValidation.AtivoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.CodigoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.DataValidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.QuantidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.UtilizadoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.PercentualDescontoErroMsg, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}