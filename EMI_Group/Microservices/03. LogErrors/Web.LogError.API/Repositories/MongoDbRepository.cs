using MongoDB.Driver;

namespace Repositories
{
    public class MongoDbRepository
    {
        public MongoClient client;

        public IMongoDatabase db;

        public MongoDbRepository()
        {
            client = new MongoClient("mongodb://127.0.0.1:27017");
            db = client.GetDatabase("TransactionHistory");
        }
    }
}
