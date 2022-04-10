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
using System.IO;


namespace Dokumentooborot
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        BDEntities db;
        public Window1()
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
