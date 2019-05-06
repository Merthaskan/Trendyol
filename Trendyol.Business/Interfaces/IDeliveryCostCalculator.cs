using System;
using System.Collections.Generic;
using System.Text;

namespace Trendyol.Business.Interfaces
{
    public interface IDeliveryCostCalculator
    {
        double CalculateFor(IShoppingCart cart);
    }
}
