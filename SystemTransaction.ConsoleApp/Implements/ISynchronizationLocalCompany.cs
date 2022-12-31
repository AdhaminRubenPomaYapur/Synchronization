using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using SystemTransaction.Dal.DBContext;
using SystemTransaction.EntityPostgre;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class ISynchronizationLocalCompany
    {
        public int LastIdSynchronizationLocalCompany()
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                dbConnection.Open();
                dbCommand.CommandText = "SELECT lastid_transactionlocalcompany()";
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
        public void InsertSynchronizationLocal(SynchronizationLocalCompany synchronizationLocalCompany)
        {
            MyTicketDbContextLocal db = new MyTicketDbContextLocal();
            IDbContextTransaction dbContextTransaction = db.Database.BeginTransaction();
            try
            {
                db.SynchronizationLocalCompanies.Add(synchronizationLocalCompany);
                db.SaveChanges();
                dbContextTransaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbContextTransaction.Rollback();
            }
        }
    }
}
