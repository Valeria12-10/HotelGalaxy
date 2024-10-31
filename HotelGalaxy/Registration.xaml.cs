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
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        private void Register1_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new GalaxyEntities())
            {
                var existingUser = db.User.Any(u => u.ФИО == fio.Text &&
                u.Телефон == tel.Text && u.Логин == log.Text &&
                u.Пароль == par.Text);

                if (existingUser)
                {
                    MessageBox.Show("Такой логин уже существует");
                    return;
                }

                var userPersonal = new User
                {
                    ФИО = fio.Text,
                    Телефон = tel.Text,
                    Логин = log.Text,
                    Пароль = par.Text
                };
                db.User.Add(userPersonal);


                db.SaveChanges();

                MessageBox.Show("Вы успешно зарегистрировались");

                Administrator fn = new Administrator();
                fn.Show();
                this.Close();

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fn = new MainWindow();
            fn.Show();
            this.Close();
        }
    }
}
