using Microsoft.EntityFrameworkCore;
using SystemTransaction.EntityPostgre;

namespace SystemTransaction.Dal.DBContext
{
    public partial class MyTicketDbContextLocal : DbContext
    {
        public MyTicketDbContextLocal()
        {
        }

        public MyTicketDbContextLocal(DbContextOptions<MyTicketDbContextLocal> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessToken> AccessTokens { get; set; }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<CompanyType> CompanyTypes { get; set; }

        public virtual DbSet<CompanyWallet> CompanyWallets { get; set; }

        public virtual DbSet<CompanyWalletTransaction> CompanyWalletTransactions { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Especiality> Especialities { get; set; }

        public virtual DbSet<Job> Jobs { get; set; }

        public virtual DbSet<Offer> Offers { get; set; }

        public virtual DbSet<Office> Offices { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<Position> Positions { get; set; }

        public virtual DbSet<Service> Services { get; set; }

        public virtual DbSet<SynchronizationCloud> SynchronizationClouds { get; set; }
        public virtual DbSet<SynchronizationCloudCompany> SynchronizationCloudCompanies { get; set; }

        public virtual DbSet<SynchronizationLocal> SynchronizationLocals { get; set; }
        public virtual DbSet<SynchronizationLocalCompany> SynchronizationLocalCompanies { get; set; }

        public virtual DbSet<Time> Times { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionCompany> TransactionCompanies { get; set; }

        public virtual DbSet<WorkDay> WorkDays { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Server=192.168.1.110;Port=5432;User Id=be-ticket;Password=Passw0rd*;Database=MyTicketDb;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessToken>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AccessTokens_UserId");

                entity.HasOne(d => d.User).WithMany(p => p.AccessTokens).HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.CompanyId, "IX_Clients_CompanyId");

                entity.HasIndex(e => e.PersonId, "IX_Clients_PersonId");

                entity.HasOne(d => d.Company).WithMany(p => p.Clients).HasForeignKey(d => d.CompanyId);

                entity.HasOne(d => d.Person).WithMany(p => p.Clients).HasForeignKey(d => d.PersonId);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasOne(d => d.CompanyType).WithMany(p => p.Companies)
                    .HasForeignKey(d => d.CompanyTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Companies_CompanyTypes");
            });

            modelBuilder.Entity<CompanyWallet>(entity =>
            {
                entity.HasIndex(e => e.CompanyId, "IX_CompanyWallets_CompanyId");

                entity.HasOne(d => d.Company).WithMany(p => p.CompanyWallets).HasForeignKey(d => d.CompanyId);
            });

            modelBuilder.Entity<CompanyWalletTransaction>(entity =>
            {
                entity.HasIndex(e => e.CompanyWalletId, "IX_CompanyWalletTransactions_CompanyWalletId");

                entity.HasOne(d => d.CompanyWallet).WithMany(p => p.CompanyWalletTransactions).HasForeignKey(d => d.CompanyWalletId);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.CompanyId, "IX_Employees_CompanyId");

                entity.HasIndex(e => e.PersonId, "IX_Employees_PersonId");

                entity.HasOne(d => d.Company).WithMany(p => p.Employees).HasForeignKey(d => d.CompanyId);

                entity.HasOne(d => d.Office).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Person).WithMany(p => p.Employees).HasForeignKey(d => d.PersonId);
            });

            modelBuilder.Entity<Especiality>(entity =>
            {
                entity.HasIndex(e => e.CompanyId, "IX_Especialities_CompanyId");

                entity.HasOne(d => d.Company).WithMany(p => p.Especialities).HasForeignKey(d => d.CompanyId);
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasIndex(e => e.EspecialityId, "IX_Jobs_EspecialityId");

                entity.HasIndex(e => e.ServiceId, "IX_Jobs_ServiceId");

                entity.HasOne(d => d.Especiality).WithMany(p => p.Jobs).HasForeignKey(d => d.EspecialityId);

                entity.HasOne(d => d.Service).WithMany(p => p.Jobs).HasForeignKey(d => d.ServiceId);
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.HasIndex(e => e.EmployeeId, "IX_Offers_EmployeeId");

                entity.HasIndex(e => e.OfficeId, "IX_Offers_OfficeId");

                entity.HasIndex(e => e.ServiceId, "IX_Offers_ServiceId");

                entity.HasOne(d => d.Client).WithMany(p => p.Offers)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Offers_Clients");

                entity.HasOne(d => d.Employee).WithMany(p => p.Offers).HasForeignKey(d => d.EmployeeId);

                entity.HasOne(d => d.Office).WithMany(p => p.Offers).HasForeignKey(d => d.OfficeId);

                entity.HasOne(d => d.Service).WithMany(p => p.Offers).HasForeignKey(d => d.ServiceId);

                entity.HasOne(d => d.Time).WithMany(p => p.Offers)
                    .HasForeignKey(d => d.TimeId)
                    .HasConstraintName("FK_Offers_Times");

                entity.HasOne(d => d.WorkDay).WithMany(p => p.Offers)
                    .HasForeignKey(d => d.WorkDayId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Offers_Workdays");
            });

            modelBuilder.Entity<Office>(entity =>
            {
                entity.HasIndex(e => e.CompanyId, "IX_Offices_CompanyId");

                entity.HasOne(d => d.Company).WithMany(p => p.Offices).HasForeignKey(d => d.CompanyId);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasIndex(e => e.Email, "Constraint_Email").IsUnique();

                entity.HasIndex(e => e.Mobile, "Constraint_Mobile").IsUnique();

                entity.Property(e => e.AuthMethod).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasIndex(e => e.EmployeeId, "IX_Positions_EmployeeId");

                entity.HasIndex(e => e.EspecialityId, "IX_Positions_EspecialityId");

                entity.HasOne(d => d.Employee).WithMany(p => p.Positions).HasForeignKey(d => d.EmployeeId);

                entity.HasOne(d => d.Especiality).WithMany(p => p.Positions).HasForeignKey(d => d.EspecialityId);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasOne(d => d.CompanyType).WithMany(p => p.Services)
                    .HasForeignKey(d => d.CompanyTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Services_CompanyTypes");
            });

            modelBuilder.Entity<SynchronizationCloud>(entity =>
            {
                entity.HasKey(e => e.Synchronizationcloudid).HasName("SynchronizationCloud_pkey");

                entity.ToTable("SynchronizationCloud");

                entity.Property(e => e.Synchronizationcloudid)
                    .UseIdentityAlwaysColumn()
                    .HasColumnName("synchronizationcloudid");
                entity.Property(e => e.Datetimelastupdate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datetimelastupdate");
                entity.Property(e => e.Transactioncloudid).HasColumnName("transactioncloudid");
            });

            modelBuilder.Entity<SynchronizationCloudCompany>(entity =>
            {
                entity.HasKey(e => e.Synchronizationcloudcompanyid).HasName("SynchronizationCloudCompany_pkey");

                entity.ToTable("SynchronizationCloudCompany");

                entity.Property(e => e.Synchronizationcloudcompanyid)
                    .UseIdentityAlwaysColumn()
                    .HasColumnName("synchronizationcloudcompanyid");
                entity.Property(e => e.Datetimelastupdate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datetimelastupdate");
                entity.Property(e => e.Transactioncloudid).HasColumnName("transactioncloudid");
            });

            modelBuilder.Entity<SynchronizationLocal>(entity =>
            {
                entity.HasKey(e => e.Synchronizationlocalid).HasName("SynchronizationLocal_pkey");

                entity.ToTable("SynchronizationLocal");

                entity.Property(e => e.Synchronizationlocalid)
                    .UseIdentityAlwaysColumn()
                    .HasColumnName("synchronizationlocalid");
                entity.Property(e => e.Datetimelastupdate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datetimelastupdate");
                entity.Property(e => e.Transactionlocalid).HasColumnName("transactionlocalid");
            });

            modelBuilder.Entity<SynchronizationLocalCompany>(entity =>
            {
                entity.HasKey(e => e.Synchronizationlocalcompanyid).HasName("SynchronizationLocalCompany_pkey");

                entity.ToTable("SynchronizationLocalCompany");

                entity.Property(e => e.Synchronizationlocalcompanyid)
                    .UseIdentityAlwaysColumn()
                    .HasColumnName("synchronizationlocalcompanyid");
                entity.Property(e => e.Datetimelastupdate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datetimelastupdate");
                entity.Property(e => e.Transactionlocalid).HasColumnName("transactionlocalid");
            });

            modelBuilder.Entity<Time>(entity =>
            {
                entity.HasOne(d => d.Office).WithMany(p => p.Times)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Times_Offices");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Transactionid).HasName("Transaction_pkey");

                entity.ToTable("Transaction");

                entity.Property(e => e.Transactionid)
                    .UseIdentityAlwaysColumn()
                    .HasColumnName("transactionid");
                entity.Property(e => e.Datetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datetime");
                entity.Property(e => e.Mdl)
                    .HasColumnType("character varying")
                    .HasColumnName("mdl");
                entity.Property(e => e.Officeid).HasColumnName("officeid");
                entity.Property(e => e.Operation)
                    .HasMaxLength(3)
                    .IsFixedLength()
                    .HasColumnName("operation");
                entity.Property(e => e.Tablename)
                    .HasMaxLength(50)
                    .HasColumnName("tablename");
            });

            modelBuilder.Entity<TransactionCompany>(entity =>
            {
                entity.HasKey(e => e.Transactioncompanyid).HasName("TransactionCompany_pkey");

                entity.ToTable("TransactionCompany");

                entity.Property(e => e.Transactioncompanyid)
                    .UseIdentityAlwaysColumn()
                    .HasColumnName("transactioncompanyid");
                entity.Property(e => e.Companyid).HasColumnName("companyid");
                entity.Property(e => e.Datetime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datetime");
                entity.Property(e => e.Mdl)
                    .HasColumnType("character varying")
                    .HasColumnName("mdl");
                entity.Property(e => e.Operation)
                    .HasMaxLength(3)
                    .IsFixedLength()
                    .HasColumnName("operation");
                entity.Property(e => e.Tablename)
                    .HasMaxLength(100)
                    .HasColumnName("tablename");
            });

            modelBuilder.Entity<WorkDay>(entity =>
            {
                entity.HasOne(d => d.Office).WithMany(p => p.WorkDays)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_WorkDays_Offices");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
