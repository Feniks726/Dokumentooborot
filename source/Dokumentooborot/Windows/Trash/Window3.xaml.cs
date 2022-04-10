using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Dokumentooborot.DAL;

namespace Dokumentooborot.Windows.Trash
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    ///     public class AuhorExtra
    public class RoleExtra
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public RoleExtra(int Id, string Name)
        {
            RoleId = Id;
            RoleName = Name;
        }
    }

    public partial class Window3 : Window  //нужно сделать комбобокс на роль в датагриде
    {
        BDEntities db;

        public Window3()
        {
            InitializeComponent();
            RolesExtra = new List<Role>();
        }
        public List<Role> RolesExtra { get; private set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new BDEntities();
            dgogrenci.ItemsSource = db.Users.ToList();
            RolesExtra.AddRange(db.Roles.ToList());                        
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Window10().ShowDialog();
        }

        private void dgogrenci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (MessageBox.Show("Удалить запись?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    db.Users.Remove((User)dgogrenci.SelectedItem);
                    db.SaveChanges();                    
                    dgogrenci.ItemsSource = db.Users.ToList();
                }
            }                
        }
    }
}