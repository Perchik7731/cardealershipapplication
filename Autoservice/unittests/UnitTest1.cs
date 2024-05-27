using CarLibrary;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;


namespace TestProject1
{
    public class CarTests
    {
        [Test]
        public void GetDiscountedPrice_NoDiscount_ReturnsOriginalPrice()
        {
            var car = new Car
            {
                Price = 10000m,
                HasDiscount = false,
                Discount = 10
            };

            var discountedPrice = car.GetDiscountedPrice();

            NUnit.Framework.Assert.Equals(10000m, discountedPrice);
        }

        [Test]
        public void GetDiscountedPrice_WithDiscount_ReturnsDiscountedPrice()
        {
            var car = new Car
            {
                Price = 10000m,
                HasDiscount = true,
                Discount = 10
            };

            var discountedPrice = car.GetDiscountedPrice();

            NUnit.Framework.Assert.Equals(9000m, discountedPrice);
        }

        [Test]
        public void CalculateDiscount_ValidPrices_ReturnsCorrectDiscount()
        {
            var car = new Car();

            var discount = car.CalculateDiscount(10000m, 9000m);

            NUnit.Framework.Assert.Equals(10m, discount);
        }

        [Test]
        public void ToString_ReturnsBrandAndModel()
        {
            var car = new Car
            {
                Brand = "Toyota",
                Model = "Camry"
            };

            var result = car.ToString();

            NUnit.Framework.Assert.Equals("Toyota Camry", result);
        }
    }

    [TestFixture]
    public class CarConfigurationTests
    {
        [Test]
        public void GetTotalPrice_WithDiscountAndServicesCost_ReturnsCorrectTotal()
        {
            var car = new Car
            {
                Price = 10000m,
                HasDiscount = true,
                Discount = 10
            };
            var configuration = new CarConfiguration
            {
                AdditionalServicesCost = 500m
            };

            var totalPrice = configuration.GetTotalPrice(car);

            NUnit.Framework.Assert.Equals(9500m, totalPrice);
        }
    }

    [TestFixture]
    public class ReceiptTests
    {
        [Test]
        public void GenerateReceipt_ReturnsCorrectFormat()
        {
            var car = new Car
            {
                Brand = "Toyota",
                Model = "Camry",
                Price = 10000m,
                HasDiscount = true,
                Discount = 10
            };
            var configuration = new CarConfiguration
            {
                BodyType = CarBodyType.Sedan,
                Engine = "V6",
                Color = "Black",
                Interior = "Leather",
                AdditionalServicesCost = 500m
            };
            var receipt = new Receipt
            {
                DealershipName = "Best Car Dealership",
                Car = car,
                Configuration = configuration
            };

            var expected = "Диллер: Best Car Dealership\n" +
                           "Машина: Toyota Camry\n" +
                           "Кузов: Sedan\n" +
                           "Двигатель: V6\n" +
                           "Цвет: Black\n" +
                           "Интерьер: Leather\n" +
                           "Дополнительные услуги: $500.00\n" +
                           "Общая цена: $9,500.00";

            NUnit.Framework.Assert.Equals(expected, receipt.GenerateReceipt());
        }
    }

    [TestFixture]
    public class CarBuilderTests
    {
        [Test]
        public void Build_ValidConfiguration_ReturnsCorrectConfiguration()
        {
            var builder = new CarBuilder()
                .SetBodyType(CarBodyType.Sedan)
                .SetEngine("V6")
                .SetColor("Black")
                .SetInterior("Leather")
                .SetAdditionalServicesCost(500m);

            var configuration = builder.Build();

            NUnit.Framework.Assert.Equals(CarBodyType.Sedan, configuration.BodyType);
            NUnit.Framework.Assert.Equals("V6", configuration.Engine);
            NUnit.Framework.Assert.Equals("Black", configuration.Color);
            NUnit.Framework.Assert.Equals("Leather", configuration.Interior);
            NUnit.Framework.Assert.Equals(500m, configuration.AdditionalServicesCost);
        }
    }
}