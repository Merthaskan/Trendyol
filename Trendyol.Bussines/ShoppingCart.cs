using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trendyol.Business.Enums;
using Trendyol.Business.Interfaces;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Trendyol.Tests")]
namespace Trendyol.Business
{
    public class ShoppingCart : IShoppingCart
    {
        internal Dictionary<Product, int> ProductQuantities { get; set; }
        internal Coupon Coupon { get; set; }
        internal List<Campaign> Campaigns { get; set; }
        private IDeliveryCostCalculator DeliveryCostCalculator { get; set; }
        public ShoppingCart(IDeliveryCostCalculator deliveryCostCalculator)
        {
            ProductQuantities = new Dictionary<Product, int>();
            Campaigns = new List<Campaign>();
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

        #region CampaingDiscountMethods
        public void ApplyDiscounts(params Campaign[] campaigns)
        {
            Campaigns.AddRange(campaigns);
        }
        private double ApplyCampaign(double totalAmount)
        {
            double discountAmount = 0;
            foreach (Campaign campaign in Campaigns)
            {
                Dictionary<Product, int> product = GetProductsByCategory(campaign.Category);
                if (product.Values.Sum() >= campaign.MinimumAmount)
                {
                    switch (campaign.DiscountType)
                    {
                        case DiscountType.Rate:
                            {
                                double da = totalAmount * (campaign.DiscountAmount / 100);
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
        public double GetCampaignDiscount()
        {
            return ApplyCampaign(GetTotalAmount());
        }
        #endregion
        #region CouponDiscountMethods
        public void ApplyCoupon(Coupon coupon)
        {
            Coupon = coupon;
        }
        private double ApplyCoupon(double totalAmount)
        {
            double discountAmount = 0;

            if (Coupon != null && totalAmount >= Coupon.MinimumAmount)
            {
                switch (Coupon.DiscountType)
                {
                    case DiscountType.Rate:
                        discountAmount = totalAmount * (Coupon.DiscountAmount / 100);
                        break;
                    case DiscountType.Amount:
                        discountAmount = Coupon.DiscountAmount;
                        break;
                    default:
                        break;
                }
            }
            return discountAmount;
        }
        public double GetCouponDiscount()
        {
            return ApplyCoupon(GetTotalAmount());
        }
        #endregion

        public double GetDeliveryCost()
        {
            return DeliveryCostCalculator.CalculateFor(this);
        }
        public double GetTotalAmount()
        {
            return ProductQuantities.Sum(e => e.Key.Price * e.Value);
        }
        private double GetProductPrice(Product product)
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
            amount -= ApplyCampaign(amount);
            amount -= ApplyCoupon(amount);
            return amount;
        }

        private bool IsSubCategory(Category parent, Category sub)
        {
            Category temp = sub.ParentCategory;
            while (temp != null)
            {
                if (temp == parent)
                {
                    return true;
                }
                temp = temp.ParentCategory;
            }
            return false;
        }
        private Dictionary<Product, int> GetProductsByCategory(Category category)
        {
            return ProductQuantities.Where(e => e.Key.Category == category || IsSubCategory(category, e.Key.Category)).ToDictionary(e => e.Key, e => e.Value);
        }
        public string Print()
        {
            StringBuilder builder = new StringBuilder();
            var products = ProductQuantities.GroupBy(p => p.Key.Category.Title).ToDictionary(e => e.Key, e => e.ToList());
            builder.AppendLine($"{"Category Name",15}  {"Product Name",15}  {"Quantity",15}  {"Unit Price",15}  {"Total Price",15}");
            foreach (var item in products)
            {
                foreach (var p in item.Value)
                {
                    builder.AppendLine($"{item.Key,15} {p.Key.Title,15} {p.Value,15} {p.Key.Price,15} {GetProductPrice(p.Key),15}\t");
                }
            }
            builder.AppendLine($"\nTotal Amount: {GetTotalAmount()}\nTotal Amount After Discounts: {GetTotalAmountAfterDiscounts()}" +
                $"\nTotal Discount: {GetTotalAmount() - GetTotalAmountAfterDiscounts()}\nDelivery Cost: {GetDeliveryCost()}");
            return builder.ToString();
        }
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
