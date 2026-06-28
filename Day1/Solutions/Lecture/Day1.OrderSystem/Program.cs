using Day1.OrderSystem.Models;
using Day1.OrderSystem.Services;

var service = new OrderService();

var cart = new Cart();
cart.Add(new CartItem("Laptop Stand", 49.99m, 1));
cart.Add(new CartItem("USB-C Cable",   12.00m, 2));

var customer = new Customer("Alice", CustomerTier.Gold);

var order = service.PlaceOrder(cart, customer);

Console.WriteLine($"Order {order.Id}");
Console.WriteLine($"  Customer : {order.Customer.Name} ({order.Customer.Tier})");
Console.WriteLine($"  Total    : {order.Total:C}");
Console.WriteLine($"  Discount : {order.Discount:C}");
Console.WriteLine($"  Final    : {order.FinalAmount:C}");
Console.WriteLine($"  Status   : {order.Status}");
