using Moq;
using NUnit.Framework;
using Trendyol.Bussines;
using Trendyol.Bussines.Enums;
using Trendyol.Bussines.Interfaces;

namespace Tests
{
    public class ShoppingCartTest
    {
        Mock<IDeliveryCostCalculator> calculator;
        ShoppingCart cart;
        [SetUp]
        public void Setup()
        {
            calculator = new Mock<IDeliveryCostCalculator>();
            cart = new ShoppingCart(calculator.Object);
        }

        #region AddProduct
        [Test]
        public void AddProduct_ValidProductValidQuantity_ProductShouldAdd()
        {
            Product product = new Product("Apple", 100.0, new Category("Fruit"));

            cart.AddItem(product, 5);

            Assert.AreEqual(1, cart.ProductQuantities.Count);
            Assert.IsTrue(cart.ProductQuantities.ContainsKey(product));
            Assert.That(cart.ProductQuantities[product] == 5);
        }
        [Test]
        public void AddProduct_AddSameProductInstanceWithDifferenQuantity_ProductShouldAdd()
        {
            Product product = new Product("Apple", 100.0, new Category("Fruit"));

            cart.AddItem(product, 3);
            cart.AddItem(product, 2);

            Assert.AreEqual(1, cart.ProductQuantities.Count);
            Assert.IsTrue(cart.ProductQuantities.ContainsKey(product));
            Assert.That(cart.ProductQuantities[product] == 5);
        }

        [Test]
        public void AddProduct_AddSameProductDifferentInstanceWithDifferenQuantity_ProductShouldAdd()
        {
            Product p1 = new Product("Apple", 100.0, new Category("Fruit"));
            Product p2 = new Product("Apple", 100.0, new Category("Fruit"));

            cart.AddItem(p1, 3);
            cart.AddItem(p2, 2);

            Assert.AreEqual(2, cart.ProductQuantities.Count);
            Assert.IsTrue(cart.ProductQuantities.ContainsKey(p1));
            Assert.IsTrue(cart.ProductQuantities.ContainsKey(p2));
            Assert.That(cart.ProductQuantities[p1] == 3);
            Assert.That(cart.ProductQuantities[p2] == 2);
        }

        [Test]
        public void AddProduct_ProductIsNull_ProductShouldNotAdd()
        {
            Product product = null;

            cart.AddItem(product, 5);

            Assert.AreEqual(0, cart.ProductQuantities.Count);
        }

        [Test]
        public void AddProduct_ProductQuantityIsZero_ProductShouldNotAdd()
        {
            Product product = new Product("Apple", 100.0, new Category("Fruit"));

            cart.AddItem(product, 0);

            Assert.AreEqual(0, cart.ProductQuantities.Count);
            Assert.IsFalse(cart.ProductQuantities.ContainsKey(product));
        }

        [Test]
        public void AddProduct_ProductQuantityIsNegative_ProductShouldNotAdd()
        {
            Product product = new Product("Apple", 100.0, new Category("Fruit"));

            cart.AddItem(product, -1);

            Assert.AreEqual(0, cart.ProductQuantities.Count);
            Assert.IsFalse(cart.ProductQuantities.ContainsKey(product));
        }
        #endregion
        #region GetNumberOfProducts
        [Test]
        public void GetNumberOfProduct_EmptyList_ReturnsZero()
        {
            Assert.AreEqual(0, cart.GetNumberOfProducts());
        }
        [Test]
        public void GetNumberOfProduct_AddValidProduct_ReturnsOne()
        {
            Product p1 = new Product("Apple", 100, new Category("Fruit"));
            cart.AddItem(p1, 5);
            Assert.AreEqual(1, cart.GetNumberOfProducts());
        }
        [Test]
        public void GetNumberOfProduct_AddValidTwoProduct_ReturnsTwo()
        {
            Product p1 = new Product("Apple", 100, new Category("Fruit"));
            Product p2 = new Product("Orange", 100, new Category("Fruit"));

            cart.AddItem(p1, 5);
            cart.AddItem(p2, 5);

            Assert.AreEqual(2, cart.GetNumberOfProducts());
        }
        #endregion
        #region GetDeliveryCost
        [Test]
        public void GetDeliveryCost_TestWithMock_ReturnsMockValue()
        {
            calculator.Setup(m => m.CalculateFor(cart)).Returns(5);
            Assert.That(cart.GetDeliveryCost() == 5);
        }
        #endregion
        #region GetNumberOfDeliveries
        [Test]
        public void GetNumberOfDeliveries_EmptyCart_ReturnsZero() => Assert.That(cart.GetNumberOfDeliveries() == 0);

