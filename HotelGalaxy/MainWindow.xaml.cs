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

                if (userPersonal == null)
                {
                    MessageBox.Show("Неверно введен логин или пароль");
                }
                else
                {
                    MessageBox.Show("Вход успешно выполнен");
                    var usern = db.Role.FirstOrDefault(role => role.ID_role == userPersonal.ID);
                    if (usern != null)
                    {
                        if (usern. Роль== "Администратор")
                        {
                            Administrator rn = new Administrator();
                            // Получение имени и фамилии сотрудника из таблицы User

                            var employee = db.User.FirstOrDefault(el => el.ID == usern.ID_role);

                            if (employee != null)
                            {
                                rn.user.Text = $"{employee.ФИО}";
                            }

                            // Получение роли сотрудника из таблицы Role
                            var role = db.Role.FirstOrDefault(r => r.ID_role == employee.ID);
                            if (role != null)
                            {
                                rn.role.Text = $"Пользователь системы: {role.Роль}";
                            }
                            rn.Show();
                            this.Close();
                        }
                        else if(usern.Роль == "Менеджер")
                        {
                            Administrator rn = new Administrator();
                            // Получение имени и фамилии сотрудника из таблицы User

                            var employee = db.User.FirstOrDefault(el => el.ID == usern.ID_role);

                            if (employee != null)
                            {
                                rn.user.Text = $"{employee.ФИО}";
                            }

                            // Получение роли сотрудника из таблицы Role
                            var role = db.Role.FirstOrDefault(r => r.ID_role == employee.ID);
                            if (role != null)
                            {
                                rn.role.Text = $"Пользователь системы: {role.Роль}";
                            }
                            rn.Show();
                            this.Close();
                        }


                    }
                    

                }

               
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Registration fn = new Registration();
            fn.Show();
            this.Close();
        }
    }
}
