using Microsoft.EntityFrameworkCore;
using static MyApiApp.Models.Address;

namespace MyApiApp.Models
{/// <summary>
/// DB setup
/// </summary>
    public class MyDb : DbContext    {

        public MyDb() { }
        
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<PersonalAccount> PersonalAccounts { get; set; }

        /// <summary>
        /// Default db tables setup
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Account>()
                .Property(a => a.AvailableBalance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasPrecision(18, 2);


            modelBuilder.Entity<PersonalAccount>()
                .HasOne(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId)
                .IsRequired(true);

            modelBuilder.Entity<PersonalAccount>()
                .HasDiscriminator<string>("AccountType")
                .HasValue<PersonalAccount>("Savings");

            modelBuilder.Entity<PersonalAccount>()
                    .Property(a => a.AvailableBalance)
                    .HasPrecision(18, 2);

                modelBuilder.Entity<PersonalAccount>()
                    .Property(a => a.Balance)
                    .HasPrecision(18, 2);

                modelBuilder.Entity<PersonalAccount>()
                    .Property(p => p.Overdraft)
                    .HasPrecision(18, 2);

                modelBuilder.Entity<Transaction>()
                    .Property(t => t.Amount)
                    .HasPrecision(18, 2);

                modelBuilder.Entity<Transaction>()
                    .HasOne(p => p.Account)
                    .WithMany()
                    .HasForeignKey(p => p.AccountId)
                    .IsRequired(true);


            modelBuilder.Entity<Address>()
                    .Property(a => a.AddressTypeId)
                    .HasConversion<int?>();            


        modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Login = "admin", Email = "admin@example.com", Password = "hashed_password_here", PhoneHash = "phone_hash_here", ActiveAccess = true }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FirstName = "John", Surname = "Doe", DateOfBirth = new DateTime(1980, 1, 1), DocumentId = "AB1234567", Pesel = "85010112345", UserId = 1, CategoryType = "std" }
            );

            modelBuilder.Entity<Address>().HasData(
                new Address { AddressId = 1, Street = "123 Elm St", City = "Metropolis", ZipCode = "A1AC2B", BlgNumber ="11", AddressLine = "123 Elm St Metropolis BlgNo: 11 zipcode:A1AC2B" }
            );

            modelBuilder.Entity<PersonalAccount>().HasData(
                new PersonalAccount { AccountId = 1, AccountNumber = "4550123456", Balance = 1000.00m, AvailableBalance = 1200.00m, Overdraft = 200.00m, BranchId="4550", Ccy="PLN", CustomerId=1, Iban = "PL9218300040000004550123456" }
            );
        }

        public MyDb(DbContextOptions<MyDb> options)
    : base(options)
        {            
        }
    }

}