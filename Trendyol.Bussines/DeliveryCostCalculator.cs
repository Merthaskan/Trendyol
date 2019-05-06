using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Bussines.Interfaces;

namespace Trendyol.Bussines
{
    public class DeliveryCostCalculator : IDeliveryCostCalculator
    {
        public double CostPerDelivery { get; private set; }
        public double CostPerProduct { get; private set; }
        public double FixedCost { get; private set; }
        public DeliveryCostCalculator(double costPerDelivery, double costPerProduct, double fixedCost = 2.99)
        {
            CostPerDelivery = costPerDelivery;
            CostPerProduct = costPerProduct;
            FixedCost = fixedCost;
        }
        public double CalculateFor(IShoppingCart cart)
        {
            int numberOfDeliveries = cart.GetNumberOfDeliveries();
            int numberOfProducts = cart.GetNumberOfProducts();
            return (CostPerDelivery * numberOfDeliveries) + (CostPerProduct * numberOfProducts) + FixedCost;
        }
    }
}
