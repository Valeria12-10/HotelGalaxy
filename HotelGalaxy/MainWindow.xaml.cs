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
        public object num { get; internal set; }

        public MainWindow()
        {
            InitializeComponent();
        }
        // Проверяет введенные логин и пароль, и, в зависимости от роли пользователя, открывает соответствующее окно
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
        // Отображает введенный пароль в текстовом поле
        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            // Показать пароль
            if (password != null)
                par.Text = password.Password;  
            par.Visibility = Visibility.Visible;
        }
        // Скрывает введенный пароль в текстовом поле
        private void HidePassword_Unchecked(object sender, RoutedEventArgs e)
        {
            // Скрыть пароль
            if (password != null)
                password.Password = par.Text;
            
            par.Visibility = Visibility.Hidden; 
                                                         
        }
        // Открывает окно регистрации нового пользователя
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Registration fn = new Registration();
            fn.Show();
            this.Close();
        }
        //Выход из программы 
        private void NewClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Forgot_Click(object sender, RoutedEventArgs e)
        {
        }    
    }
}
