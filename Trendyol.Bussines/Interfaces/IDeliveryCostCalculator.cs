using System;
using System.Collections.Generic;
using System.Text;

namespace Trendyol.Bussines.Interfaces
{
    public interface IDeliveryCostCalculator
    {
        double CalculateFor(IShoppingCart cart);
    }
}
