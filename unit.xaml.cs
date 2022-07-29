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

namespace pharmacy
{
    /// <summary>
    /// Interaction logic for unit.xaml
    /// </summary>
    public partial class unit : Window
    {
        DAL.unit un = new DAL.unit();
        public unit()
        {
            InitializeComponent();
        }

        void setData()
        {
            un.name = txt_name.Text;
        }

        void getData()
        {
            txt_name.Text = un.name;
        }

        void insert()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True "))
            {
                setData();

                db.units.InsertOnSubmit(un);
                db.SubmitChanges();
                MessageBox.Show("تمت الاضافة");
                refreshDGV();

            }
        }

        void delete()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {

                db.units.Attach(un);
                db.units.DeleteOnSubmit(un);
                db.SubmitChanges();

                MessageBox.Show("تم الحذف");
                refreshDGV();

            }
        }

        void update()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {
                db.units.Attach(un);
                setData();

                db.SubmitChanges();
                MessageBox.Show("تم التعديل");
                refreshDGV();


            }
        }

        void refreshDGV()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {
                dgvUnit.ItemsSource = db.units;
                dgvUnit.IsReadOnly = true;
                dgvUnit.FlowDirection = FlowDirection.RightToLeft;
                dgvUnit.Columns[0].Header = "م.";
                dgvUnit.Columns[1].Header = "الاسم";
            }
        }

        private void dgvUnit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            un = (DAL.unit)dgvUnit.SelectedItem;
            getData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            insert();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            delete();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            update();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            txt_name.Text = "";
            un = new DAL.unit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshDGV();
        }
    }
}
