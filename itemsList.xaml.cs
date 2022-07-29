using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for itemsList.xaml
    /// </summary>
    public partial class itemsList : Window
    {
       
        public itemsList()
        {
            InitializeComponent();
        }

        void refreshDGV()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {
                
                DGV.ItemsSource = db.items;
                DGV.FlowDirection = FlowDirection.RightToLeft;
                DGV.IsReadOnly = true;
                DGV.Columns[0].Header = "م.";
                DGV.Columns[1].Header = "الاسم";
                DGV.Columns[2].Header = "الفئة";
                DGV.Columns[3].Header = "الوصف";
                DGV.Columns[4].Header = "الحالة";
                DGV.Columns[5].Header = "الصورة";
                DGV.Columns[6].Header = "اسم الوحدة";
                DGV.Columns[7].Header = "سعر الشراء";
                DGV.Columns[8].Header = "سعر البيع";
                DGV.Columns[9].Header = "خصم البيع";
                DGV.Columns[10].Header = "الباركود";
                DGV.Columns[11].Header = "الكمية";
                DGV.Columns[3].Visibility = Visibility.Hidden;
                DGV.Columns[4].Visibility = Visibility.Hidden;
                DGV.Columns[5].Visibility = Visibility.Hidden;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            refreshDGV();
        }

        private void DGV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            item itm = new item((DAL.item)DGV.SelectedItem);
            itm.ShowDialog();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {
                DGV.ItemsSource = db.items.Where(x => x.name.Contains(searchBox.Text));
                DGV.FlowDirection = FlowDirection.RightToLeft;
                DGV.IsReadOnly = true;
                DGV.Columns[0].Header = "م.";
                DGV.Columns[1].Header = "الاسم";
                DGV.Columns[2].Header = "الفئة";
                DGV.Columns[3].Header = "الوصف";
                DGV.Columns[4].Header = "الحالة";
                DGV.Columns[5].Header = "الصورة";
                DGV.Columns[6].Header = "اسم الوحدة";
                DGV.Columns[7].Header = "سعر الشراء";
                DGV.Columns[8].Header = "سعر البيع";
                DGV.Columns[9].Header = "خصم البيع";
                DGV.Columns[10].Header = "الباركود";
                DGV.Columns[11].Header = "الكمية";
                DGV.Columns[3].Visibility = Visibility.Hidden;
                DGV.Columns[4].Visibility = Visibility.Hidden;
                DGV.Columns[5].Visibility = Visibility.Hidden;
            }
        }
    }
}
