using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTransaction.EntityMongo
{
    public class MongoSynchronizationLocal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("LogTimestamp")]
        public DateTime LogTimestamp { get; set; }
        [BsonElement("datetimelastupdate")]
        public DateTime Datetimelastupdate { get; set; }
    }
}
