using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using SystemTransaction.Dal.DBContext;
using SystemTransaction.EntityPostgre;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class IClients
    {
        public List<Client>? GetClientsCloud(int companyid)
        {
			try
			{
                MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
                return myTicketDbContext.Clients.FromSqlRaw(@$"select * from ""Clients"" where ""CompanyId"" = {companyid}").ToList();
            }
			catch (Exception e)
			{
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public List<Client>? GetClientsLocal(int companyid)
        {
            try
            {
                MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
                return myTicketDbContext.Clients.FromSqlRaw(@$"select * from ""Clients"" where ""CompanyId"" = {companyid}").ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public int LastGetIdClientCloud(int companyid)
        {
            MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT lastid_clientcompany('{companyid}')";
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

        public int LastGetIdClientLocal(int companyid)
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT lastid_clientcompany('{companyid}')";
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

        public int CountClientsCloud(int companyid)
        {
            MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT count_clients('{companyid}')";
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

        public int CountClientsLocal(int companyid)
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT count_clients('{companyid}')";
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
