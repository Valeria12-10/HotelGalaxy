using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelGalaxy
{
    public partial class Rooms : Window
    {
        public Rooms()
        {
            InitializeComponent();
        }

        // Получение выбранного  номера
        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            
            var selectedRoom = dgnum.SelectedItem;

            if (selectedRoom == null)
            {
                MessageBox.Show("Пожалуйста, выберите номер.");
                return;
            }

            // Получаем номер из выбранного элемента
            var roomData = selectedRoom as dynamic; 
            string roomNumber = roomData?.Номер;

            if (string.IsNullOrEmpty(roomNumber))
            {
                MessageBox.Show("Ошибка: выбранный номер недоступен.");
                return;
            }

            // Получаем выбранный тип номера из ComboBox
            string selectedRoomType = (tiproom.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(selectedRoomType))
            {
                MessageBox.Show("Пожалуйста, выберите тип номера.");
                return;
            }

            // Открываем новое окно и передаем номер и тип
            var reservationWindow = new HotelGalaxy.Reservation();
            reservationWindow.SetRoomNumber(roomNumber); 
            reservationWindow.SetRoomType(selectedRoomType);
            reservationWindow.Show();

           // this.Close();
        }



        //Поиск свободных номеров
        private void Search_Click(object sender, RoutedEventArgs e)
        {
           
            string selectedRoomType = (tiproom.SelectedItem as ComboBoxItem)?.Content.ToString();

            
            if (string.IsNullOrEmpty(selectedRoomType))
            {
                MessageBox.Show("Пожалуйста, выберите тип номера.");
                return;
            }

            try
            {
                using (var db = new GalaxyEntities())
                {
                    
                    var rooms = (from room in db.Room
                                 where room.Категория_номера == selectedRoomType && room.Доступность == "Свободен" 
                                 select new
                                 {
                                     Номер = room.Номер,
                                     КатегорияНомера = room.Категория_номера,
                                     Цена = room.Цена,
                                     Доступность = room.Доступность
                                 }).ToList();

                    // Устанавливаем источник данных для DataGrid
                    dgnum.ItemsSource = rooms;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске номеров: {ex.Message}");
            }
        }


    }
}


