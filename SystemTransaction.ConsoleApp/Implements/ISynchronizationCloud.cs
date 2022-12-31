using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using SystemTransaction.Dal.DBContext;
using SystemTransaction.EntityPostgre;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class ISynchronizationCloud
    {
        public int LastIdSynchronizationCloud()
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand    dbCommand    = dbConnection.CreateCommand();

            try
			{    
                dbConnection.Open();
                dbCommand.CommandText = "SELECT lastid_transactioncloud()";
                return Convert.ToInt32(dbCommand.ExecuteScalar());
            }
			catch (Exception e)
			{
                Console.WriteLine(e.Message);
				return -1;
			}
            finally
            {
                dbConnection.Close();
            }
        }
        public void InsertSynchronizationCloud(SynchronizationCloud synchronizationCloud, ref MyTicketDbContextLocal myTicketDbContextLocal)
        {
            myTicketDbContextLocal.SynchronizationClouds.Add(synchronizationCloud);
            return;
        }

    }
}
