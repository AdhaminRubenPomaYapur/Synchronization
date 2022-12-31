using MongoDB.Driver;
using SystemTransaction.EntityMongo;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class ILogs
    {
        public Logs? LastLog()
        {
            try
            {
                var client = new MongoClient("mongodb://backend:Passw0rd*@192.168.1.110:27017/myticketlogs?authSource=admin");
                var database = client.GetDatabase("myticketlogs");
                var logsdb = database.GetCollection<Logs>("log");
                return logsdb.Find(d => true).Sort(sort: "{_id:-1}").Limit(1).First();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en LastLog: "+e.Message);
                return null;
            }
        }
        public List<Logs>? getLastLogsDate(DateTime? dateTime)
        {
            try
            {
                var client = new MongoClient("mongodb://backend:Passw0rd*@192.168.1.110:27017/myticketlogs?authSource=admin");
                var database = client.GetDatabase("myticketlogs");
                var logsdb = database.GetCollection<Logs>("log");
                return logsdb.Find(d => d.Timestamp>dateTime).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en getLastLogsDate: " + e.Message);
                return null;
            }
        }
        public List<Logs>? getAll()
        {
            try
            {
                var client = new MongoClient("mongodb://backend:Passw0rd*@192.168.1.110:27017/myticketlogs?authSource=admin");
                var database = client.GetDatabase("myticketlogs");
                var logsdb = database.GetCollection<Logs>("log");
                return logsdb.Find(d => true).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en getAll: " + e.Message);
                return null;
            }
        }
        public bool Insert(Logs logs)
        {
            try
            {
                var client = new MongoClient("mongodb+srv://backend:Passw0rd*@cluster0.vafaprw.mongodb.net");
                var database = client.GetDatabase("myticketlogs");
                var logsdb = database.GetCollection<Logs>("log");
                logsdb.InsertOne(logs);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en Insert: " + e.Message);
                return false;
            }
        }
    }
}
