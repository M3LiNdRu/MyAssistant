using Library.MongoDb.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Library.MongoDb
{
    public class DataStore<T,C> : IDataStore<T,C> where T : ICollectionDocument<C>
    {
        private readonly IMongoDatabase _db;
        private readonly string _collection;

        public DataStore(IOptions<DbConfigurationSettings> options, string collection)
        {
            var mongo = new MongoClient(options.Value.ConnectionString);
            _db = mongo.GetDatabase(options.Value.Database);
            _collection = collection;
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            await _db.GetCollection<T>(_collection).DeleteOneAsync(predicate, cancellationToken);
        }

        public async Task DeleteAllAsync(CancellationToken cancellationToken)
        {
            await _db.GetCollection<T>(_collection).DeleteManyAsync(FilterDefinition<T>.Empty, cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken)
        {
            return await _db.GetCollection<T>(_collection).Find<T>(FilterDefinition<T>.Empty).ToListAsync<T>(cancellationToken);
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _db.GetCollection<T>(_collection).Find<T>(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task InsertAsync(T document, CancellationToken cancellationToken)
        {
            await _db.GetCollection<T>(_collection).InsertOneAsync(document, cancellationToken: cancellationToken);
        }

        public async Task UpdateOneAsync(T document, CancellationToken cancellationToken)
        {
            var filter = Builders<T>.Filter.Eq(s => s.Id, document.Id);
            await _db.GetCollection<T>(_collection).ReplaceOneAsync(filter, document, cancellationToken: cancellationToken);
        }
    }
}
