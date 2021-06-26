using fabiostefani.io.Core.Data;

namespace fabiostefani.io.Vendas.Domain
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        void Adicionar(Pedido pedido);
    }
}