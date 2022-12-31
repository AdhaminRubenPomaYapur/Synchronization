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
    public class IServices
    {

        public List<Service> GetServicesByIdCloud(int companyTypeId)
        {
            MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
            return myTicketDbContext.Services.FromSqlRaw(@$"select * from ""Services"" where ""CompanyTypeId"" = {companyTypeId}").ToList();
        }
        public List<Service> GetServicesByIdLocal(int companyTypeId)
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            return myTicketDbContext.Services.FromSqlRaw(@$"select * from ""Services"" where ""CompanyTypeId"" = {companyTypeId}").ToList();
        }
        public void InsertServiceLocal(Service service, ref MyTicketDbContextLocal myTicketDbContextLocal)
        {
            myTicketDbContextLocal.Database.ExecuteSqlRaw(@$"insert into ""Services"" (""ServiceId"",""ServiceName"", ""ServiceState"", ""CompanyTypeId"") overriding system value values ({service.ServiceId}, '{service.ServiceName}' , {service.ServiceState}, {service.CompanyTypeId});");
            return;
        }

        public void InsertServiceCloud(Service service, ref MyTicketDbContextCloud myTicketDbContextCloud)
        {
            myTicketDbContextCloud.Database.ExecuteSqlRaw(@$"insert into ""Services"" (""ServiceId"" ,""ServiceName"", ""ServiceState"", ""CompanyTypeId"") overriding system value values ({service.ServiceId}, '{service.ServiceName}' , {service.ServiceState}, {service.CompanyTypeId});");
            return;
        }

        public int CountServicesCloud(int companytypeid)
        {
            MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT count_services('{companytypeid}')";
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

        public int CountServicesLocal(int companytypeid)
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            DbConnection dbConnection = myTicketDbContext.Database.GetDbConnection();
            DbCommand dbCommand = dbConnection.CreateCommand();
            try
            {
                dbConnection.Open();
                dbCommand.CommandText = $"SELECT count_services('{companytypeid}')";
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
