//Daniel Pius
//Assignment 3 Pizza order
//Data Class

using Microsoft.EntityFrameworkCore;
using PizzaOrderApi.Models;

namespace PizzaOrderApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    //lambda function
    public DbSet<Pizza> Pizzas => Set<Pizza>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
}