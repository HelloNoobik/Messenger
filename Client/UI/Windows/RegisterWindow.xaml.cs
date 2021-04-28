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
using Client.Classes;

namespace Client.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        SocketClient socketClient;
        public RegisterWindow(SocketClient client)
        {
            InitializeComponent();
            socketClient = client;
        }

        private void picExit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => Environment.Exit(0);
        private void gTopBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void liBack_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AuthWindow win = new AuthWindow(socketClient);
            win.Show();
            Close();
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            btRegister.IsEnabled = false;
            string login = tbLogin.Text.Trim(' ');
            string email = tbEmail.Text.Trim(' ');
            string password = pbPass.Password.Trim(' ');
            string passwordConfirm = pbPassConfirm.Password.Trim(' ');

            if(!String.IsNullOrWhiteSpace(login) && !String.IsNullOrWhiteSpace(password) && !String.IsNullOrWhiteSpace(passwordConfirm) && !String.IsNullOrWhiteSpace(email) && password == passwordConfirm) 
            {
                if (socketClient.RegisterRequest(login, password, email)) 
                {
                    AuthWindow win = new AuthWindow(socketClient);
                    win.Show();
                    Close();
                }
            }
            btRegister.IsEnabled = true;
        }
    }
}
