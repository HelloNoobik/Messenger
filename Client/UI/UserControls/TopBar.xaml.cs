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

namespace Client.UI.UserControls
{
    public partial class TopBar : UserControl
    {
        public TopBar()
        {
            InitializeComponent();
        }

        private void gTopBar_PreviewMouseDown(object sender, MouseButtonEventArgs e) => Window.GetWindow(this).DragMove();
        private void picExit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => Environment.Exit(0);
        private void picExit_MouseEnter(object sender, MouseEventArgs e) => picExit.Foreground = Brushes.White;
        private void picExit_MouseLeave(object sender, MouseEventArgs e) => picExit.Foreground = Brushes.Black;
    }
}
