using System.Threading.Tasks;

namespace Forms.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
