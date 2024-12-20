using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using iText.Layout.Properties;
using Document = iTextSharp.text.Document;
using PdfWriter = iTextSharp.text.pdf.PdfWriter;
using Paragraph = iTextSharp.text.Paragraph;
using System.Drawing;
using Font = iTextSharp.text.Font;
using Brushes = System.Windows.Media.Brushes;
namespace HotelGalaxy
{
    public partial class Reservation : Window
    {
        public Reservation()
        {
            InitializeComponent();
            DataContext = this;
            this.Loaded += Window_Loaded;
            tiproom.SelectionChanged += Tiproom_SelectionChanged;
            // Подписка на события Checked и Unchecked для пересчета цены
            zav.Checked += CheckBox_Changed;
            zav.Unchecked += CheckBox_Changed;
            tak.Checked += CheckBox_Changed;
            tak.Unchecked += CheckBox_Changed;
            ybor.Checked += CheckBox_Changed;
            ybor.Unchecked += CheckBox_Changed;
        }
        // Словарь для хранения цен на дополнительные услуги
        private Dictionary<string, decimal> servicePrices = new Dictionary<string, decimal>()
        {
            {"Завтрак", 247.00m},
            {"Такси", 100.00m},
            {"Уборка номера", 146.00m},
        };
        // Словарь для хранения цен на типы номеров
        private Dictionary<string, decimal> roomPrices = new Dictionary<string, decimal>()
        {
            { "Люкс", 8000m },
            { "Одноместный", 3000m },
            { "Двухместный", 6000m }
        };
        // Обработчик события изменения состояния чекбокса
        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            UpdateTotalCost();
        }
        // Метод для обновления общей стоимости
        private void UpdateTotalCost()
        {
            decimal totalCost = 0;
            var checkedServices = new List<string>();
            // Проверка состояния чекбоксов и добавление выбранных услуг в список
            if (zav.IsChecked == true) checkedServices.Add("Завтрак");
            if (tak.IsChecked == true) checkedServices.Add("Такси");
            if (ybor.IsChecked == true) checkedServices.Add("Уборка номера");

            // Подсчет общей стоимости выбранных услуг 
            foreach (var serviceName in checkedServices)
            {
                if (servicePrices.TryGetValue(serviceName, out decimal price))
                {
                    totalCost += price;
                }
            }

            // Получаем текущее значение zenroom
            if (double.TryParse(zenroom.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out double zenroomValue))
            {
                // Обновляем zen с общей стоимостью услуг и текущей стоимостью номера
                UpdateValues((double)totalCost, zenroomValue);
            }
        }
        // Обработчик события загрузки окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTotalCost();
        }
        // Обработчик события нажатия кнопки возвращения на окно доступных номеров
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // Метод для вычисления количества дней между датами
        private void CalculateDays()
        {
            if (dateTimePicker1.SelectedDate.HasValue && dateTimePicker2.SelectedDate.HasValue)
            {
                DateTime checkInDate = dateTimePicker1.SelectedDate.Value;
                DateTime checkOutDate = dateTimePicker2.SelectedDate.Value;
                // Проверка, что дата выезда позже даты заезда
                if (checkOutDate > checkInDate)
                {
                    int days = (checkOutDate - checkInDate).Days;
                    res.Text = days.ToString();
                }
                else
                {
                    res.Text = "Дата выезда должна быть позже даты заезда";
                }
            }
            else
            {
                res.Text = "Выберите даты заезда и выезда";
            }
        }
        // Обработчик события изменения выбора типа номера
        private void Tiproom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tiproom.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedRoomType = selectedItem.Content.ToString();
                if (roomPrices.TryGetValue(selectedRoomType, out decimal roomPrice))
                {
                    zenroom.Text = roomPrice.ToString("C", CultureInfo.CurrentCulture);
                }
                else
                {
                    zenroom.Text = "0,00";
                }
                // Обновляем общую стоимость после выбора типа номера
                TextB_TextChanged(null, null);
            }
            else
            {
                zenroom.Text = "0,00";
            }
        }
        // Метод для обновления значений общей стоимости
        public void UpdateValues(double newZenValue, double newZenroomValue)
        {
            zen.Text = newZenValue.ToString("C", CultureInfo.CurrentCulture);
            zenroom.Text = newZenroomValue.ToString("C", CultureInfo.CurrentCulture);

            // Обновляем общую стоимость
            TextB_TextChanged(null, null);
        }
        // Обработчик события изменения текста в текстовом поле
        private void TextB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(zen.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out double zenValue) &&
                double.TryParse(zenroom.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out double zenroomValue))
            {
                double total = zenValue + zenroomValue;
                allzen.Text = total.ToString("C", CultureInfo.CurrentCulture);
            }
            else
            {
                allzen.Text = "0,00";
            }
        }
        // Обработчик события изменения выбранной даты в первом календаре
        private void DateTimePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateDays();
        }
        // Обработчик события изменения выбранной даты во втором календаре
        private void DateTimePicker2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateDays();
        }
        // Обработчик события нажатия кнопки увеличения числа
        private void IncrementButton_Click(object sender, RoutedEventArgs e)
        {
            int currentNumber = int.Parse(NumberTextBlock.Text);
            if (currentNumber < 6)
            {
                NumberTextBlock.Text = (currentNumber + 1).ToString();
            }
        }
        // Обработчик события нажатия кнопки уменьшения числа
        private void DecrementButton_Click(object sender, RoutedEventArgs e)
        {
            int currentNumber = int.Parse(NumberTextBlock.Text);
            if (currentNumber > 1)
            {
                NumberTextBlock.Text = (currentNumber - 1).ToString();
            }
        }
        // Обработчик события изменения текста в поле ввода номера телефона
        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = number.Text.Trim(); // Получаем текст и удаляем лишние пробелы

            // Проверяем номер телефона
            if (!ValidatePhoneNumber(input))
            {
                validationMessage.Text = "Номер телефона должен быть в формате +7(XXX)XXX-XX-XX";
                validationMessage.Visibility = Visibility.Visible; // Показываем сообщение об ошибке
                number.BorderBrush = Brushes.Red; // Устанавливаем красную рамку для индикации ошибки
            }
            else
            {
                validationMessage.Visibility = Visibility.Collapsed; // Скрываем сообщение об ошибке
                number.BorderBrush = Brushes.Green; // Устанавливаем зеленую рамку для индикации корректного ввода
            }
        }
        // Метод для валидации номера телефона
        private bool ValidatePhoneNumber(string phoneNumber)
        {

            phoneNumber = phoneNumber.Replace(" ", "");
            Regex phoneRegex = new Regex(@"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$");
            return phoneRegex.IsMatch(phoneNumber);
        }
        // Метод для получения ID типа номера по его названию
        private int? GetRoomTypeId(string roomTypeName)
        {
            // Предположим, у вас есть словарь или база данных, где вы храните соответствие между названиями и ID типов номеров
            Dictionary<string, int> roomTypeMapping = new Dictionary<string, int>
             {
                 { "Люкс", 1 },
                 { "Одноместный", 2 },
                 { "Двухместный", 3 }
            };
            // Проверяем, есть ли в словаре соответствие для переданного названия типа номера
            if (roomTypeMapping.TryGetValue(roomTypeName, out int roomTypeId))
            {
                return roomTypeId; // Возвращаем ID типа номера
            }
            return null; // Если не найден, возвращаем null
        }
        // Метод для валидации данных гостя
        private bool ValidateGuest(Guest guest)
        {
            return !string.IsNullOrWhiteSpace(guest.ФИО) &&
                   !string.IsNullOrWhiteSpace(guest.Телефон) &&
                  !string.IsNullOrWhiteSpace(guest.Серия_и_Номер_Паспорта);
        }
        // Метод для получения доступного номера по типу
        private Room GetAvailableRoom(GalaxyEntities db, string roomType)
        {
            return db.Room.FirstOrDefault(r => r.Категория_номера == roomType && r.Доступность == "Свободен");
        }
        // Метод добавления дополнительных услуг в бронирование
        private void AddAdditionalService(GalaxyEntities db, int bookingId, string serviceName)
        {
            var service = db.AdditionalService.FirstOrDefault(s => s.Название == serviceName);
            if (service != null)
            {
                var agreement = new Agreement { Бронирование = bookingId, Список_услуг = service.ID_AdditionalService };
                db.Agreement.Add(agreement);
            }
        }
        // Метод для установки номера комнаты
        public void SetRoomNumber(string roomNumber)
        {
            num.Text = roomNumber;
        }
        // Метод для установки типа номера
        public void SetRoomType(string roomType)
        {
            foreach (ComboBoxItem item in tiproom.Items)
            {
                if (item.Content.ToString() == roomType)
                {
                    tiproom.SelectedItem = item;
                    break;
                }
            }
        }
        // Обработчик события нажатия кнопки "Бронировать"
        private void Broni_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new GalaxyEntities())
                {
                    // Добавление гостя
                    var contact = new Guest();
                    contact.ФИО = fio.Text;
                    contact.Телефон = number.Text.Trim();
                    contact.Серия_и_Номер_Паспорта = ser.Text;

                    // Валидация данных гостя
                    if (!ValidateGuest(contact))
                    {
                        MessageBox.Show("Пожалуйста, заполните все поля для гостя.");
                        return;
                    }
                    // Валидация номера телефона
                    if (!ValidatePhoneNumber(contact.Телефон))
                    {
                        MessageBox.Show("Пожалуйста, введите корректный номер телефона.");
                        return;
                    }
                    db.Guest.Add(contact);
                    db.SaveChanges();

                    // Проверка типа номера
                    if (tiproom.SelectedItem is ComboBoxItem selectedItem)
                    {
                        string selectedRoomType = selectedItem.Content.ToString();
                        var room = GetAvailableRoom(db, selectedRoomType);

                        if (room == null)
                        {
                            MessageBox.Show("Нет доступных комнат данного типа.");
                            return;
                        }
                        // Создание бронирования
                        var booking = new Booking();
                        booking.Дата_заезда = dateTimePicker1.SelectedDate.Value;
                        booking.Дата_выезда = dateTimePicker2.SelectedDate.Value;
                        booking.Количество_дней_проживания = (dateTimePicker2.SelectedDate.Value - dateTimePicker1.SelectedDate.Value).Days.ToString();
                        booking.Гость = contact.ID_Guest;
                        booking.Номер = room.ID_room;
                        booking.Количество_человек = NumberTextBlock.Text;
                        if (!dateTimePicker1.SelectedDate.HasValue || !dateTimePicker2.SelectedDate.HasValue)
                        {
                            MessageBox.Show("Пожалуйста, выберите даты заезда и отъезда.");
                            return;
                        }
                        // Обновление доступности номера
                        room.Доступность = "Занят";
                        db.SaveChanges();

                        db.Booking.Add(booking);
                        db.SaveChanges();
                        // Получаем ID только что созданного бронирования
                        int bookingId = booking.ID_Booking;

                        // Получаем ID типа номера
                        int? roomTypeId = GetRoomTypeId(selectedRoomType);
                        if (roomTypeId == null)
                        {
                            MessageBox.Show("Не удалось получить ID типа номера.");
                            return;
                        }

                        // Создание договора
                        var dog = new Agreement
                        {
                            Бронирование = bookingId,
                            Дата_заключения = DateTime.Now,
                            Список_услуг = roomTypeId,
                            Общая_стоимость = decimal.Parse(allzen.Text, NumberStyles.Currency, CultureInfo.CurrentCulture)
                        };
                        // Добавление дополнительных услуг
                        List<string> additionalServices = new List<string>();
                        if (zav.IsChecked == true)
                        {
                            additionalServices.Add("Завтрак");
                        }
                        if (tak.IsChecked == true)
                        {
                            additionalServices.Add("Такси");
                        }
                        if (ybor.IsChecked == true)
                        {
                            additionalServices.Add("Уборка номера");
                        }
                        // Сохранение дополнительных услуг в базе данных
                        foreach (var service in additionalServices)
                        {
                            AddAdditionalService(db, bookingId, service);
                        }

                        // Получение дополнительных услуг для текущего бронирования
                        List<string> additionalServicesList = GetAdditionalServicesForBooking(bookingId);

                        ExportAgreementToPdf(dog, contact, booking);
                        db.Agreement.Add(dog);
                        db.SaveChanges();

                        MessageBox.Show($"Бронирование прошло успешно!");
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, выберите тип номера.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
        private List<string> GetAdditionalServicesForBooking(int bookingId)
        {
            using (var db = new GalaxyEntities())
            {
                return db.AdditionalService
                         .Where(service => service.ID_AdditionalService == bookingId)
                         .Select(service => service.Название)
                         .ToList();
            }
        }
        private void ExportAgreementToPdf(Agreement agreement, Guest guest, Booking booking)
        {
            string pdfPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Договор.pdf");
            string fontPath = "C:\\Users\\vip66\\OneDrive\\Рабочий стол\\Comic Sans MS.ttf"; // Укажите путь к вашему TTF-шрифту

            try
            {
                // Создание документа
                using (Document document = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));
                    document.Open();

                    // Создание шрифта с поддержкой кириллицы
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font font = new Font(baseFont, 10);

                    // Проверка данных
                    if (guest == null || agreement == null)
                    {
                        MessageBox.Show("Данные для экспорта отсутствуют.");
                        return;
                    }
                    // Заголовок
                    document.Add(new Paragraph($"Договор бронирования - № {booking.Номер}", font));
                    document.Add(new Paragraph($"ФИО Заказчика: {guest.ФИО}", font));
                    document.Add(new Paragraph($"Телефон: {guest.Телефон}", font));
                    document.Add(new Paragraph($"Серия и Номер Паспорта: {guest.Серия_и_Номер_Паспорта}", font));
                    document.Add(new Paragraph($"Дата заключения договора: {DateTime.Now.ToString("dd.MM.yyyy")}", font));
                    document.Add(new Paragraph($"Дата заезда: {dateTimePicker1.SelectedDate.Value.ToString("dd.MM.yyyy")}", font));
                    document.Add(new Paragraph($"Дата выезда: {dateTimePicker1.SelectedDate.Value.ToString("dd.MM.yyyy")}", font));
                    document.Add(new Paragraph($"Общая стоимость: {agreement.Общая_стоимость:C}", font));
                    document.Add(new Paragraph($"Количество дней:{booking.Количество_дней_проживания}", font));
                    document.Add(new Paragraph($"Количество человек:{booking.Количество_человек}", font));
                    // document.Add(new Paragraph("Дополнительные услуги:", font));

                    // Добавление списка дополнительных услуг
                    var services = GetAdditionalServicesForBooking(booking.ID_Booking);
                    foreach (var service in services)
                    {
                        document.Add(new Paragraph($"- {service}", font));
                    }
                    document.Add(new Paragraph("Статус бронирования: Подтверждено", font));
                    document.Add(new Paragraph("г. Слободской", font));
                    document.Add(new Paragraph($"{guest.ФИО} именуемый(ая) в дальнейшем \"Заказчик\", с одной стороны, и Индивидуальный предприниматель Поглазова Валерия Владимировна, именуемый в дальнейшем \"Исполнитель\", с другой стороны, в дальнейшем именуемые «Стороны», заключили настоящий договор (далее Договор) о нижеследующем:", font));

                    // Общие положения
                    document.Add(new Paragraph("ОБЩИЕ ПОЛОЖЕНИЯ", font));
                    document.Add(new Paragraph("1.1. Заказывая услуги через Исполнителя, Заказчик соглашается с условиями Договора публичной оферты (далее - Договор), изложенными ниже. Исполнитель оказывает Заказчику услуги по предоставлению услуг бронирования объекта недвижимости для временного размещения (далее Объект).", font));
                    document.Add(new Paragraph("1.2. Настоящий Договор, а также приложения, относящиеся к нему, являются публичной офертой.", font));
                    document.Add(new Paragraph("1.3. Полным и безоговорочным принятием условий Договора считается осуществление Заказчиком платежа в счет оплаты услуг по бронированию.", font));

                    // Предмет договора
                    document.Add(new Paragraph("ПРЕДМЕТ ДОГОВОРА", font));
                    document.Add(new Paragraph("2.1. В рамках настоящего Договора Исполнитель за плату обязуется оказать услуги и совершать определенные действия по бронированию объекта недвижимости для временного размещения (далее-Объект), соответствующего установленным настоящим Договором требованиям, в Заказчик обязуется оплатить эти услуги.", font));
                    document.Add(new Paragraph("2.2. Исполнитель подтверждает бронирование, после оплаты Заказчиком 50% от общей суммы бронирования, в размере и в сроки, указанные в разделе 5 настоящего Договора.", font));

                    // Обязанности сторон
                    document.Add(new Paragraph("ОБЯЗАННОСТИ СТОРОН", font));
                    document.Add(new Paragraph("3.1. Исполнитель обязан:", font));
                    document.Add(new Paragraph("3.1.1. Осуществлять для Заказчика подбор Объектов и уведомлять его о наиболее подходящих из них;", font));
                    document.Add(new Paragraph("3.1.2. Предоставлять всю необходимую Заказчику информацию о предлагаемых Объектах;", font));
                    document.Add(new Paragraph("3.1.3. Осуществлять деловые контакты, вести переговоры с потенциальными Арендодателями в интересах Заказчика;", font));
                    document.Add(new Paragraph("3.1.4. После внесения Заказчиком суммы бронирования, в размере и сроки, указанные в разделе 5, предоставить Заказчику Акт об оказании услуг по Договору.", font));
                    document.Add(new Paragraph("3.1.5. По желанию Заказчика согласовывать просмотр Объекта с Принимающей стороной.", font));

                    // Закрытие документа
                    document.Close();
                }

                MessageBox.Show($"Договор успешно экспортирован в PDF: {pdfPath}");
                OpenPdfFile(pdfPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при создании PDF: {ex.Message}");
            }
        }

        // Метод для открытия PDF-файла
        private void OpenPdfFile(string pdfPath)
        {
            // Проверка, существует ли файл
            if (File.Exists(pdfPath))
            {
                try
                {
                    // Открытие PDF-файла с помощью стандартного приложения
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = pdfPath,
                        UseShellExecute = true // Используем оболочку для открытия файла
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть PDF-файл: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("PDF-файл не найден. Убедитесь, что он был создан.");
            }
        }
    }
}
