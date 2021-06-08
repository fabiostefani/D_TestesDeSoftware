using fabiostefani.io.Clientes.Core;

namespace fabiostefani.io.Clientes
{
     public interface IClienteRepository : IRepository<Cliente>
    {
        Cliente ObterPorEmail(string email);
    }
}