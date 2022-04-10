using System.Windows;
using Dokumentooborot.Windows.Trash;

namespace Dokumentooborot.Windows
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class wUser : Window
    {
        public wUser()
        {
            InitializeComponent();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window6().ShowDialog();
            this.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
