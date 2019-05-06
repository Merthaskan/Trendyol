using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Business.Enums;

namespace Trendyol.Business
{
    public class Coupon : Discount
    {

        public Coupon(int minimumAmount, double discountAmount, DiscountType discountType)
            : base(minimumAmount, discountAmount, discountType)
        {
        }
    }
}
