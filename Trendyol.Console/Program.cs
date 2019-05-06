using System;
using Trendyol.Bussines;
using Trendyol.Bussines.Enums;
using Trendyol.Bussines.Interfaces;

namespace Trendyol.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Category fruit = new Category("Fruit");
            Category shoe = new Category("Shoe");
            Category menShoe = new Category("Men Shoe", shoe);

            Product apple = new Product("Apple", 100, fruit);
            Product orange = new Product("Orange", 50, fruit);
            Product sportShoe = new Product("Sport Shoe", 250, menShoe);

            IShoppingCart cart = new ShoppingCart(new DeliveryCostCalculator(1.5, 10));
            cart.AddItem(apple, 10);
            cart.AddItem(orange, 5);
            cart.AddItem(sportShoe, 1);

            Campaign c1 = new Campaign(fruit, minimumAmount: 5, discountAmount: 10, DiscountType.Rate);
            Campaign c2 = new Campaign(menShoe, minimumAmount: 150, discountAmount: 30, DiscountType.Rate);
            cart.ApplyDiscounts(c1, c2);

            Coupon coupon = new Coupon(minimumAmount: 1, discountAmount: 10, DiscountType.Amount);
            cart.ApplyCoupon(coupon);

            System.Console.WriteLine(cart.Print());

        }
    }
}
