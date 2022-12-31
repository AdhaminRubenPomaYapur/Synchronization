using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using SystemTransaction.Dal.DBContext;
using SystemTransaction.EntityPostgre;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class ITransactionCompany
    {
        public int LastIdTransactionCompanyCloud(int companyid)
        {
            MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT lastid_transactioncompany('{companyid}')";
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

        public int LastIdTransactionCompanyLocal(int companyid)
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT lastid_transactioncompany('{companyid}')";
                return Convert.ToInt32(dbCommand.ExecuteScalar());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        public List<TransactionCompany>? GetAllDataCloudTransaction(int companyid)
        {
            try
            {
                MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
                return myTicketDbContext.TransactionCompanies.FromSqlRaw(@$"select * from ""TransactionCompany"" where companyid = {companyid}").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public List<TransactionCompany>? GetAllDataLocalTransaction(int companyid)
        {
            try
            {
                MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
                return myTicketDbContext.TransactionCompanies.FromSqlRaw(@$"select * from ""TransactionCompany"" where companyid = {companyid}").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public List<TransactionCompany>? LastIdTransactionsLocal(int transactioncompanyid, int companyid)
        {
            try
            {
                MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
                return myTicketDbContext.TransactionCompanies.FromSqlRaw(@$"select * from ""TransactionCompany"" where transactioncompanyid>{transactioncompanyid} and companyid = {companyid}").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public List<TransactionCompany>? LastIdTransactionsCloud(int transactioncompanyid, int companyid)
        {
            try
            {
                MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
                return myTicketDbContext.TransactionCompanies.FromSqlRaw(@$"select * from ""TransactionCompany"" where transactioncompanyid>{transactioncompanyid} and companyid = {companyid}").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error de transaction: " + e.Message);
                return null;
            }
        }
    }
}
