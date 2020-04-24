using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Parser.DAL.Context;

namespace Parser.DAL
{
    internal sealed class Repository : IRepository
    {
        private readonly ParserDbContext _dbContext;
        private bool _disposed;
        private IDbContextTransaction _transaction;
        private IsolationLevel? _isolationLevel;

        public Repository(ParserDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public IQueryable<T> Query<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public async Task BulkMergeAsync<T>(IEnumerable<T> entities, Expression<Func<T, object>> match) where T : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _dbContext.Set<T>().UpsertRange(entities).On(match).RunAsync();
        }

        public async Task InsertAsync<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task BulkInsertAsync<T>(IEnumerable<T> entities) where T : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
            StartNewTransactionIfNeeded();
            return await _dbContext.SaveChangesAsync();
        }

        public void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            StartNewTransactionIfNeeded();
            _isolationLevel = isolationLevel;
        }
        
        public void CommitTransaction()
        {
            _dbContext.SaveChanges();

            if (_transaction == null) return;
            
            _transaction.Commit();

            _transaction.Dispose();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            if (_transaction == null) return;

            _transaction.Rollback();

            _transaction.Dispose();
            _transaction = null;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void StartNewTransactionIfNeeded()
        {
            if (_transaction == null)
            {
                _transaction = _isolationLevel.HasValue
                    ? _dbContext.Database.BeginTransaction(_isolationLevel.GetValueOrDefault())
                    : _dbContext.Database.BeginTransaction();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                    _transaction?.Dispose();
                    _transaction = null;
                }
            }

            _disposed = true;
        }
    }
}