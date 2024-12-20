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

            using (var db = new GalaxyEntities())
            {
                var rooms = (from room in db.Room

                             select new
                             {

                                 Номер = room.Номер,
                                 КатегорияНомера = room.Категория_номера,
                                 Цена = room.Цена,
                                 Доступность = room.Доступность
                             }).ToList();


                data.ItemsSource = rooms;

            }


        }

        // Открываем окно для просмотра доступных номеров 
        private void Bron_Click(object sender, RoutedEventArgs e)
        {
            Rooms fn = new Rooms();
            fn.Show();
            
        }
        // Извлекаем список доступных номеров из базы данных и отображает его в элементе данных
        private void ViewRoom_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new GalaxyEntities())
            {
                var rooms = (from room in db.Room
                                  
                                  select new
                                  {
                                     
                                      Номер = room.Номер,
                                      КатегорияНомера = room.Категория_номера,
                                      Цена = room.Цена,
                                      Доступность = room.Доступность
                                  }).ToList();

               
                data.ItemsSource = rooms;
               
            }
        }

        // Возврат на окно авторизации и закрываем текущее окно
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fn = new MainWindow();
            fn.Show();
            this.Close();

         
        }

        // Извлекаем список всех бронирований и связанных с ними данных из базы данных и отображает его в элементе данных
        private void ViewBron_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new GalaxyEntities())
            {
                var brons = (from bron in db.Booking
                             join gu in db.Guest on bron.Гость equals gu.ID_Guest
                             join nu in db.Room on bron.Номер equals nu.ID_room
                             select new
                             {
                                
                                 Номер = nu.Номер,
                                 ТипНомера = nu.Категория_номера,
                                 Гость = gu.ФИО,
                                 Заезд = bron.Дата_заезда,
                                 Выезд = bron.Дата_выезда,
                                 Количество_Дней = bron.Количество_дней_проживания,
                                 КоличествоЧеловек = bron.Количество_человек,
                                 ДополнительныеУслуги = (from agr in db.Agreement
                                                         join ag in db.AdditionalService on agr.Список_услуг equals ag.ID_AdditionalService
                                                         where agr.Бронирование == bron.ID_Booking
                                                         select ag.Название).ToList()
                             }).ToList();

                // Объединяем дополнительные услуги в строку
                var bronsWithJoinedServices = brons.Select(b => new
                {
                    Номер = b.Номер,
                    ТипНомера = b.ТипНомера,
                    Гость = b.Гость,
                    Заезд = b.Заезд,
                    Выезд = b.Выезд,
                    Количество_Дней = b.Количество_Дней,
                    КоличествоЧеловек = b.КоличествоЧеловек,
                    СписокУслуг = string.Join(", ", b.ДополнительныеУслуги)
                }).ToList();

                // Устанавливаем источник данных для таблицы
                data.ItemsSource = bronsWithJoinedServices;
            }
        }

        // Извлекаем информацию о гостях и связанных с ними бронированиях из базы данных и отображает ее в элементе данных
        private void ViewGu_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new GalaxyEntities())
            {
                var brons = (from bron in db.Booking
                             join gu in db.Guest on bron.Гость equals gu.ID_Guest
                             join nu in db.Room on bron.Номер equals nu.ID_room
                             select new
                             {
                                 Гость = gu.ФИО, 
                                 Телефон = gu.Телефон,
                                 Паспорт = gu.Серия_и_Номер_Паспорта,
                                 Номер = nu.Номер,
                                 ТипНомера = nu.Категория_номера,
                                
                                 
                             }).ToList();


                var bronsWithJoinedServices = brons.Select(b => new
                {
                    Гость = b.Гость,
                    Телефон = b.Телефон,
                    Паспорт = b.Паспорт,
                    Номер = b.Номер,
                    ТипНомера = b.ТипНомера
                }).ToList();

                data.ItemsSource = bronsWithJoinedServices;
            }
        }

        // Извлекает информацию о договоров, связанных с бронированиями, и отображает ее в элементе данных
        private void ViewGo_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new GalaxyEntities())
            {
                var brons = (from ag in db.Agreement
                             join bo in db.Booking on ag.Бронирование equals bo.ID_Booking
                             join r in db.Room on bo.Номер equals r.ID_room 
                             join g in db.Guest on bo.Гость equals g.ID_Guest 
                             join s in db.AdditionalService on ag.Список_услуг equals s.ID_AdditionalService

                             select new
                             {   НомерБрони = bo.ID_Booking,
                                 Номер = r.Номер,
                                 Гость = g.ФИО,
                                 Дата_Заключения = ag.Дата_заключения,
                                 Тип_номера = r.Категория_номера,
                                 Количество_Человек = bo.Количество_человек,
                                 Количество_Дней = bo.Количество_дней_проживания,
                                 СписокУслуг = s.Название,
                                 Общая_стоимость = ag.Общая_стоимость
                             }).ToList();
               

                data.ItemsSource = brons;

            }
        }
    }
}
