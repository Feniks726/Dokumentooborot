using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using Dokumentooborot.DAL;

namespace Dokumentooborot.Windows
{
    /// <summary>
    /// Логика взаимодействия для Window9.xaml
    /// </summary>
    public class who_know
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public who_know(int Id, string Name)
        {
            RoleId = Id;
            RoleName = Name;
        }
    }
    public partial class wDocumentAdding : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source = DESKTOP-HSHULSK\SQL;Initial Catalog=BD;Integrated Security=True"); //менять при установке
        public wDocumentAdding()
        {
            InitializeComponent();            
        }
        string filename;
        BDEntities db;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            D.dataD1 = GetMaxId().ToString();            
            new wWhoKnow().ShowDialog();            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = @"c:\"; //узнать необходимую директорию
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                filename = dialog.FileName;
                FileTxt.Text = filename;
            }
        }

        private int GetMaxId()
        {
            sqlConnection.Open(); //получение Id нового документа
            string querty = $"select max(Id) from Documents";
            SqlCommand command = new SqlCommand(querty, sqlConnection);
            object result = command.ExecuteScalar();
            int LastDocId = Convert.ToInt32(result);
            sqlConnection.Close();
            return LastDocId;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Document doc = new Document();
            doc.Index = IndexTxt.Text;
            doc.Name = NameDocTxt.Text;
            doc.Number_order = NumberOrderTxt.Text;
            doc.Doc_tipe_id = (TipeDocCombo.SelectedItem as Doc_tipe).Id;
            doc.Save_place_id = (SavePlaceCombo.SelectedItem as Save_place).Id;
            doc.Developer_id = (DeveloperCombo.SelectedItem as Department).Id;
            doc.Validity_period = ValidityPeriodPicker.DisplayDate;
            doc.Date_order = DateOrderPicker.DisplayDate;
            doc.Relevance = RelevanceCheck.IsChecked.Value;
            doc.Doc_file = filename;
            
            db.Documents.Add(doc);

            //добавление Who_know

            MessageBox.Show(GetMaxId().ToString());
            db.SaveChanges();
        }
        




        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            db = new BDEntities();
            
            List<string> aa = D.dataD2.Select(t => t.ToString()).ToList();
            
            WhoKnowTxt.Text = string.Join(" ", aa);
            
            TipeDocCombo.ItemsSource = db.Doc_tipe.ToList();
            SavePlaceCombo.ItemsSource = db.Save_place.ToList();
            DeveloperCombo.ItemsSource = db.Departments.ToList();
        }
    }
}
