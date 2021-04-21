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

namespace Client.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class RestoreWindow : Window
    {
        public RestoreWindow()
        {
            InitializeComponent();
        }

        private void picExit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => Environment.Exit(0);
        private void gTopBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void btRestore_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btRestore.IsEnabled = false;
            tbLogin.IsEnabled = false;
            tbCode.IsEnabled = false;

            tbCode.Visibility = Visibility.Visible;
            pbPass.Visibility = Visibility.Visible;
            pbPassConfirm.Visibility = Visibility.Visible;

            btRestore.IsEnabled = true;
        }
    }
}
