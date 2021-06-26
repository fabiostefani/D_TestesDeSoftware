using System.Threading.Tasks;

namespace fabiostefani.io.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}