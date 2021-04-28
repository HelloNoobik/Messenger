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
using SharedLibrary;
using Client.UI.Windows;

namespace Client.UI.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Dialog.xaml
    /// </summary>
    public partial class Dialog : UserControl
    {
        public List<SharedLibrary.Message> Messages { get; private set; }
        public Dialog()
        {
            InitializeComponent();
            Messages = new List<SharedLibrary.Message>();
        }

        public Dialog(SharedLibrary.Chat chat) : this() 
        {
            Messages = chat.Messages;
            ChatName.Content = chat.Creator;
            LastMessage.Content = Messages.Last();
        }

        public void UpdateMessages(SharedLibrary.Chat chat) 
        {
            Messages = chat.Messages;
            LastMessage.Content = Messages.Last().MessageText;
        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            ChatsWindow win = (ChatsWindow)Window.GetWindow(this);
            win.gChat.Children.Clear();
            win.gChat.Children.Add(new Chat(this));
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e) => border.BorderBrush = Brushes.Yellow;
        private void Border_MouseLeave(object sender, MouseEventArgs e) => border.BorderBrush = Brushes.White;
    }
}
