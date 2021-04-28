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
using Client.Classes;
using Client.UI.UserControls;
using SharedLibrary;

namespace Client.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChatsWindow.xaml
    /// </summary>
    public partial class ChatsWindow : Window
    {
        SocketClient client;
        public ChatsWindow(SocketClient socketClient)
        {
            InitializeComponent();
            client = socketClient;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<SharedLibrary.Chat> chats = client.GetChats();

            foreach (SharedLibrary.Chat chat in chats) 
            {
                Dialog dialog = new Dialog(chat);
                spDialogs.Children.Add(dialog);
            }
        }
    }
}
