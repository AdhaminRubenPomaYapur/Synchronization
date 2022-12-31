using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Serilog;
using SystemTransaction.ConsoleApp.Implements;
using SystemTransaction.Dal.DBContext;
using SystemTransaction.EntityMongo;
using SystemTransaction.EntityPostgre;



namespace SystemTransaction.ConsoleApp
{

    internal class Program
    {
        int officeId = 21;
        int companyId = 25;
        int companyTypeId = 4;
        int companyWalletId = 10;
        static void Main(string[] args)
        {
            Program program = new Program();
            program.SynchronizationClients();
            program.SynchronizationCloudToLocalCompany();
            program.SynchronizationLocalToCloudCompany();
            program.SynchronizationCloudToLocalOffice();
            program.SynchronizationLocalToCloudOffice();
            program.SynchronizationLocalToCloudMongoDB();
        }

        public void SynchronizationLocalToCloudMongoDB()
        {
            ISynchronizationMongoDb synchronizationMongoDb = new ISynchronizationMongoDb();
            DateTime? synchronizationDateTime = synchronizationMongoDb.LastDateLog();

            if (synchronizationDateTime == null)
            {
                Console.WriteLine("Entra al null");
                ILogs logs = new ILogs();

                List<Logs> listLogs = logs.getAll() ?? new List<Logs>();
                Logs lastLog = logs.LastLog() ?? new Logs();

                if (listLogs.Count == 0) return;

                foreach (Logs log in listLogs)
                {
                    Console.WriteLine(log.MessageTemplate);
                    log.Local = "Local";
                    logs.Insert(log);
                }
                MongoSynchronizationLocal mongoSynchronizationLocal = new MongoSynchronizationLocal();
                mongoSynchronizationLocal.LogTimestamp = lastLog.Timestamp;
                mongoSynchronizationLocal.Datetimelastupdate = DateTime.Now;
                synchronizationMongoDb.Insert(mongoSynchronizationLocal);
                Console.WriteLine("Paso");
            }
            else
            {
                ILogs logs = new ILogs();

                List<Logs> listLogs = logs.getLastLogsDate(synchronizationDateTime) ?? new List<Logs>();
                Logs lastLog = logs.LastLog() ?? new Logs();

                if (lastLog.Timestamp == synchronizationDateTime || listLogs.Count == 0) return;

                foreach (Logs log in listLogs)
                {
                    Console.WriteLine(log.MessageTemplate);
                    log.Local = "Local";
                    logs.Insert(log);
                }
                MongoSynchronizationLocal mongoSynchronizationLocal = new MongoSynchronizationLocal();
                mongoSynchronizationLocal.LogTimestamp = lastLog.Timestamp;
                mongoSynchronizationLocal.Datetimelastupdate = DateTime.Now;
                synchronizationMongoDb.Insert(mongoSynchronizationLocal);
                Console.WriteLine("No paso");
            }

        }
        public void SynchronizationCloudToLocalOffice()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = builder.Build();
            Log.Logger = new LoggerConfiguration().WriteTo.MongoDB(root["DefaultConnection"]!).CreateLogger();

            MyTicketDbContextLocal myTicketDbContextLocal = new MyTicketDbContextLocal();

            ISynchronizationCloud iSynchronizationCloud = new ISynchronizationCloud();
            ITransaction          iTransaction          = new ITransaction();
            IOperations           iOperations           = new IOperations();

            IDbContextTransaction dbContextTransaction = myTicketDbContextLocal.Database.BeginTransaction();

            int lastIdSynchronizationCloud             = iSynchronizationCloud.LastIdSynchronizationCloud();

