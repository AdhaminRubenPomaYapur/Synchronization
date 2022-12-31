using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTransaction.Dal.DBContext;
using SystemTransaction.EntityPostgre;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class ICompanyWalletTransactions
    {
        public List<CompanyWalletTransaction> GetCompanyWalletTrxByIdCloud(int companywalletid)
        {
            MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
            return myTicketDbContext.CompanyWalletTransactions.FromSqlRaw(@$"select * from ""CompanyWalletTransactions"" where ""CompanyWalletId"" = {companywalletid}").ToList();
        }
        public List<CompanyWalletTransaction> GetCompanyWalletTrxByIdLocal(int companywalletid)
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            return myTicketDbContext.CompanyWalletTransactions.FromSqlRaw(@$"select * from ""CompanyWalletTransactions"" where ""CompanyWalletId"" = {companywalletid}").ToList();
        }
        public void InsertCompanyWalletTransactionLocal(CompanyWalletTransaction companyWalletTransaction, ref MyTicketDbContextLocal myTicketDbContextLocal)
        {
            myTicketDbContextLocal.Database.ExecuteSqlRaw(@$"insert into ""CompanyWalletTransactions"" (""CompanyWalletTransactionId"",""TrxDate"", ""TrxDate"", ""Debit"", ""Credit"", ""Balance"", ""CompanyWalletId"") overriding system value values ({companyWalletTransaction.CompanyWalletTransactionId}, '{companyWalletTransaction.TrxDate.ToString("yyyy-MM-dd")}' , '{companyWalletTransaction.TrxDetail}', {companyWalletTransaction.Debit}, {companyWalletTransaction.Credit}, {companyWalletTransaction.Balance}, {companyWalletTransaction.CompanyWalletId});");
            return;
        }

        public void InsertCompanyWalletTransactionCloud(CompanyWalletTransaction companyWalletTransaction, ref MyTicketDbContextCloud myTicketDbContextCloud)
        {
            myTicketDbContextCloud.Database.ExecuteSqlRaw(@$"insert into ""CompanyWalletTransactions"" (""CompanyWalletTransactionId"",""TrxDate"", ""TrxDate"", ""Debit"", ""Credit"", ""Balance"", ""CompanyWalletId"") overriding system value values ({companyWalletTransaction.CompanyWalletTransactionId}, '{companyWalletTransaction.TrxDate.ToString("yyyy-MM-dd")}' , '{companyWalletTransaction.TrxDetail}', {companyWalletTransaction.Debit}, {companyWalletTransaction.Credit}, {companyWalletTransaction.Balance}, {companyWalletTransaction.CompanyWalletId});");
            return;
        }

        public int CountCompanyWalletTransactionsCloud(int companywalletid)
        {
            MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT count_companywallettransactions('{companywalletid}')";
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

        public int CountCompanyWalletTransactionsLocal(int companywalletid)
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT count_companywallettransactions('{companywalletid}')";
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
    }
}
