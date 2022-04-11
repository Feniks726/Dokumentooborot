using System.Windows;

namespace Dokumentooborot.Windows
{
    /// <summary>
    /// Логика взаимодействия для wReports.xaml
    /// </summary>
    public partial class wReports : Window
    {
        public wReports()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new wMakeReports().ShowDialog();
            this.Show();
        }
    }
}
