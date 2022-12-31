using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using SystemTransaction.Dal.DBContext;
using SystemTransaction.EntityPostgre;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class ISynchronizationCloudCompany
    {
        public int LastIdSynchronizationCloudCompany()
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                dbConnection.Open();
                dbCommand.CommandText = "SELECT lastid_transactioncloudcompany()";
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
        public void InsertSynchronizationCloud(SynchronizationCloudCompany synchronizationCloudCompany, ref MyTicketDbContextLocal myTicketDbContextLocal)
        {
            myTicketDbContextLocal.SynchronizationCloudCompanies.Add(synchronizationCloudCompany);
            return;
        }
    }
}
