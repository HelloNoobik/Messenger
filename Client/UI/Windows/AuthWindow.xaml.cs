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
    public partial class AuthWindow : Window
    {
        SocketClient socketClient;
        public AuthWindow()
        {
            InitializeComponent();
            socketClient = new SocketClient();
        }

        public AuthWindow(SocketClient client)
        {
            InitializeComponent();
            socketClient = client;
        }

        private void picExit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => Environment.Exit(0);
        private void gTopBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void liRegister_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RegisterWindow win = new RegisterWindow(socketClient);
            win.Show();
            Close();
        }

        private void liRestore_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RestoreWindow win = new RestoreWindow(socketClient);
            win.Show();
            Close();
        }

        private void btAuth_Click(object sender, RoutedEventArgs e)
        {
            btAuth.IsEnabled = false;
            string login = tbLogin.Text.Trim(' ');
            string password = pbPass.Password.Trim(' ');
            if (!String.IsNullOrWhiteSpace(login) && !String.IsNullOrWhiteSpace(password)) 
            {
                if (socketClient.AuthRequest(login, password)) 
                {
                    ChatsWindow win = new ChatsWindow(socketClient);
                    win.Show();
                    Close();
                }
            }
            btAuth.IsEnabled = true;
        }
    }
}
