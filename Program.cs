//Daniel Pius 
//Assignment 3 Pizza order
//provides logic for GET, POST, PUT, DELETE

using Microsoft.EntityFrameworkCore;
using PizzaOrderApi.Data;
using PizzaOrderApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Swagger Implemetation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//SQlite DB Implementation
builder.Services.AddDbContext<AppDbContext> (option =>
{
    option.UseSqlite("Data Source=pizzaorders.db");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll" , policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Create the DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Pizzas.Any())
    {
        db.Pizzas.AddRange(
            new Pizza { Name = "Pepperoni", Price = 12.99 },
            new Pizza { Name = "Cheese", Price = 10.99 },
            new Pizza { Name = "BBQ Chicken", Price = 14.99 },
            new Pizza { Name = "Veggie", Price = 11.99 }
        );
        db.SaveChanges();
    }
}

//GET, POST, PUT, DELETE methods
//All Pizzas
app.MapGet("/api/pizzas", async (AppDbContext db) =>
{
    return await db.Pizzas.ToListAsync();
});

//Get Pizza by ID
app.MapGet("/api/pizzas/{id}", async (int id, AppDbContext db) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    return pizza is null ? Results.NotFound() : Results.Ok(pizza);
});

//Post New Pizza
app.MapPost("/api/pizzas", async (Pizza pizza, AppDbContext db) =>
{
    db.Pizzas.Add(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/api/pizzas/{pizza.Id}", pizza);
});

//Put Method for Pizza
app.MapPut("/api/pizzas/{id}", async (int id, Pizza updatedPizza, AppDbContext db) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();

    pizza.Name = updatedPizza.Name;
    pizza.Price = updatedPizza.Price;

    await db.SaveChangesAsync();
    return Results.Ok(pizza);
});

//Delete Pizza by ID method
app.MapDelete("/api/pizzas/{id}", async (int id, AppDbContext db) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();

    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok($"Pizza {id} deleted");
});

//All Get, Post, Put, Delete Methods for the Bucket
// GET all order items in bucket
app.MapGet("/api/orders", async (AppDbContext db) =>
{
    return await db.OrderItems.ToListAsync();
});

// POST add item to bucket
app.MapPost("/api/orders", async (OrderItem item, AppDbContext db) =>
{
    // If same pizza already exists in bucket -> increase quantity
    var existing = await db.OrderItems.FirstOrDefaultAsync(o => o.PizzaId == item.PizzaId);

    if (existing != null)
    {
        existing.Quantity += item.Quantity;
        await db.SaveChangesAsync();
        return Results.Ok(existing);
    }

    db.OrderItems.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/api/orders/{item.Id}", item);
});

// PUT modify pizza order quantity by the Id
app.MapPut("/api/orders/{id}", async (int id, OrderItem updatedItem, AppDbContext db) =>
{
    var item = await db.OrderItems.FindAsync(id);
    if (item is null) return Results.NotFound();

    item.Quantity = updatedItem.Quantity;

    await db.SaveChangesAsync();
    return Results.Ok(item);
});

// DELETE order by ID
app.MapDelete("/api/orders/{id}", async (int id, AppDbContext db) =>
{
    var item = await db.OrderItems.FindAsync(id);
    if (item is null) return Results.NotFound();

    db.OrderItems.Remove(item);
    await db.SaveChangesAsync();
    return Results.Ok($"Order item {id} removed");
});

app.Run();
