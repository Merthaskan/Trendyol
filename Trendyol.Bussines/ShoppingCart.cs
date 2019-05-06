using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trendyol.Bussines.Enums;
using Trendyol.Bussines.Interfaces;

namespace Trendyol.Bussines
{
    public class ShoppingCart : IShoppingCart
    {
        public Dictionary<Product, int> ProductQuantities { get; set; }
        public List<Coupon> Coupons { get; set; }
        public List<Campaign> Campaigns { get; set; }
        private IDeliveryCostCalculator DeliveryCostCalculator { get; set; }
        public ShoppingCart(IDeliveryCostCalculator deliveryCostCalculator)
        {
            ProductQuantities = new Dictionary<Product, int>();
            Campaigns = new List<Campaign>();
            Coupons = new List<Coupon>();
            DeliveryCostCalculator = deliveryCostCalculator;
        }

        public void AddItem(Product product, int amount)
        {
            if (product != null && amount > 0)
            {

                if (ProductQuantities.TryGetValue(product, out int productAmount))
                {
                    ProductQuantities[product] = productAmount + amount;
                    return;
                }
                ProductQuantities.Add(product, amount);
            }

        }

        public double GetCampaignDiscount()
        {
            return applyCampaign(GetTotalAmount());
        }

        public double GetCouponDiscount()
        {
            return applyCoupon(GetTotalAmount());
        }

        public double GetDeliveryCost()
        {
            return DeliveryCostCalculator.CalculateFor(this);
        }

        public double GetTotalAmount()
        {
            return ProductQuantities.Sum(e => e.Key.Price * e.Value);
        }

        private double GetTotalPrice(Product product)
        {
            if (ProductQuantities.TryGetValue(product, out int quantity))
            {
                return product.Price * quantity;
            }
            throw new KeyNotFoundException(nameof(product));

        }
        public double GetTotalAmountAfterDiscounts()
        {
            double amount = GetTotalAmount();
            amount -= applyCampaign(amount);
            amount -= applyCoupon(amount);
            return amount;
        }

        private Dictionary<Product, int> GetProductsByCategory(string categoryTitle)
        {
            return ProductQuantities.Where(e => e.Key.Category.Title == categoryTitle).ToDictionary(e => e.Key, e => e.Value);
        }

        private double applyCampaign(double totalAmount)
        {
            double discountAmount = 0;
            foreach (Campaign campaign in Campaigns)
            {
                Dictionary<Product, int> product = GetProductsByCategory(campaign.Category.Title);
                if (product.Values.Sum() >= campaign.MinimumAmount)
                {
                    switch (campaign.DiscountType)
                    {
                        case DiscountType.Rate:
                            {
                                double da = totalAmount / campaign.DiscountAmount;
                                if (da > discountAmount)
                                {
                                    discountAmount = da;
                                }
                                break;
                            }
                        case DiscountType.Amount:
                            {
                                if (campaign.DiscountAmount > discountAmount)
                                {
                                    discountAmount = campaign.DiscountAmount;
                                }
                                break;
                            }

                        default:
                            break;
                    }
                }
            }
            return discountAmount;
        }

        private double applyCoupon(double totalAmount)
        {
            double discountAmount = 0;
            foreach (Coupon coupon in Coupons)
            {
                if (totalAmount >= coupon.MinimumAmount)
                {
                    switch (coupon.DiscountType)
                    {
                        case DiscountType.Rate:
                            discountAmount += totalAmount / coupon.DiscountAmount;
                            break;
                        case DiscountType.Amount:
                            discountAmount += coupon.DiscountAmount;
                            break;
                        default:
                            break;
                    }
                }
            }
            return discountAmount;
        }

        public void ApplyDiscounts(params Campaign[] campaigns)
        {
            Campaigns.AddRange(campaigns);
        }

        public string Print()
        {
            StringBuilder builder = new StringBuilder();
            var products = ProductQuantities.GroupBy(p => p.Key.Category.Title).ToDictionary(e => e.Key, e => e.ToList());
            foreach (var item in products)
            {
                foreach (var p in item.Value)
                {
                    builder.Append($"{item.Key} {p.Key.Title} {p.Value} {p.Key.Price} {GetTotalPrice(p.Key)}");
                }
            }
            builder.Append($"Total Amount: {GetTotalAmount()} Delivery Cost: {GetDeliveryCost()}");
            return builder.ToString();
        }

        //private List<string> GetCategories()
        //{
        //    HashSet<string> categorySet = new HashSet<string>();
        //    var products = ProductQuantities.Keys.ToList();
        //    foreach (Product product in products)
        //    {
        //        categorySet.Add(product.Category.Title);
        //    }
        //    return categorySet.ToList();
        //}
        public int GetNumberOfDeliveries()
        {
            return ProductQuantities.GroupBy(e => e.Key.Category.Title).Count();
        }

        public int GetNumberOfProducts()
        {
            return ProductQuantities.Count;
        }

        public bool RemoveItem(Product product, int amount)
        {
            Product removeProduct = ProductQuantities.Keys.FirstOrDefault(p => p.Title == product.Title);
            if (removeProduct != null)
            {
                int addedAmount = ProductQuantities[removeProduct];
                if (amount >= addedAmount)
                {
                    return ProductQuantities.Remove(removeProduct);
                }
                ProductQuantities[removeProduct] = addedAmount - amount;
                return true;
            }
            return false;
        }
    }
}
