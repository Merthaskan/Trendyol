using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Bussines.Enums;

namespace Trendyol.Bussines
{
    public class Coupon : Discount
    {

        public Coupon(int minimumAmount, double discountAmount, DiscountType discountType)
            : base(minimumAmount, discountAmount, discountType)
        {
        }
    }
}
