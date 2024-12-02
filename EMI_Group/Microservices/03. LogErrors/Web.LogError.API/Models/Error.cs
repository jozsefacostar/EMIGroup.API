using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Error
    {
        [BsonId]
        public ObjectId Id { get;set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? Data { get; set; }
        public string Module { get; set; }
    }
}
