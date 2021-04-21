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
    /// <summary>
    /// Логика взаимодействия для Link.xaml
    /// </summary>
    public partial class Link : UserControl
    {
        public Link()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(string), typeof(Link), new PropertyMetadata("",ContentPropertyChanged));
        public string Content
        {
            get 
            {
                return (string)GetValue(ContentProperty);
            }

            set 
            {
                SetValue(ContentProperty, value);
            }
        }

        private static void ContentPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e) 
        {
            Link link = source as Link;
            string value = e.NewValue as string;

            link.lbLink.Content = value;
        }
        private void lbLink_MouseEnter(object sender, MouseEventArgs e) => lbLink.Foreground = Brushes.White;
        private void lbLink_MouseLeave(object sender, MouseEventArgs e) => lbLink.Foreground = Brushes.Yellow;
    }
}
