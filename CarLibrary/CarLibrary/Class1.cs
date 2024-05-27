using System;

namespace CarLibrary
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public bool HasDiscount { get; set; }
        public decimal Discount { get; set; }
        public DateTime ManufactureDate { get; set; }

        public decimal GetDiscountedPrice()
        {
            return HasDiscount ? Price - (Price * Discount / 100) : Price;
        }

        public decimal CalculateDiscount(decimal originalPrice, decimal finalPrice)
        {
            return 100 * (originalPrice - finalPrice) / originalPrice;
        }

        
        public override string ToString()
        {
            return $"{Brand} {Model}";
        }
    }

    public enum CarBodyType
    {
        Phaeton,
        Limousine,
        Cabriolet,
        Sedan,
        Roadster,
        Coupe,
        Pickup,
        StationWagon,
        Hatchback
    }

    public class CarConfiguration
    {
        public CarBodyType BodyType { get; set; }
        public string Engine { get; set; }
        public string Color { get; set; }
        public string Interior { get; set; }
        public decimal AdditionalServicesCost { get; set; }

        public decimal GetTotalPrice(Car car)
        {
            return car.GetDiscountedPrice() + AdditionalServicesCost;
        }
    }

    public class Receipt
    {
        public string DealershipName { get; set; }
        public Car Car { get; set; }
        public CarConfiguration Configuration { get; set; }

        public string GenerateReceipt()
        {
            return $"Диллер: {DealershipName}\n" +
                   $"Машина: {Car.Brand} {Car.Model}\n" +
                   $"Кузов: {Configuration.BodyType}\n" +
                   $"Двигатель: {Configuration.Engine}\n" +
                   $"Цвет: {Configuration.Color}\n" +
                   $"Интерьер: {Configuration.Interior}\n" +
                   $"Дополнительные услуги: {Configuration.AdditionalServicesCost:C}\n" +
                   $"Общая цена: {Configuration.GetTotalPrice(Car):C}";
        }
    }

    public class CarBuilder
    {
        private CarConfiguration _configuration = new CarConfiguration();

        public CarBuilder SetBodyType(CarBodyType bodyType)
        {
            _configuration.BodyType = bodyType;
            return this;
        }

        public CarBuilder SetEngine(string engine)
        {
            _configuration.Engine = engine;
            return this;
        }

        public CarBuilder SetColor(string color)
        {
            _configuration.Color = color;
            return this;
        }

        public CarBuilder SetInterior(string interior)
        {
            _configuration.Interior = interior;
            return this;
        }

        public CarBuilder SetAdditionalServicesCost(decimal cost)
        {
            _configuration.AdditionalServicesCost = cost;
            return this;
        }

        public CarConfiguration Build()
        {
            return _configuration;
        }
    }
}