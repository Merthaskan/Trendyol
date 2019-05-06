using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Bussines.Enums;
using Trendyol.Bussines.Interfaces;

namespace Trendyol.Bussines
{
    public class Campaign : Discount
    {
        public Category Category { get; set; }

        public Campaign(Category category, int minimumAmount, double discountAmount, DiscountType discountType)
            : base(minimumAmount, discountAmount, discountType)
        {
            Category = category;
        }


    }
}