        [Test]
        public void GetNumberOfDeliveries_OneProduct_ReturnsOne()
        {
            Product p1 = new Product("Apple", 100, new Category("Fruit"));
            cart.AddItem(p1, 5);
            Assert.That(cart.GetNumberOfDeliveries() == 1);
        }

        [Test]
        public void GetNumberOfDeliveries_TwoDifferentCategoryProduct_ReturnsTwo()
        {
            Product p1 = new Product("Apple", 100, new Category("Fruit"));
            Product p2 = new Product("Leek", 20, new Category("Vegetable"));

            cart.AddItem(p1, 5);
            cart.AddItem(p2, 3);
            Assert.That(cart.GetNumberOfDeliveries() == 2);
        }

        [Test]
        public void GetNumberOfDeliveries_TwoSameCategoryProduct_ReturnsOne()
        {
            Category category = new Category("Fruit");
            Product p1 = new Product("Apple", 100, category);
            Product p2 = new Product("Orange", 20, category);

            cart.AddItem(p1, 5);
            cart.AddItem(p2, 3);
            Assert.That(cart.GetNumberOfDeliveries() == 1);
        }
        #endregion
        #region RemoveItem
        [Test]
        public void RemoveItem_EmptyList_ReturnsFalse()
        {
            Product p1 = new Product("Apple", 100, new Category("Fruit"));
            Assert.That(!cart.RemoveItem(p1, 5));
        }
        [Test]
        public void RemoveItem_ProductAllQuantity_ReturnsTrue()
        {
            Product p1 = new Product("Apple", 100, new Category("Fruit"));
            cart.AddItem(p1, 5);
            Assert.That(cart.RemoveItem(p1, 5));
            Assert.That(cart.GetNumberOfProducts() == 0);
        }
        [Test]
        public void RemoveItem_ProductOneQuantity_ReturnsTrue()
        {
            Product p1 = new Product("Apple", 100, new Category("Fruit"));
            cart.AddItem(p1, 5);
            Assert.That(cart.RemoveItem(p1, 1));
            Assert.That(cart.GetNumberOfProducts() == 1);
            Assert.That(cart.ProductQuantities[p1] == 4);
        }
        [Test]
        public void RemoveItem_ProductMoreQuantityThanProductQuantity_ReturnsTrue()
        {
            Product p1 = new Product("Apple", 100, new Category("Fruit"));
            cart.AddItem(p1, 5);
            Assert.That(cart.RemoveItem(p1, 6));
            Assert.That(cart.GetNumberOfProducts() == 0);
        }
        [Test]
        public void RemoveItem_AddTwoProductRemoveOneProduct_ReturnsTrue()
        {
            Product p1 = new Product("Apple", 100, new Category("Fruit"));
            Product p2 = new Product("Apple", 100, new Category("Fruit"));

            cart.AddItem(p1, 5);
            cart.AddItem(p2, 6);
            Assert.That(cart.RemoveItem(p1, 5));
            Assert.That(cart.GetNumberOfProducts() == 1);
        }
        #endregion
        #region GetTotalAmount
        [Test]
        public void GetTotalAmount_EmptyList_ReturnsZero()
        {
            Assert.That(cart.GetTotalAmount() == 0);
        }
        [Test]
        public void GetTotalAmount_OneProduct_ReturnsFive()
        {
            Product p1 = new Product("Apple", 1, new Category("Fruit"));
            cart.AddItem(p1, 5);
            Assert.That(cart.GetTotalAmount() == 5);
        }
        [Test]
        public void GetTotalAmount_TwoProduct_ReturnsSeven()
        {
            Category category = new Category("Fruit");
            Product p1 = new Product("Apple", 1, category);
            Product p2 = new Product("Orange", 2, category);

            cart.AddItem(p1, 5);
            cart.AddItem(p2, 1);
            Assert.That(cart.GetTotalAmount() == 7);
        }
        #endregion
        #region GetCampaignDiscount
        [Test]
        public void GetCampaignDiscount_NoCampaing_ReturnsZero()
        {
            Assert.That(cart.GetCampaignDiscount() == 0);
        }
        [Test]
        public void GetCampaignDiscount_OneCampaingOneProductOnCategoryGreaterThanMinimumAmount_ReturnsFive()
        {
            Category category = new Category("Fruit");
            Product p1 = new Product("Apple", 100, category);
            cart.AddItem(p1, 3);

            Campaign campaign = new Campaign(category, minimumAmount: 2, discountAmount: 5, DiscountType.Amount);
            cart.ApplyDiscounts(campaign);
            Assert.That(cart.GetCampaignDiscount() == 5);
        }
        [Test]
        public void GetCampaignDiscount_OneCampaingOneProductOnCategoryLessThanMinimumAmount_ReturnsZero()
        {
            Category category = new Category("Fruit");
            Product p1 = new Product("Apple", 100, category);
            cart.AddItem(p1, 3);

            Campaign campaign = new Campaign(category, minimumAmount: 5, discountAmount: 5, DiscountType.Amount);
            cart.ApplyDiscounts(campaign);
            Assert.That(cart.GetCampaignDiscount() == 0);
        }
        [Test]
        public void GetCampaignDiscount_OneCampaingTwoProductOnCategoryGreaterThanMinimumAmount_ReturnsTen()
        {
            Category category = new Category("Fruit");
            Product p1 = new Product("Apple", 100, category);
            Product p2 = new Product("Orange", 50, category);

            cart.AddItem(p1, 3);
            cart.AddItem(p2, 2);


            Campaign campaign = new Campaign(category, minimumAmount: 5, discountAmount: 10, DiscountType.Amount);
            cart.ApplyDiscounts(campaign);

            Assert.That(cart.GetCampaignDiscount() == 10);
        }
        [Test]
        public void GetCampaignDiscount_OneCampaingTwoProductOnCategoryLessThanMinimumAmount_ReturnsZero()
        {
            Category category = new Category("Fruit");
            Product p1 = new Product("Apple", 100, category);
            Product p2 = new Product("Orange", 50, category);

            cart.AddItem(p1, 3);
            cart.AddItem(p2, 2);


            Campaign campaign = new Campaign(category, minimumAmount: 6, discountAmount: 10, DiscountType.Amount);
            cart.ApplyDiscounts(campaign);

            Assert.That(cart.GetCampaignDiscount() == 0);
        }

