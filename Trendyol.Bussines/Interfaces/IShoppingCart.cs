﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Trendyol.Bussines.Interfaces
{
    public interface IShoppingCart
    {
        double GetTotalAmount();
        double GetTotalAmountAfterDiscounts();
        double GetCouponDiscount();
        double GetCampaignDiscount();
        double GetDeliveryCost();
        void AddItem(Product product, int amount);
        bool RemoveItem(Product product, int amount);
        void ApplyDiscounts(params Campaign[] campaigns);
        int GetNumberOfDeliveries();
        int GetNumberOfProducts();
        string Print();
    }
}