            if (lastIdSynchronizationCloud == -1)
            {
                try
                {
                    List<Transaction> transactionsCloud = iTransaction.GetAllDataCloudTransaction(officeId) ?? new List<Transaction>();
                    
                    if (transactionsCloud.Count == 0) return;

                    SynchronizationCloud synchronizationCloud = new SynchronizationCloud();

                    synchronizationCloud.Transactioncloudid = iTransaction.LastIdTransactionCloud(officeId);
                    synchronizationCloud.Datetimelastupdate = DateTime.Now;

                    if (synchronizationCloud.Transactioncloudid == -1) return;

                    foreach (Transaction transaction in transactionsCloud)
                    {
                        iOperations.ExecuteQueryLocal(transaction.Mdl, ref myTicketDbContextLocal);
                    }

                    myTicketDbContextLocal.SaveChanges();
                    iSynchronizationCloud.InsertSynchronizationCloud(synchronizationCloud, ref myTicketDbContextLocal);
                    myTicketDbContextLocal.SaveChanges();
                    dbContextTransaction.Commit();
                    Log.Logger.Information("Primera Synchronization Cloud a Local con exito");
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    Log.Error(e, e.Message);
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
            else
            {
                try
                {
                    SynchronizationCloud synchronizationCloud = new SynchronizationCloud();

                    synchronizationCloud.Transactioncloudid = iTransaction.LastIdTransactionCloud(officeId);
                    synchronizationCloud.Datetimelastupdate = DateTime.Now;

                    if (synchronizationCloud.Transactioncloudid == lastIdSynchronizationCloud) { return; };

                    List<Transaction> transactionsCloud = iTransaction.LastIdTransactionsCloud(lastIdSynchronizationCloud, officeId) ?? new List<Transaction>();

                    foreach (Transaction transaction in transactionsCloud)
                    {
                        iOperations.ExecuteQueryLocal(transaction.Mdl, ref myTicketDbContextLocal);
                    }

                    myTicketDbContextLocal.SaveChanges();
                    iSynchronizationCloud.InsertSynchronizationCloud(synchronizationCloud, ref myTicketDbContextLocal);
                    myTicketDbContextLocal.SaveChanges();
                    dbContextTransaction.Commit();
                    Log.Logger.Information("Synchronization Cloud a Local con exito");
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }

        }
        public void SynchronizationLocalToCloudOffice()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = builder.Build();
            Log.Logger = new LoggerConfiguration().WriteTo.MongoDB(root["DefaultConnection"]!).CreateLogger();

            MyTicketDbContextCloud myTicketDbContextCloud = new MyTicketDbContextCloud();

            ISynchronizationLocal iSynchronizationLocal = new ISynchronizationLocal();
            ITransaction iTransaction = new ITransaction();
            IOperations iOperations = new IOperations();


            IDbContextTransaction dbContextTransaction = myTicketDbContextCloud.Database.BeginTransaction();

            int lastIdSynchronizationLocal = iSynchronizationLocal.LastIdSynchronizationLocal();

            if (lastIdSynchronizationLocal == -1)
            {
                try
                {
                    SynchronizationLocal synchronizationLocal = new SynchronizationLocal();
                    List<Transaction> transactionsLocal = iTransaction.GetAllDataLocalTransaction(officeId) ?? new List<Transaction>();

                    synchronizationLocal.Transactionlocalid = iTransaction.LastIdTransactionLocal(officeId);
                    synchronizationLocal.Datetimelastupdate = DateTime.Now;

                    foreach (Transaction transaction in transactionsLocal)
                    {
                        iOperations.ExecuteQueryCloud(transaction.Mdl, ref myTicketDbContextCloud);
                    }
                    myTicketDbContextCloud.SaveChanges();
                    iSynchronizationLocal.InsertSynchronizationLocal(synchronizationLocal);
                    myTicketDbContextCloud.SaveChanges();
                    dbContextTransaction.Commit();
                    Log.Logger.Information("Primera Synchronization Local to Cloud con exito");
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
            else
            {
                try
                {
                    SynchronizationLocal synchronizationLocal = new SynchronizationLocal();

                    synchronizationLocal.Transactionlocalid = iTransaction.LastIdTransactionLocal(officeId);
                    synchronizationLocal.Datetimelastupdate = DateTime.Now;

                    if (synchronizationLocal.Synchronizationlocalid == lastIdSynchronizationLocal) { return; }

                    List<Transaction> transactionsLocal = iTransaction.LastIdTransactionsLocal(lastIdSynchronizationLocal, officeId) ?? new List<Transaction>();

                    foreach (Transaction transaction in transactionsLocal)
                    {
                        iOperations.ExecuteQueryCloud(transaction.Mdl, ref myTicketDbContextCloud);
                    }
                    myTicketDbContextCloud.SaveChanges();
                    iSynchronizationLocal.InsertSynchronizationLocal(synchronizationLocal);
                    myTicketDbContextCloud.SaveChanges();
                    dbContextTransaction.Commit();
                    Log.Logger.Information("Synchronization Local to Cloud con exito");
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }
        public void SynchronizationCloudToLocalCompany()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = builder.Build();
            Log.Logger = new LoggerConfiguration().WriteTo.MongoDB(root["DefaultConnection"]!).CreateLogger();

            MyTicketDbContextLocal myTicketDbContextLocal = new MyTicketDbContextLocal();

            ISynchronizationCloudCompany iSynchronizationCloudCompany = new ISynchronizationCloudCompany();
            ITransactionCompany iTransactionCompany = new ITransactionCompany();
            IOperations iOperations = new IOperations();

            IDbContextTransaction dbContextTransaction = myTicketDbContextLocal.Database.BeginTransaction();

            int lastIdSynchronizationCloudCompany = iSynchronizationCloudCompany.LastIdSynchronizationCloudCompany();

            if (lastIdSynchronizationCloudCompany == -1)
            {
                try
                {
                    List<TransactionCompany> transactionsCloudCompany = iTransactionCompany.GetAllDataCloudTransaction(companyId) ?? new List<TransactionCompany>();

                    if (transactionsCloudCompany.Count == 0) return;

                    SynchronizationCloudCompany synchronizationCloudCompany = new SynchronizationCloudCompany();

                    synchronizationCloudCompany.Transactioncloudid = iTransactionCompany.LastIdTransactionCompanyCloud(companyId);
                    synchronizationCloudCompany.Datetimelastupdate = DateTime.Now;

                    if (synchronizationCloudCompany.Transactioncloudid == -1) return;

                    foreach (TransactionCompany transactionCompany in transactionsCloudCompany)
                    {
                        iOperations.ExecuteQueryLocal(transactionCompany.Mdl, ref myTicketDbContextLocal);
                    }

                    myTicketDbContextLocal.SaveChanges();
                    iSynchronizationCloudCompany.InsertSynchronizationCloud(synchronizationCloudCompany, ref myTicketDbContextLocal);
                    myTicketDbContextLocal.SaveChanges();
                    dbContextTransaction.Commit();
                    Log.Logger.Information("Primera Synchronization Cloud a Local con exito");
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    Log.Error(e, e.Message);
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
            else
            {
                try
                {
                    SynchronizationCloudCompany synchronizationCloudCompany = new SynchronizationCloudCompany();

                    synchronizationCloudCompany.Transactioncloudid = iTransactionCompany.LastIdTransactionCompanyCloud(companyId);
                    synchronizationCloudCompany.Datetimelastupdate = DateTime.Now;

                    if (synchronizationCloudCompany.Transactioncloudid == lastIdSynchronizationCloudCompany) { return; };

                    List<TransactionCompany> transactionsCloudCompany = iTransactionCompany.LastIdTransactionsCloud(lastIdSynchronizationCloudCompany, companyId) ?? new List<TransactionCompany>();

                    foreach (TransactionCompany transactionCompany in transactionsCloudCompany)
                    {
                        iOperations.ExecuteQueryLocal(transactionCompany.Mdl, ref myTicketDbContextLocal);
                    }

                    myTicketDbContextLocal.SaveChanges();
                    iSynchronizationCloudCompany.InsertSynchronizationCloud(synchronizationCloudCompany, ref myTicketDbContextLocal);
                    myTicketDbContextLocal.SaveChanges();
                    dbContextTransaction.Commit();
                    Log.Logger.Information("Synchronization Cloud a Local con exito");
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    Log.Error(e, e.Message);
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }
        public void SynchronizationLocalToCloudCompany()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = builder.Build();
            Log.Logger = new LoggerConfiguration().WriteTo.MongoDB(root["DefaultConnection"]!).CreateLogger();

            MyTicketDbContextCloud myTicketDbContextCloud = new MyTicketDbContextCloud();

            ISynchronizationLocalCompany iSynchronizationLocalCompany = new ISynchronizationLocalCompany();
            ITransactionCompany iTransactionCompany = new ITransactionCompany();
            IOperations iOperations = new IOperations();

            IDbContextTransaction dbContextTransaction = myTicketDbContextCloud.Database.BeginTransaction();

            int lastIdSynchronizationLocalCompany = iSynchronizationLocalCompany.LastIdSynchronizationLocalCompany();

            if (lastIdSynchronizationLocalCompany == -1)
            {
                try
                {
                    List<TransactionCompany> transactionsCloudCompany = iTransactionCompany.GetAllDataLocalTransaction(companyId) ?? new List<TransactionCompany>();

                    if (transactionsCloudCompany.Count == 0) return;

                    SynchronizationLocalCompany synchronizationLocalCompany = new SynchronizationLocalCompany();

                    synchronizationLocalCompany.Transactionlocalid = iTransactionCompany.LastIdTransactionCompanyLocal(companyId);
                    synchronizationLocalCompany.Datetimelastupdate = DateTime.Now;

                    if (synchronizationLocalCompany.Transactionlocalid == -1) return;

                    foreach (TransactionCompany transactionCompany in transactionsCloudCompany)
                    {
                        iOperations.ExecuteQueryCloud(transactionCompany.Mdl, ref myTicketDbContextCloud);
                    }

                    myTicketDbContextCloud.SaveChanges();
                    iSynchronizationLocalCompany.InsertSynchronizationLocal(synchronizationLocalCompany);
                    myTicketDbContextCloud.SaveChanges();
                    dbContextTransaction.Commit();
                    Log.Logger.Information("Primera Synchronization Local a Cloud con exito");
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
            else
            {
                try
                {
                    SynchronizationLocalCompany synchronizationLocalCompany  = new SynchronizationLocalCompany();

                    synchronizationLocalCompany.Transactionlocalid = iTransactionCompany.LastIdTransactionCompanyLocal(companyId);
                    synchronizationLocalCompany.Datetimelastupdate = DateTime.Now;

                    if (synchronizationLocalCompany.Transactionlocalid == lastIdSynchronizationLocalCompany) { return; };

                    List<TransactionCompany> transactionsCloudCompany = iTransactionCompany.LastIdTransactionsLocal(lastIdSynchronizationLocalCompany, companyId) ?? new List<TransactionCompany>();

                    foreach (TransactionCompany transactionCompany in transactionsCloudCompany)
                    {
                        iOperations.ExecuteQueryCloud(transactionCompany.Mdl, ref myTicketDbContextCloud);
                    }

                    myTicketDbContextCloud.SaveChanges();
                    iSynchronizationLocalCompany.InsertSynchronizationLocal(synchronizationLocalCompany);
                    myTicketDbContextCloud.SaveChanges();
                    dbContextTransaction.Commit();
                    Log.Logger.Information("Synchronization Local a Cloud con exito");
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }
        public void SynchronizationClients()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = builder.Build();
            Log.Logger = new LoggerConfiguration().WriteTo.MongoDB(root["DefaultConnection"]!).CreateLogger();

            MyTicketDbContextCloud myTicketDbContextCloud = new MyTicketDbContextCloud();
            MyTicketDbContextLocal myTicketDbContextLocal = new MyTicketDbContextLocal();

            IClients iClients = new IClients();
            IPersons iPersons = new IPersons();

            IDbContextTransaction dbContextTransactionCloud = myTicketDbContextCloud.Database.BeginTransaction();
            IDbContextTransaction dbContextTransactionLocal = myTicketDbContextLocal.Database.BeginTransaction();

            int countClientsCloudCompany = iClients.CountClientsCloud(companyId);
            int countClientsLocalCompany = iClients.CountClientsLocal(companyId);

            if (countClientsCloudCompany == countClientsLocalCompany) return;
            

            if(countClientsCloudCompany > countClientsLocalCompany)
            {
                List<Client> clientsCloud = iClients.GetClientsCloud(companyId) ?? new List<Client>();
                List<Client> clientsLocal = iClients.GetClientsLocal(companyId) ?? new List<Client>();

                try
                {
                    for (int i = 0; i < clientsCloud.Count; i++)
                    {
                        Client clientCloud = clientsCloud[i];
                        bool exists = false;

                        for (int j = i; j < clientsLocal.Count; j++)
                        {
                            Client clientLocal = clientsLocal[j];

                            if (clientCloud.ClientId == clientLocal.ClientId)
                            {
                                exists = true;
                            }
                        }

                        if (exists == false)
                        {
                            Person person = iPersons.GetPersonByIdCloud(clientCloud.PersonId);
                            iPersons.InsertPersonLocal(person, ref myTicketDbContextLocal);
                        }
                    }
                    myTicketDbContextLocal.SaveChanges();
                    dbContextTransactionLocal.Commit();
                    Log.Logger.Information("Synchronization Persons Cloud To Local exitoso");
                }
                catch (Exception e)
                {
                    dbContextTransactionLocal.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
            else
            {
                List<Client> clientsCloud = iClients.GetClientsCloud(companyId) ?? new List<Client>();
                List<Client> clientsLocal = iClients.GetClientsLocal(companyId) ?? new List<Client>();

                try
                {
                    for (int i = 0; i < clientsLocal.Count; i++)
                    {
                        Client clientLocal = clientsLocal[i];
                        bool exists = false;

                        for (int j = i; j < clientsCloud.Count; j++)
                        {
                            Client clientCloud = clientsCloud[j];

                            if (clientCloud.ClientId == clientLocal.ClientId)
                            {
                                exists = true;
                            }
                        }

                        if (exists == false)
                        {
                            Person person = iPersons.GetPersonByIdCloud(clientLocal.PersonId);
                            iPersons.InsertPersonCloud(person, ref myTicketDbContextCloud);
                        }
                    }
                    myTicketDbContextCloud.SaveChanges();
                    dbContextTransactionCloud.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransactionCloud.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }
        public void SynchronizationServices()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = builder.Build();
            Log.Logger = new LoggerConfiguration().WriteTo.MongoDB(root["DefaultConnection"]!).CreateLogger();

            MyTicketDbContextCloud myTicketDbContextCloud = new MyTicketDbContextCloud();
            MyTicketDbContextLocal myTicketDbContextLocal = new MyTicketDbContextLocal();

            IServices iServices = new IServices();

            IDbContextTransaction dbContextTransactionCloud = myTicketDbContextCloud.Database.BeginTransaction();
            IDbContextTransaction dbContextTransactionLocal = myTicketDbContextLocal.Database.BeginTransaction();

            int countServicesCloudCompany = iServices.CountServicesCloud(companyId);
            int countServicesLocalCompany = iServices.CountServicesLocal(companyId);

            if (countServicesCloudCompany == countServicesLocalCompany) return;

            if(countServicesCloudCompany > countServicesLocalCompany)
            {
                List<Service> servicesCloud = iServices.GetServicesByIdCloud(companyTypeId) ?? new List<Service>();
                List<Service> servicesLocal = iServices.GetServicesByIdLocal(companyTypeId) ?? new List<Service>();

                try
                {
                    for (int i = 0; i < servicesCloud.Count; i++)
                    {
                        Service serviceCloud = servicesCloud[i];
                        bool exists = false;

                        for(int j = i; j < servicesLocal.Count; j++)
                        {
                            Service serviceLocal = servicesLocal[j];
                            if(serviceCloud.ServiceId == serviceLocal.ServiceId)
                            {
                                exists = true;
                            }
                        }

                        if(exists == false)
                        {
                            iServices.InsertServiceLocal(serviceCloud, ref myTicketDbContextLocal);
                        }
                    }
                    myTicketDbContextLocal.SaveChanges();
                    dbContextTransactionLocal.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransactionLocal.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            } 
            else
            {
                List<Service> servicesCloud = iServices.GetServicesByIdCloud(companyTypeId) ?? new List<Service>();
                List<Service> servicesLocal = iServices.GetServicesByIdLocal(companyTypeId) ?? new List<Service>();
                try
                {
                    for (int i = 0; i < servicesLocal.Count; i++)
                    {
                        Service serviceLocal = servicesLocal[i];
                        bool exists = false;

                        for (int j = i; j < servicesCloud.Count; j++)
                        {
                            Service serviceCloud = servicesCloud[j];
                            if (serviceCloud.ServiceId == serviceLocal.ServiceId)
                            {
                                exists = true;
                            }
                        }

                        if (exists == false)
                        {
                            iServices.InsertServiceCloud(serviceLocal, ref myTicketDbContextCloud);
                        }
                    }
                    myTicketDbContextCloud.SaveChanges();
                    dbContextTransactionCloud.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransactionCloud.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }

        }
        public void SynchronizationCompanyWallet()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = builder.Build();
            Log.Logger = new LoggerConfiguration().WriteTo.MongoDB(root["DefaultConnection"]!).CreateLogger();

            MyTicketDbContextCloud myTicketDbContextCloud = new MyTicketDbContextCloud();
            MyTicketDbContextLocal myTicketDbContextLocal = new MyTicketDbContextLocal();

            ICompanyWalletTransactions iCompanyWalletTransactions = new ICompanyWalletTransactions();

            IDbContextTransaction dbContextTransactionCloud = myTicketDbContextCloud.Database.BeginTransaction();
            IDbContextTransaction dbContextTransactionLocal = myTicketDbContextLocal.Database.BeginTransaction();

            int countCompanyWalletTrxCloudCompany = iCompanyWalletTransactions.CountCompanyWalletTransactionsCloud(companyWalletId);
            int countCompanyWalletTrxLocalCompany = iCompanyWalletTransactions.CountCompanyWalletTransactionsLocal(companyWalletId);

            if (countCompanyWalletTrxCloudCompany == countCompanyWalletTrxLocalCompany) return;

            if (countCompanyWalletTrxCloudCompany > countCompanyWalletTrxLocalCompany)
            {
                try
                {
                    List<CompanyWalletTransaction> companyWalletTrxsCloud = iCompanyWalletTransactions.GetCompanyWalletTrxByIdCloud(companyWalletId) ?? new List<CompanyWalletTransaction>();
                    List<CompanyWalletTransaction> companyWalletTrxsLocal = iCompanyWalletTransactions.GetCompanyWalletTrxByIdLocal(companyWalletId) ?? new List<CompanyWalletTransaction>();

                    
                    for (int i = 0; i < companyWalletTrxsCloud.Count; i++)
                    {
                        CompanyWalletTransaction companyWalletTrxCloud = companyWalletTrxsCloud[i];
                        bool exists = false;

                        for (int j = i; j < companyWalletTrxsLocal.Count; j++)
                        {
                            CompanyWalletTransaction companyWalletTrxLocal = companyWalletTrxsLocal[j];
                            if (companyWalletTrxCloud.CompanyWalletTransactionId == companyWalletTrxLocal.CompanyWalletTransactionId)
                            {
                                exists = true;
                            }
                        }

                        if (exists == false)
                        {
                            iCompanyWalletTransactions.InsertCompanyWalletTransactionLocal(companyWalletTrxCloud, ref myTicketDbContextLocal);
                        }
                    }
                    myTicketDbContextLocal.SaveChanges();
                    dbContextTransactionLocal.Commit();
                    
                }
                catch (Exception e)
                {
                    dbContextTransactionLocal.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
            else
            {
                try
                {
                    List<CompanyWalletTransaction> companyWalletTrxsCloud = iCompanyWalletTransactions.GetCompanyWalletTrxByIdCloud(companyWalletId) ?? new List<CompanyWalletTransaction>();
                    List<CompanyWalletTransaction> companyWalletTrxsLocal = iCompanyWalletTransactions.GetCompanyWalletTrxByIdLocal(companyWalletId) ?? new List<CompanyWalletTransaction>();


                    for (int i = 0; i < companyWalletTrxsLocal.Count; i++)
                    {
                        CompanyWalletTransaction companyWalletTrxLocal = companyWalletTrxsLocal[i];
                        bool exists = false;

                        for (int j = i; j < companyWalletTrxsCloud.Count; j++)
                        {
                            CompanyWalletTransaction companyWalletTrxCloud = companyWalletTrxsCloud[j];
                            if (companyWalletTrxCloud.CompanyWalletTransactionId == companyWalletTrxLocal.CompanyWalletTransactionId)
                            {
                                exists = true;
                            }
                        }

                        if (exists == false)
                        {
                            iCompanyWalletTransactions.InsertCompanyWalletTransactionCloud(companyWalletTrxLocal, ref myTicketDbContextCloud);
                        }
                    }
                    myTicketDbContextCloud.SaveChanges();
                    dbContextTransactionCloud.Commit();

                }
                catch (Exception e)
                {
                    dbContextTransactionCloud.Rollback();
                    Log.Error(e, e.Message);
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }


        }
    }
}
