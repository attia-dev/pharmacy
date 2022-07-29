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
    /// Interaction logic for category.xaml
    /// </summary>
    public partial class category : Window
    {
        DAL.category cat = new DAL.category();
        public category()
        {
            InitializeComponent();
            

        }

        void setData()
        {
            cat.name = txt_name.Text;
        }

        void getData()
        {
            txt_name.Text = cat.name;
        }
        void insert()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {
                setData();
                
                db.categories.InsertOnSubmit(cat);
                db.SubmitChanges();
                MessageBox.Show("تمت الاضافة");
                refreshDGV();

            }
        }
        void delete()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {

                db.categories.Attach(cat);
                db.categories.DeleteOnSubmit(cat);
                db.SubmitChanges();

                MessageBox.Show("تم الحذف");
                refreshDGV();

            }
        }

        void update()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {
                db.categories.Attach(cat);
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
                dgvCategory.ItemsSource = db.categories;
                dgvCategory.IsReadOnly = true;
                dgvCategory.FlowDirection = FlowDirection.RightToLeft;
                dgvCategory.Columns[0].Header = "م.";
                dgvCategory.Columns[1].Header = "الاسم";
            }
        }

        private void dgvCategory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            cat = (DAL.category)dgvCategory.SelectedItem;
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
            cat = new DAL.category();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshDGV();
        }
    }
}
