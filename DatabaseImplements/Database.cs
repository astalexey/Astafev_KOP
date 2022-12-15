using DatabaseImplements.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseImplements
{
    public class Database : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=KOP_Ast;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Customer> Customers { set; get; }
        public virtual DbSet<GetTechnique> GetTechniquies { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
        public virtual DbSet<Supply> Supplies { set; get; }
        public virtual DbSet<SupplyGetTechnique> SupplyGetTechniques { set; get; }
        public virtual DbSet<SupplyOrder> SupplyOrders { set; get; }
    }
}
