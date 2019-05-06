using System;
using System.Collections.Generic;
using System.Text;

namespace Trendyol.Bussines
{
    public class Product
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }

        public Product(string title,double price, Category category)
        {
            Title = title;
            Price = price;
            Category = category;
        }
    }
}
