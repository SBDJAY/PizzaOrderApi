//Daniel Pius
//Assignment 3 Pizza order
//Model Class

namespace PizzaOrderApi.Models;

public class OrderItem
{
    public int Id {get; set;}
    public int PizzaId {get; set;}
    public string PizzaName {get; set;}= "";
    public double PizzaPrice {get; set;}
    public int Quantity {get; set;}
}