        [Test]
        public void GetCampaignDiscount_OneCampaingWithRateOneProductOnCategoryGreaterThanMinimumAmount_ReturnsFifteen()
        {
            Category category = new Category("Fruit");
            Product p1 = new Product("Apple", 100, category);
            cart.AddItem(p1, 3);

            double expected = cart.GetTotalAmount() * 0.05;


            Campaign campaign = new Campaign(category, minimumAmount: 2, discountAmount: 5, DiscountType.Rate);
            cart.ApplyDiscounts(campaign);
            Assert.That(cart.GetCampaignDiscount() == expected);
        }
        [Test]
        public void GetCampaignDiscount_OneCampaingWithRateOneProductOnCategoryLessThanMinimumAmount_ReturnsZero()
        {
            Category category = new Category("Fruit");
            Product p1 = new Product("Apple", 100, category);
            cart.AddItem(p1, 3);

            Campaign campaign = new Campaign(category, minimumAmount: 5, discountAmount: 5, DiscountType.Rate);
            cart.ApplyDiscounts(campaign);
            Assert.That(cart.GetCampaignDiscount() == 0);
        }
        [Test]
        public void GetCampaignDiscount_OneCampaingWithTwoProductOnCategoryGreaterThanMinimumAmount_ReturnsForty()
        {
            Category category = new Category("Fruit");
            Product p1 = new Product("Apple", 100, category);
            Product p2 = new Product("Orange", 50, category);

            cart.AddItem(p1, 3);
            cart.AddItem(p2, 2);

            double expected = cart.GetTotalAmount() * 0.1;

            Campaign campaign = new Campaign(category, minimumAmount: 5, discountAmount: 10, DiscountType.Rate);
            cart.ApplyDiscounts(campaign);

            Assert.That(cart.GetCampaignDiscount() == expected);
        }
        public void GetCampaignDiscount_OneCampaingWithRateTwoProductOnCategoryLessThanMinimumAmount_ReturnsZero()
        {
            Category category = new Category("Fruit");
            Product p1 = new Product("Apple", 100, category);
            Product p2 = new Product("Orange", 50, category);

            cart.AddItem(p1, 3);
            cart.AddItem(p2, 2);


            Campaign campaign = new Campaign(category, minimumAmount: 6, discountAmount: 10, DiscountType.Amount);
            cart.ApplyDiscounts(campaign);

            Assert.That(cart.GetCampaignDiscount() == 0);
        }
        #endregion


    }
}