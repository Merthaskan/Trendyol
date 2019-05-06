using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Bussines.Enums;

namespace Trendyol.Bussines
{
    public abstract class Discount
    {
        public double DiscountAmount { get; private set; }
        public DiscountType DiscountType { get; private set; }
        public int MinimumAmount { get; private set; }

        public Discount(int minimumAmount, double discountAmount, DiscountType discountType)
        {
            DiscountAmount = discountAmount;
            DiscountType = discountType;
            MinimumAmount = minimumAmount;
        }

    }
}
