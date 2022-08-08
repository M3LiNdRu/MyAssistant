using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Library.MongoDb
{
    public interface IDataStore<T,C> where T : ICollectionDocument<C>
    {
        Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task InsertAsync(T document, CancellationToken cancellationToken);
        Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task DeleteAllAsync(CancellationToken cancellationToken);
        Task UpdateOneAsync(T document, CancellationToken cancellationToken);
        Task AppendToArrayAsync<TItem>(Expression<Func<T, bool>> predicate, Expression<Func<T, IEnumerable<TItem>>> item, List<TItem> elements, CancellationToken cancellationToken);
        Task RemoveFromArrayAsync<TItem>(Expression<Func<T, bool>> predicate, Expression<Func<T, IEnumerable<TItem>>> item, List<TItem> elements, CancellationToken cancellationToken);
    }
}