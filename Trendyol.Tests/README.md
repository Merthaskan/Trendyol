# Tests
Bu proje iş mantığındaki sınıfları test etmek için oluşturulmuştur. NUnit framework kullanılmıştır.

## Test Edilen Sınıflar
* `ShoppingCart`
* `DeliveryCostCalculator`

## Test Açıklamaları

### ShoppingCartTest Sınıfı

`ShoppingCart` sınıfını test etmek için oluşturulmuştur. Moq kütüphanesi yardımıyla `IDelivertCostCalculator` arayüzü mocklanmıştır. Mock sayesinde `ShoppingCart` sınıfının bağımlılığı sanallaştırılmış ve implementasyonundan bağımsız olarak sınıfı test edebilmemize olanak sağlamaktadır.

### DeliveryCostCalculatorTest Sınıfı

`DeliveryCostCalculator` sınıfını test etmek için oluşturulmuştur. Moq kütüphanesi yardımıyla `IShoppingCart` arayüzü mocklanmıştır. Bu mock sayesinde `IShoppingCart` implementasyonlarından bağımsız olarak yapılan hesabın doğrulu kontrol edilmiştir.