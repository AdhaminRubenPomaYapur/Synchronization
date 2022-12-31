using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemTransaction.EntityMongo
{
    public class Logs
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Timestamp")]
        public DateTime Timestamp { get; set; }
        [BsonElement("Level")]
        public string? Level { get; set; }
        [BsonElement("MessageTemplate")]
        public string? MessageTemplate { get; set; }
        [BsonElement("RenderedMessage")]
        public string? RenderedMessage { get; set; }
        [BsonElement("Exception")]
        public string? Exception { get; set; }
        [BsonElement("UtcTimestamp")]
        public string? UtcTimestamp { get; set; }
        [BsonElement("local")]
        public string? Local { get; set; }
    }
}
