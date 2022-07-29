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

namespace pharmacy
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class users : Window
    {
        DAL.account acc = new DAL.account();
        public users()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {
                acc.username = txt_username.Text;
                acc.password = txt_password.Text;
                db.accounts.InsertOnSubmit(acc);
                db.SubmitChanges();
                MessageBox.Show("تمت الاضافة");
                refreshDGV();

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {

                db.accounts.Attach(acc);
                db.accounts.DeleteOnSubmit(acc);
                db.SubmitChanges();

                MessageBox.Show("تم الحذف");
                refreshDGV();

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {
                db.accounts.Attach(acc);
                acc.username = txt_username.Text;
                acc.password = txt_password.Text;
                db.SubmitChanges();
                MessageBox.Show("تم التعديل");
                refreshDGV();


            }
        }

        private void dgv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            acc = (DAL.account)dgv.SelectedItem;
            txt_username.Text = acc.username;
            txt_password.Text = acc.password;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshDGV();
        }

        void refreshDGV()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {
                dgv.ItemsSource = db.accounts;
                dgv.IsReadOnly = true;
                dgv.FlowDirection = FlowDirection.RightToLeft;
                dgv.Columns[0].Header = "م.";
                dgv.Columns[1].Header = "الاسم";
                dgv.Columns[2].Header = "الباسورد";
                dgv.Columns[3].Header = "الحساب";
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            txt_username.Text = "";
            txt_password.Text = "";
            acc = new DAL.account();
        }
    }
}
