using System;
using System.Data.Entity;
using System.Data.SqlClient;

using Tour_agency.Model;

namespace Tour_agency
{
    public class AgencyDbContext : DbContext
    {
        
        public AgencyDbContext() : base("DbConnectionString2")
        { }

        public (Constants.VisitorType visitorType, int? id) GetVisitorData(string login, string password)
        {
            var loginParam = new SqlParameter
            {
                ParameterName = "@login",
                Value = login,
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Input,
                Size = 20
            };
            var passwordParam = new SqlParameter
            {
                ParameterName = "@password",
                Value = password,
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Input,
                Size = 20
            };
            var statusParam = new SqlParameter
            {
                ParameterName = "@status",
                SqlDbType = System.Data.SqlDbType.SmallInt,
                Direction = System.Data.ParameterDirection.Output
            };
            var numberParam = new SqlParameter
            {
                ParameterName = "@number",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };


            Database.ExecuteSqlCommand("Exec ПроверкаВхода @login, @password, @status OUTPUT, @number OUTPUT", loginParam, passwordParam, statusParam, numberParam);

            Constants.VisitorType status = (Constants.VisitorType)Enum.ToObject(typeof(Constants.VisitorType), statusParam.Value);
            int? number = null;

            if (!status.Equals(Constants.VisitorType.Error)) {
            number = (int)numberParam.Value;
            }

            return (status, number);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Installment>().HasKey(it => new { it.ClientId, it.TourId });
        }

        

        public DbSet<Tour> Tours { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Selling> Sellings { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<AddingService> AddingServices { get; set; }
    }
}
