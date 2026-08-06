using Microsoft.EntityFrameworkCore.Storage;

namespace LuxeLiving.Application.Interfaces.Common
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        Task<int> SaveChangesAsync();
    }
}