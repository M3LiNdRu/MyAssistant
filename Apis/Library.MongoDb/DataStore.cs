﻿using Library.MongoDb.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Library.MongoDb
{
    public class DataStore<T> : IDataStore<T> where T : ICollectionDocument
    {
        private readonly IMongoDatabase _db;
        private readonly string _collection;

        public DataStore(IOptionsMonitor<DbConfigurationSettings> options, string collection)
        {
            var mongo = new MongoClient(options.CurrentValue.ConnectionString);
            _db = mongo.GetDatabase(options.CurrentValue.Database);
            _collection = collection;
        }

        public async Task Delete(Expression<Func<T, bool>> predicate)
        {
            await _db.GetCollection<T>(_collection).DeleteOneAsync(predicate);
        }

        public async Task DeleteAll()
        {
            await _db.GetCollection<T>(_collection).DeleteManyAsync(FilterDefinition<T>.Empty);
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await _db.GetCollection<T>(_collection).Find<T>(FilterDefinition<T>.Empty).ToListAsync<T>();
        }

        public async Task<T> FindOne(Expression<Func<T, bool>> predicate)
        {
            return await _db.GetCollection<T>(_collection).Find<T>(predicate).FirstOrDefaultAsync();
        }

        public async Task Insert(T document)
        {
            await _db.GetCollection<T>(_collection).InsertOneAsync(document);
        }

        public async Task UpdateOne(T document)
        {
            var filter = Builders<T>.Filter.Eq(s => s.Id, document.Id);
            await _db.GetCollection<T>(_collection).ReplaceOneAsync(filter, document);
        }
    }
}
