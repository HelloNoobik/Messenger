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
    public partial class RestoreWindow : Window
    {
        SocketClient socketClient;
        public RestoreWindow(SocketClient client)
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

        private void btRestore_Click(object sender, RoutedEventArgs e)
        {
            btRestore.IsEnabled = false;
            string login = tbLogin.Text.Trim(' ');

            if (!String.IsNullOrWhiteSpace(login))
            {
                if (socketClient.RestoreRequest(login))
                {
                    tbLogin.IsEnabled = false;
                    tbCode.Visibility = Visibility.Visible;

                    btRestore.Click -= btRestore_Click;
                    btRestore.Click += btRestore_Click_2;
                } 
            }

            btRestore.IsEnabled = true;
        }

        private void btRestore_Click_2(object sender, RoutedEventArgs e) 
        {
            btRestore.IsEnabled = false;
            string code = tbCode.Text.Trim(' ');

            if (!String.IsNullOrWhiteSpace(code))
            {
                if (socketClient.RestoreRequest(code))
                {
                    tbCode.IsEnabled = false;
                    pbPass.Visibility = Visibility.Visible;
                    pbPassConfirm.Visibility = Visibility.Visible;

                    btRestore.Click -= btRestore_Click_2;
                    btRestore.Click += btRestore_Click_3;
                }
                else
                {
                    btRestore.Click -= btRestore_Click_2;
                    btRestore.Click += btRestore_Click;
                } 
            }

            btRestore.IsEnabled = true;
        }

        private void btRestore_Click_3(object sender, RoutedEventArgs e)
        {
            btRestore.IsEnabled = false;
            string password = pbPass.Password.Trim(' ');
            string passwordConfirm = pbPassConfirm.Password.Trim(' ');

            if (!String.IsNullOrWhiteSpace(password) && !String.IsNullOrWhiteSpace(passwordConfirm) && password == passwordConfirm)
            {
                if (socketClient.ChangePassword(password))
                {
                    AuthWindow win = new AuthWindow(socketClient);
                    win.Show();
                    Close();
                } 
            }

            btRestore.IsEnabled = true;
        }
    }
}
