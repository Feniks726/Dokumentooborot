using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Dokumentooborot.DAL;
using Dokumentooborot.Windows.Trash;

namespace Dokumentooborot.Windows
{
    /// <summary>
    /// Логика взаимодействия для wUser.xaml
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
            LoadTypeDocuments();
            LoadPlaceStore();
            chbRelevant.IsChecked = null;
            Find_LostFocus(sender, e);
        }


        private void LoadTypeDocuments()
        {
            List<Doc_tipe> typeDocs = db.Doc_tipe.ToList();
            typeDocs.Insert(0, new Doc_tipe { Id = -1, Name = string.Empty });
            cmbTypeDocuments.ItemsSource = typeDocs.ToList();
        }

        private void LoadPlaceStore()
        {
            List<Save_place> placeStores = db.Save_place.ToList();
            placeStores.Insert(0,new Save_place { Id = -1, Name = string.Empty });
            cmbPlaceStore.ItemsSource = placeStores.ToList();
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

        private void chbRelevant_Checked(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void SearchDocuments()
        {
            List<Document> documents = db.Documents.ToList();

            if (!string.IsNullOrEmpty(Find.Text))
            {
                documents = documents.Where(t => t.Name.Contains(Find.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(txtIndexSearch.Text))
            {
                documents = documents.Where(t => t.Index.Contains(txtIndexSearch.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(cmbTypeDocuments.Text))
            {
                documents = documents.Where(t => t.Doc_tipe_id == ((Doc_tipe)cmbTypeDocuments.SelectedValue).Id).ToList();
            }

            if (!string.IsNullOrEmpty(cmbPlaceStore.Text))
            {
                documents = documents.Where(t => t.Save_place_id == ((Save_place)cmbPlaceStore.SelectedValue).Id).ToList();
            }

            if (chbRelevant.IsChecked != null)
            {
                documents = documents.Where(t => t.Relevance == chbRelevant.IsChecked.Value).ToList();
            }

            dgogrenci.ItemsSource = documents.ToList();
        }
    }
}
