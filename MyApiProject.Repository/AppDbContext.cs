using Microsoft.EntityFrameworkCore;
using MyApiProject.Core.Entities;
using MyApiProject.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyApiProject.Repository;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> products { get; set; }
    public DbSet<ProductBrand> ProductBrands { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders{ get; set; }
    public DbSet<OrderItem> OrderItems{ get; set; }
    public DbSet<DeliveryMethods> DeliveryMethods{ get; set; }
}