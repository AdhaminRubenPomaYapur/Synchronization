using MongoDB.Driver;
using SystemTransaction.EntityMongo;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class ISynchronizationMongoDb
    {
        public DateTime? LastDateLog()
        {
            try
            {
                var client = new MongoClient("mongodb://backend:Passw0rd*@192.168.1.110:27017/myticketlogs?authSource=admin");
                var database = client.GetDatabase("myticketlogs");
                var synchronizationdb = database.GetCollection<MongoSynchronizationLocal>("synchronizationtocloud");

                return synchronizationdb.Find(d => true).Sort(sort: "{_id:-1}").Limit(1).First().LogTimestamp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public bool Insert(MongoSynchronizationLocal mongoSynchronizationLocal)
        {
            try
            {
                var client = new MongoClient("mongodb://backend:Passw0rd*@192.168.1.110:27017/myticketlogs?authSource=admin");
                var database = client.GetDatabase("myticketlogs");
                var synchronizationdb = database.GetCollection<MongoSynchronizationLocal>("synchronizationtocloud");
                synchronizationdb.InsertOne(mongoSynchronizationLocal);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;                
            }
        }
    }
}
