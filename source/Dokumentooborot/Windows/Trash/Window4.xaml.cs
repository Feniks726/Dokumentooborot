using System;
using System.Linq;
using System.Windows;
using Dokumentooborot.DAL;

namespace Dokumentooborot.Windows.Trash
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        string filename;
        BDEntities db;
        public Window4()
        {
            InitializeComponent();
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {                        
            this.Hide();
            new Window5().ShowDialog();
            this.Show();
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int dataDId = Convert.ToInt32(D.dataD1);
            var uRow = db.Documents.Where(w => w.Id == dataDId).FirstOrDefault();

            if (IndexTxt.Text != null)
            {
                uRow.Index = IndexTxt.Text;

                if (NameDocTxt != null)
                {
                    uRow.Name = NameDocTxt.Text;
                    if (NumberOrderTxt != null)
                    {
                        uRow.Number_order = NumberOrderTxt.Text;                        
                        if (DateOrderPicker.Text != null && ValidityPeriodPicker != null)
                        {
                            if(DateOrderPicker.DisplayDate < ValidityPeriodPicker.DisplayDate)
                            {
                                uRow.Date_order = DateOrderPicker.DisplayDate;
                                uRow.Validity_period = ValidityPeriodPicker.DisplayDate;

                                uRow.Doc_file = FileTxt.Text;

                                uRow.Save_place_id = (SavePlaceCombo.SelectedItem as Save_place).Id;
                                uRow.Doc_tipe_id = (TipeDocCombo.SelectedItem as Doc_tipe).Id;
                                uRow.Developer_id = (DeveloperCombo.SelectedItem as Department).Id;
                                //whoknow
                                db.SaveChanges();
                                                                
                            }                            
                        }
                    }
                }
            }


        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new BDEntities();
            int dataDId = Convert.ToInt32(D.dataD1);
            var docData = db.Documents.Where(w => w.Id == dataDId).FirstOrDefault();


            IndexTxt.Text = docData.Index;
            NumberOrderTxt.Text = docData.Number_order;
            NameDocTxt.Text = docData.Name;
            FileTxt.Text = docData.Doc_file;            
            RelevanceCheck.IsChecked = docData.Relevance;

            string who = "";
            foreach (Who_know item in db.Who_know.ToList())
            {
                if (D.dataD1 == item.Document_id.ToString())
                {                                        
                    
                    who = string.Concat(who, item.Department.Name + ", ");
                }                
            }
            if (who.Length > 5)
                who = who.Remove(who.Length - 2);

            WhoKnowTxt.Text = who;

            DateOrderPicker.Text = docData.Date_order.ToString();
            ValidityPeriodPicker.Text = docData.Validity_period.ToString();
            
            SavePlaceCombo.ItemsSource = db.Save_place.ToList();
            SavePlaceCombo.SelectedIndex = docData.Save_place_id - 1;

            TipeDocCombo.ItemsSource = db.Doc_tipe.ToList();
            TipeDocCombo.SelectedIndex = docData.Doc_tipe_id - 1;

           
            DeveloperCombo.ItemsSource = db.Departments.ToList();
            DeveloperCombo.SelectedIndex = docData.Developer_id - 1;
        }
    }    
}
