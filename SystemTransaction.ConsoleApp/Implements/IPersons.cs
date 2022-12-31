using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemTransaction.Dal.DBContext;
using SystemTransaction.EntityPostgre;

namespace SystemTransaction.ConsoleApp.Implements
{
    public class IPersons
    {
        public Person GetPersonByIdCloud(int personid)
        {
            MyTicketDbContextCloud myTicketDbContext = new MyTicketDbContextCloud();
            return myTicketDbContext.Persons.FromSqlRaw(@$"select * from ""Persons"" where ""PersonId"" = {personid}").First();
        }
        public Person GetPersonByIdLocal(int personid)
        {
            MyTicketDbContextLocal myTicketDbContext = new MyTicketDbContextLocal();
            return myTicketDbContext.Persons.FromSqlRaw(@$"select * from ""Persons"" where ""PersonId"" = {personid}").First();
        }

        public void InsertPersonLocal(Person person, ref MyTicketDbContextLocal myTicketDbContextLocal)
        {
            myTicketDbContextLocal.Database.ExecuteSqlRaw(@$"insert into ""Persons"" (""PersonId"", ""PersonName"", ""BirthDate"", ""Sex"", ""Password"", ""Email"", ""Mobile"", ""AuthMethod"") overriding system value values ({person.PersonId} ,'{person.PersonName}', '{person.BirthDate.ToString("yyyy-MM-dd")}', {person.Sex}, '{person.Password}', '{person.Email}', '{person.Mobile}', {person.AuthMethod});");
            return;
        }

        public void InsertPersonCloud(Person person, ref MyTicketDbContextCloud myTicketDbContextCloud)
        {
            myTicketDbContextCloud.Database.ExecuteSqlRaw(@$"insert into ""Persons"" (""PersonId"", ""PersonName"", ""BirthDate"", ""Sex"", ""Password"", ""Email"", ""Mobile"", ""AuthMethod"") overriding system value values ({person.PersonId} ,'{person.PersonName}', '{person.BirthDate.ToString("yyyy-MM-dd")}', {person.Sex}, '{person.Password}', '{person.Email}', '{person.Mobile}', {person.AuthMethod});");
            return;
        }
    }
}
