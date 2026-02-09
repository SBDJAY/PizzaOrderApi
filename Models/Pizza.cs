//Daniel Pius
//Assignment 3 Pizza order
//Model Class
namespace PizzaOrderApi.Models;

//This is the model class for the Pizza object
//Does not include the ingredietns and descriptive parts of the pizza itself
public class Pizza
{
    public int Id { get; set;}
    public string Name {get; set;} = "";
    public double Price {get; set;}
}