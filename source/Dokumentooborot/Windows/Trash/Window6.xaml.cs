using System.Windows;

namespace Dokumentooborot.Windows.Trash
{
    /// <summary>
    /// Логика взаимодействия для Window6.xaml
    /// </summary>
    public partial class Window6 : Window
    {
        public Window6()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window7().ShowDialog();
            this.Show();
        }
    }
}
