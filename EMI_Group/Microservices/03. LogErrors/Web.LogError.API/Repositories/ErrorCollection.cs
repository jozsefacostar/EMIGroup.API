using MongoDB.Bson;
using MongoDB.Driver;
using Models;

namespace Repositories
{
    public class ErrorCollection : IErrorCollection
    {

        internal MongoDbRepository _repository = new MongoDbRepository();
        private IMongoCollection<Error> Collection;

        public ErrorCollection()
        {
            Collection = _repository.db.GetCollection<Error>("Error");
        }

        public async Task<List<Error>> GetAllErrors()
        {
            return await Collection.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        public async Task DeleteError(string id)
        {
            var filter = Builders<Error>.Filter.Eq(x => x.Id, new ObjectId(id));
            await Collection.DeleteOneAsync(id);
        }
        public async Task<Error> GetErrorById(string iderror)
        {
            return await Collection.FindAsync(new BsonDocument { { "_id", iderror } }).Result.FirstAsync();
        }

        public async Task InsertError(Error error)
        {
            await Collection.InsertOneAsync(error);
        }

        public async Task UpdateError(Error error)
        {
            var filter = Builders<Error>
                .Filter
                .Eq(x => x.Id, error.Id);
            await Collection.ReplaceOneAsync(filter, error);
        }
    }
}
