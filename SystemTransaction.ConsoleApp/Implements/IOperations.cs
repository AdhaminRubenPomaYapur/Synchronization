using Microsoft.EntityFrameworkCore;
using SystemTransaction.Dal.DBContext;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class IOperations
    {
        
        public void ExecuteQueryLocal(string query, ref MyTicketDbContextLocal myTicketDbContextLocal)
        {
            myTicketDbContextLocal.Database.ExecuteSqlRaw(query);
            return;
        }
        public void ExecuteQueryCloud(string query, ref MyTicketDbContextCloud myTicketDbContextCloud)
        {
            myTicketDbContextCloud.Database.ExecuteSqlRaw(query);
            return;
        }
    }
}
