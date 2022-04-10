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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dokumentooborot
{
    //public partial class ActiveUser
    //{
    //    public static int userid;
    //    public static string Фамилия;
    //    public static string Имя;
    //    public static string Отчество;
    //    public static int foreverything;
    //}
    static class D
    {
        public static string dataD1;
        public static List<int> dataD2 = new List<int>();        
    }
    public partial class MainWindow : Window
    {
        BDEntities db;
        public MainWindow()
        {
            InitializeComponent();
            db = new BDEntities();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "" || PasswordBox.Password == "")
                MessageBox.Show("Error: пустые поля");

            if (db.Users.Select(item => item.Login + " " + item.Password + " " + item.Role_id.ToString()).Contains(LoginBox.Text + " " + PasswordBox.Password + " " + "1"))
            {
                //User fuser = new User();
                //fuser = db.Users.Where(w => w.Login == LoginBox.Text && w.Password == PasswordBox.Password).FirstOrDefault();
                //ActiveUser.userid = fuser.Id;
                //ActiveUser.Имя = fuser.Имя;
                //ActiveUser.Фамилия = fuser.Фамилия;
                //ActiveUser.Отчество = fuser.Отчество;

                Window1 taskWin = new Window1();
                taskWin.Show();
                this.Close();
            }
            else if (db.Users.Select(item => item.Login + " " + item.Password + " " + item.Role_id.ToString()).Contains(LoginBox.Text + " " + PasswordBox.Password + " " + 2))
            {
                //Users fuser = new Users();
                //fuser = db.Users.Where(w => w.Login == LoginBox.Text && w.Password == PasswordBox.Password).FirstOrDefault();
                //ActiveUser.userid = fuser.Id;
                //ActiveUser.Имя = fuser.Имя;
                //ActiveUser.Фамилия = fuser.Фамилия;
                //ActiveUser.Отчество = fuser.Отчество;

                Window2 taskWindow = new Window2();
                taskWindow.Show();
                this.Close();
            }
            else
                MessageBox.Show("Error: Неверный Login или Password");
        }
    }
}
