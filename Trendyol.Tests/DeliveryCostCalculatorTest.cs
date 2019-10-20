using NUnit.Framework;
using Moq;
using System;
using Trendyol.Business;
using Trendyol.Business.Interfaces;

namespace Trendyol.Tests
{
    public class DeliveryCostCalculatorTest
    {
        DeliveryCostCalculator deliveryCostCalculator;
        Mock<IShoppingCart> cart;

        [SetUp]
        public void Setup()
        {
            cart = new Mock<IShoppingCart>();
        }

        [Test]
        public void CalculateFor_NullCart_ReturnsException()
        {
            deliveryCostCalculator = new DeliveryCostCalculator(5, 10);
            Assert.Throws<ArgumentNullException>(() => deliveryCostCalculator.CalculateFor(null));
        }
        [Test]
        public void CalculateFor_CartWithNoProduct_ReturnsFixedCost()
        {
            deliveryCostCalculator = new DeliveryCostCalculator(5, 10);
            cart.Setup(m => m.GetNumberOfDeliveries()).Returns(0);
            cart.Setup(m => m.GetNumberOfProducts()).Returns(0);

            Assert.That(deliveryCostCalculator.CalculateFor(cart.Object) == 2.99);
        }

        [Test]
        public void CalculateFor_CartWithOneDeliveryOneProduct_ReturnsValidCalculation()
        {
            deliveryCostCalculator = new DeliveryCostCalculator(5, 10);
            cart.Setup(m => m.GetNumberOfDeliveries()).Returns(1);
            cart.Setup(m => m.GetNumberOfProducts()).Returns(1);

            double expected = (5 * 1) + (10 * 1) + 2.99;

            Assert.That(deliveryCostCalculator.CalculateFor(cart.Object) == expected);
        }
    }
}
