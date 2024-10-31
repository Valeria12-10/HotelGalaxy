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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelGalaxy
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new GalaxyEntities()) // Подключение к базе данных
            {

                var userPersonal = db.User.FirstOrDefault(user => user.Логин == log.Text && user.Пароль == par.Text);


                if (userPersonal != null)
                {
                    Administrator fn = new Administrator();
                    fn.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверно введен логин или пароль");
                }
            };
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Registration fn = new Registration();
            fn.Show();
            this.Close();
        }
    }
}
