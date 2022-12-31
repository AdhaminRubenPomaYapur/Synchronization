using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using SystemTransaction.Dal.DBContext;
using SystemTransaction.EntityPostgre;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class ISynchronizationLocal
    {
        public int LastIdSynchronizationLocal()
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                dbConnection.Open();
                dbCommand.CommandText = "SELECT lastid_transactionlocal()";
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
        public bool InsertSynchronizationLocal(SynchronizationLocal synchronizationLocal)
        {
            MyTicketDbContextLocal db = new MyTicketDbContextLocal();
            IDbContextTransaction dbContextTransaction = db.Database.BeginTransaction();
            try
            {
                db.SynchronizationLocals.Add(synchronizationLocal);
                db.SaveChanges();
                dbContextTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbContextTransaction.Rollback();
                return false;
            }
        }
    }
}
