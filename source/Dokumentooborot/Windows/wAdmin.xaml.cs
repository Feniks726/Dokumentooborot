using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Dokumentooborot.DAL;

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
            db = new BDEntities();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            new wDocumentAdding().ShowDialog();            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new wUsersManage().ShowDialog();                        
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new wReports().ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            new wBackUp().ShowDialog();
        }

        private void dgogrenci_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selctedRow = (Document)dgogrenci.SelectedItem;

            if (selctedRow != null) //убрать double click на шапке
            {
                Document doc = new Document();
                doc.Id = selctedRow.Id;
                D.dataD1 = doc.Id.ToString(); //передаю Id документа            
                new wDocumentEdit().ShowDialog();
                this.Show();
            }
            
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
            placeStores.Insert(0, new Save_place { Id = -1, Name = string.Empty });
            cmbPlaceStore.ItemsSource = placeStores.ToList();
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

        private void Find_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void txtIndexSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void txtNumberOrder_LostFocus(object sender, RoutedEventArgs e)
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

        private void dateActionTo_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void dateOrder_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchDocuments();
        }

        private void chbRelevant_LostFocus(object sender, RoutedEventArgs e)
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

            if (!string.IsNullOrEmpty(dateActionTo.Text))
            {
                documents = documents.Where(t => t.Validity_period <= dateActionTo.SelectedDate).ToList();
            }

            if (!string.IsNullOrEmpty(dateOrder.Text))
            {
                documents = documents.Where(t => t.Date_order == dateOrder.SelectedDate).ToList();
            }

            if (chbRelevant.IsChecked != null)
            {
                documents = documents.Where(t => t.Relevance == chbRelevant.IsChecked.Value).ToList();
            }

            dgogrenci.ItemsSource = documents.ToList();
        }

        private void dgogrenci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (MessageBox.Show("Удалить запись?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Document d = dgogrenci.SelectedItem as Document;
                    foreach (Who_know item in db.Who_know.ToList())
                    {
                        if (item.Document_id == d.Id)
                        {
                            db.Who_know.Remove(item);
                        }
                    }
                    db.Documents.Remove((Document)dgogrenci.SelectedItem);
                    db.SaveChanges();
                    dgogrenci.ItemsSource = db.Documents.ToList();
                }
            }
        }
    }
}
