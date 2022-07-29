using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
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
using Image = System.Drawing.Image;

namespace pharmacy
{
    /// <summary>
    /// Interaction logic for item.xaml
    /// </summary>
    public partial class item : Window
    {
        DAL.item itm = new DAL.item();
        bool newItem;
        public item()
        {
            newItem = true;
            InitializeComponent();
        }
        public item(DAL.item gottenItm)
        {
            newItem = false;
            InitializeComponent();
            itm = gottenItm;
            getData();
            btn_save.Content = "تعديل";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {
                combCategory.ItemsSource = db.categories.Select(x => x.name);
                combUnit.ItemsSource = db.units.Select(x => x.name);
            }
           
        }

        bool setData()
        {
            itm.name = txt_name.Text;
            itm.type = combCategory.Text;
            itm.description = txt_description.Text;
            itm.itemUnit = combUnit.Text;
            try { itm.buyPrice = Convert.ToDouble(txt_buyPrice.Text); }
            catch 
            {
                MessageBox.Show("برجاء ادخال سعر الشراء بشكل صحيح");
                return false;
            }
            try { itm.sellPrice = Convert.ToDouble(txt_sellPrice.Text); }
            catch 
            {
                MessageBox.Show("برجاء ادخال سعر البيع بشكل صحيح");
                return false;
            }
            try { itm.sellDiscount = Convert.ToDouble(txt_sellDiscount.Text); }
            catch 
            {
                MessageBox.Show("برجاء ادخال خصم البيع بشكل صحيح");
                return false;
            }

            try { itm.count = Convert.ToDouble(txt_count.Text); }
            catch 
            {
                MessageBox.Show("برجاء ادخال الكمية بشكل صحيح");
                return false;
            }

            itm.barcode = txt_barcode.Text;
            if (imgPhoto.Source != null)
            { itm.image = imageToByte(); }
            return true;
        }

        void getData()
        {
            txt_name.Text = itm.name;
            combCategory.SelectedItem = itm.type;
            txt_description.Text = itm.description;
            combUnit.SelectedItem = itm.itemUnit;
            txt_buyPrice.Text = itm.buyPrice.ToString();
            txt_sellPrice.Text = itm.sellPrice.ToString();
            txt_sellDiscount.Text = itm.sellDiscount.ToString();
            txt_barcode.Text = itm.barcode;
            txt_count.Text = itm.count.ToString();
            if (itm.image != null)
            {
                imgPhoto.Source = ConvertToImage(itm.image);
            }
            
           

        }

        void insert()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {


                if (validateData())
                {
                    db.items.InsertOnSubmit(itm);
                    if (setData())
                    {
                        db.SubmitChanges();
                        MessageBox.Show("تمت الاضافة");
                    }

                }



            }
        }

        void update()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {


                if (validateData())
                {
                    db.items.Attach(itm);
                    if (setData())
                    {
                        db.SubmitChanges();
                        MessageBox.Show("تم التعديل");
                    }
                }



            }
        }

        bool validateData()
        {
            if (txt_name.Text.Trim() == string.Empty)
            {
                MessageBox.Show("برجاء ادخال الاسم");
                return false;
            }
            if (combCategory.Text.Trim() == string.Empty)
            {
                MessageBox.Show("برجاء اختيار الفئة");
                return false;
            }
            if (combUnit.Text.Trim() == string.Empty)
            {
                MessageBox.Show("برجاء اختيار الوحدة");
                return false;
            }
            return true;

        }




        void delete()
        {
            using (var db = new DAL.dbDataContext(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True"))
            {

                db.items.Attach(itm);
                db.items.DeleteOnSubmit(itm);
                db.SubmitChanges();

                MessageBox.Show("تم الحذف");


            }
            itm = new DAL.item();
            newItem = true;
            btn_save.Content = "اضافة";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (newItem)
                insert();
            else
                update();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            delete();

            refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            newItem = true;
            refresh();
            btn_save.Content = "اضافة";
        }

        void refresh()
        {
            txt_name.Text = "";
            combCategory.Text = "";
            txt_description.Text = "";
            combUnit.Text = "";
            txt_buyPrice.Text = "";
            txt_sellPrice.Text = "";
            txt_sellDiscount.Text = "";
            txt_barcode.Text = "";
            txt_count.Text = "";
            itm = new DAL.item();
        }

        private void btn_image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));

            }
        }


        byte[] imageToByte()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var bmp = imgPhoto.Source as BitmapImage;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(ms);
               return ms.ToArray();
            }
        }

      

        public BitmapImage ConvertToImage(System.Data.Linq.Binary binary)
        {
            byte[] buffer = binary.ToArray();
            MemoryStream stream = new MemoryStream(buffer);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //imgPhoto.Source = "pack://application:,,,/pharmacy;component/new-item-sign-stamp-on-260nw-1773071672.jpg";
        }
    }
}
