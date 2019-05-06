using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Business.Enums;
using Trendyol.Business.Interfaces;

namespace Trendyol.Business
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
