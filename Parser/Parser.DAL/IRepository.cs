using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Parser.DAL
{
    public interface IRepository : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;
        Task InsertAsync<T>(T entity) where T : class;
        Task BulkInsertAsync<T>(IEnumerable<T> entities) where T : class;
        Task BulkMergeAsync<T>(IEnumerable<T> entities, Expression<Func<T, object>> match) where T : class;
        Task<int> SaveChangesAsync();
        void CommitTransaction();
        void RollbackTransaction();
        void SetIsolationLevel(IsolationLevel isolationLevel);
    }
}