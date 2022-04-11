using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Dokumentooborot.DAL;
using Dokumentooborot.Windows.Trash;

namespace Dokumentooborot.Windows
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class wUser : Window
    {
        BDEntities db;

        public wUser()
        {
            InitializeComponent();
             
            db = new BDEntities();
       }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window6().ShowDialog();
            this.Show();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Find_LostFocus(sender, e);
        }

        private void Find_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void txtIndexSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void cmbTypeDocuments_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void cmbPlaceStore_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void SearchDocuments()
        {
            List<Document> documents = db.Documents.ToList();

            if (!string.IsNullOrEmpty(Find.Text))
            {
                 documents =  documents.Where(t => t.Name.Contains(Find.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(txtIndexSearch.Text))
            {
                documents = documents.Where(t => t.Index.Contains(txtIndexSearch.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(cmbTypeDocuments.Text))
            {
                documents =documents.Where(t => t.Doc_tipe_id == (int)cmbTypeDocuments.SelectedValue).ToList();
            }

            if (!string.IsNullOrEmpty(cmbPlaceStore.Text))
            {
                documents = documents.Where(t => t.Save_place_id == (int)cmbPlaceStore.SelectedValue).ToList();
            }

            dgogrenci.ItemsSource = documents.ToList();
        }
    }
}
