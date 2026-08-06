using Microsoft.EntityFrameworkCore.Storage;

namespace FurnitureShop.Application.Interfaces.Common
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        Task<int> SaveChangesAsync();
    }
}