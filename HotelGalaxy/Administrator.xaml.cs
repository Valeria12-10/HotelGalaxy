using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelGalaxy
{
    public partial class Administrator : Window
    {
        public Administrator()
        {
            InitializeComponent();
        }

        private void Bron_Click(object sender, RoutedEventArgs e)
        {
            Reservation fn = new Reservation();
            fn.Show();
            this.Close();
        }

        private void ViewRoom_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new GalaxyEntities())
            {
                var rooms = (from room in db.Room
                                  
                                  select new
                                  {
                                      ID = room.ID_room,
                                      Номер_номера = room.Номер,
                                      Категория_номера = room.Категория_номера,
                                      Цена = room.Цена,
                                      Доступность = room.Доступность
                                  }).ToList();

                // Заполнение DataGrid данными
                data.ItemsSource = rooms;
            }
        }
    }
}
