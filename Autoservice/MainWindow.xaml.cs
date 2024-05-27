using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CarLibrary;
using ButtonLogger;
using System.Data.SQLite;

namespace Autoservice
{
    
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "Data Source=cars.db;Version=3;";
        private LoggerForButton _loggerbutton;
        private List<Car> _cars;
        private List<string> _additionalServices;
        private Car _selectedCar;
        private CarConfiguration _configuration;
        public MainWindow()
        {
            InitializeComponent();
            LoadCarsFromDatabase();
            LoadAdditionalServices();
            BodyTypeComboBox.ItemsSource = Enum.GetValues(typeof(CarBodyType));

            _loggerbutton = new LoggerForButton("button_log.txt");
            InitializePlaceholders();
        }
        private void InitializePlaceholders()
        {
            SetPlaceholder(EngineTextBox, "Двигатель");
            SetPlaceholder(ColorTextBox, "Цвет");
            SetPlaceholder(InteriorTextBox, "Интерьер");
            SetPlaceholder(ServicesCostTextBox, "Стоимость дополнительных услуг");
        }

        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.Foreground = SystemColors.GrayTextBrush;
            }
        }

        private void RemovePlaceholder(TextBox textBox, string placeholder)
        {
            if (textBox.Text == placeholder)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Tag is string placeholder)
            {
                RemovePlaceholder(textBox, placeholder);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Tag is string placeholder)
            {
                SetPlaceholder(textBox, placeholder);
            }
        }

        public void LoadCarsFromDatabase()
        {
            _cars = new List<Car>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Cars";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var car = new Car
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Brand = reader["Brand"].ToString(),
                            Model = reader["Model"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            HasDiscount = Convert.ToBoolean(reader["HasDiscount"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            ManufactureDate = DateTime.Parse(reader["ManufactureDate"].ToString())
                        };
                        _cars.Add(car);
                    }
                }
            }

            CarsListBox.ItemsSource = _cars;
        }

        private void LoadAdditionalServices()
        {
            _additionalServices = new List<string>
            {
                "Расширенная гарантия - $1000",
                "Помощь на дорогах - $500",
                "Тонированные окна - $300",
                "Кожаные сиденья - $2000"
            };

            ServicesListBox.ItemsSource = _additionalServices;
        }

        private void CarsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCar = (Car)CarsListBox.SelectedItem;
            UpdatePriceTextBlock();
        }

        private void BuildConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            _loggerbutton.LogButtonClick();
            if (_selectedCar == null)
            {
                MessageBox.Show("Пожалуйста выберите машину");
                return;
            }

            var builder = new CarBuilder()
                .SetBodyType((CarBodyType)BodyTypeComboBox.SelectedItem)
                .SetEngine(EngineTextBox.Text)
                .SetColor(ColorTextBox.Text)
                .SetInterior(InteriorTextBox.Text)
                .SetAdditionalServicesCost(decimal.Parse(ServicesCostTextBox.Text));

            _configuration = builder.Build();
            UpdatePriceTextBlock();
        }

        private void UpdatePriceTextBlock()
        {
            if (_selectedCar != null && _configuration != null)
            {
                PriceTextBlock.Text = $"Оригинальная цена: {_selectedCar.Price:C}\n" +
                                      $"Цена со скидкой: {_selectedCar.GetDiscountedPrice():C}\n" +
                                      $"Общая цена с услугами: {_configuration.GetTotalPrice(_selectedCar):C}";
            }
        }

        private void GenerateReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            _loggerbutton.LogButtonClick();
            if (_selectedCar == null || _configuration == null)
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль и постройте его конфигурацию.");
                return;
            }

            var receipt = new Receipt
            {
                DealershipName = "Лучший дилерский центр по продаже автомобилей",
                Car = _selectedCar,
                Configuration = _configuration
            };

            MessageBox.Show(receipt.GenerateReceipt());
        }
    }
}