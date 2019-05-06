# Console

Bu proje kurulan iş yapısının nasıl kullanılacağına fikir vermek için oluşturulmuştur. İş yapısını test etmek için **yapılmamıştır**. Burda shopping cart kullanılırken `IShoppingCart` arayüzü üzerinden polymorphism ile atanmıştır. Bu sayede az değişiklik yapılarak `ShoppingCart` sınıfı yerine değişiklik ihtiyacı olursa sadece aşağıdaki atama değiştirilmesi yeterli olucaktır.

```
IShoppingCart cart = new ShoppingCart(new DeliveryCostCalculator(1.5, 10));
```