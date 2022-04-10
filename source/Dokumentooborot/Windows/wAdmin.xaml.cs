using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Dokumentooborot.DAL;
using Dokumentooborot.Windows.Trash;

namespace Dokumentooborot.Windows
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class wAdmin : Window
    {
        BDEntities db;
        public wAdmin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            new Window9().ShowDialog();            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Window3().ShowDialog();                        
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new Window6().ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            new Window8().ShowDialog();
        }

        private void dgogrenci_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selctedRow = (Document)dgogrenci.SelectedItem;

            if (selctedRow != null) //убрать double click на шапке
            {
                Document doc = new Document();
                doc.Id = selctedRow.Id;
                D.dataD1 = doc.Id.ToString(); //передаю Id документа            
                new Window4().ShowDialog();
                this.Show();
            }
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new BDEntities();
            dgogrenci.ItemsSource = db.Documents.ToList();
            
        }

        private void FileClick(object sender, RoutedEventArgs e)
        {
            Document u = this.dgogrenci.SelectedItem as Document;
            
            string filename = "@\"" + u.Doc_file + "\"";
            MessageBox.Show(filename);

            if (filename != null)
            {
               FileStream fs = File.Open(filename, FileMode.Open);
            }            
        }
    }
}
