using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using SystemTransaction.Dal.DBContext;
using SystemTransaction.EntityPostgre;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class ITransaction
    {
        public int LastIdTransactionCloud(int officeid)
        {
            MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT lastid_transaction('{officeid}')";
                return Convert.ToInt32(dbCommand.ExecuteScalar());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Synchronization: " + e.Message);
                return -1;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        public int LastIdTransactionLocal(int officeid)
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT lastid_transaction('{officeid}')";
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
        public List<Transaction>? GetAllDataCloudTransaction(int officeid)
        {   
            try
            {
                MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
                return myTicketDbContext.Transactions.FromSqlRaw(@$"select * from ""Transaction"" where officeid = {officeid}").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public List<Transaction>? GetAllDataLocalTransaction(int officeid)
        {
            try
            {
                MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
                return myTicketDbContext.Transactions.FromSqlRaw(@$"select * from ""Transaction"" where officeid = {officeid}").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public List<Transaction>? LastIdTransactionsLocal(int transactionid, int officeid)
        {
            try
            {
                MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
                return myTicketDbContext.Transactions.FromSqlRaw(@$"select * from ""Transaction"" where transactionid>{transactionid} and officeid = {officeid}").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public List<Transaction>? LastIdTransactionsCloud(int transactionid, int officeid)
        {
            try
            {
                MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
                return myTicketDbContext.Transactions.FromSqlRaw(@$"select * from ""Transaction"" where transactionid>{transactionid} and officeid = {officeid}").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error de transaction: " + e.Message);
                return null;
            }
        }

    }
}
