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
    /// Interaction logic for invoice.xaml
    /// </summary>
    public partial class invoice : Window
    {
        float qty, price, discount, subtotal , invTotal , invDiscount , invNetTotal , invPaid , invRemaining ;
        
        DAL.item selectedItem;
        DAL.invoice inv;
        DAL.invoiceDetail invDetail;

        List<DAL.invoiceDetail> shopCart = new List<DAL.invoiceDetail>();
        public invoice()
        {
            inv = new DAL.invoice();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            txt_subtotal.Text = "0";

            using (var db = new DAL.dbDataContext())
            {

               
                cbox.ItemsSource = db.items;
                cbox.DisplayMemberPath ="name";
                cbox.SelectedIndex = 1;
                dg_shopcart.ItemsSource = db.invoiceDetails.Where(x => x.invoiceID == 0);

               
            }



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (shopCart.Count > 0)
            {
                inv = new DAL.invoice();
                inv.dateAndTime = DateTime.Now;
                inv.userID = 1;
                inv.discount = invDiscount;
                inv.totalPrice = invTotal;
                inv.netPrice = invNetTotal;
                inv.paid = invPaid;
                inv.remaining = invRemaining;

                using (var db = new DAL.dbDataContext())
                {
                    db.invoices.InsertOnSubmit(inv);
                    db.invoiceDetails.InsertAllOnSubmit(shopCart);

                    db.SubmitChanges();
                    MessageBox.Show("تم الحفظ");
                    shopCart.RemoveAll(x=>x.id>=0);
                    dg_shopcart.ItemsSource = null;

                }

                invTotal = invDiscount = invNetTotal = invPaid = invRemaining = 0;
                txt_invTotal.Text = txt_invDiscount.Text = txt_invNetTotal.Text = txt_invPaid.Text = txt_invRemaining.Text = "0";


            }
            else
                MessageBox.Show("سلة البيع فارغة");

        }



        private void txt_qty_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculateSubtotal();
        }

        private void txt_invDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            float.TryParse(txt_invDiscount.Text,out invDiscount);
            invNetTotal = invTotal - invDiscount;
            txt_invNetTotal.Text = invNetTotal.ToString();
        }

        private void txt_paid_TextChanged(object sender, TextChangedEventArgs e)
        {
            float.TryParse(txt_invPaid.Text, out invPaid);
            invRemaining = invPaid- invNetTotal;
            txt_invRemaining.Text = invRemaining.ToString();
        }

        private void cbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                selectItem();
                qty = 1;
                discount = 0;
                txt_qty.Text = qty.ToString();
                txt_discount.Text = discount.ToString();

            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem != null)
            {
                using (var db = new DAL.dbDataContext())
                {
                    invDetail = new DAL.invoiceDetail();

                    invDetail.invoiceID = db.invoices.Count() < 1 ? 1 : db.invoices.Max(x => x.id) + 1;
                    invDetail.productID = db.items.Single(x => x.id == selectedItem.id).id;
                    invDetail.productQuantity = qty;
                    invDetail.discount = discount;
                    invDetail.totalPrice = subtotal;

                    shopCart.Add(invDetail);
                    invTotal=invNetTotal =(float) shopCart.Sum(x => x.totalPrice);
                    txt_invTotal.Text =txt_invNetTotal.Text= invTotal.ToString();

                    float.TryParse(txt_invDiscount.Text, out invDiscount);
                    invNetTotal = invTotal - invDiscount;
                    txt_invNetTotal.Text = invNetTotal.ToString();

                    dg_shopcart.ItemsSource = null;
                    dg_shopcart.ItemsSource = shopCart.Select(x => new {
                        index = shopCart.IndexOf(x) + 1,
                        name = db.items.Single(i => i.id == x.productID).name,
                        unitName = db.items.Single(i => i.id == x.productID).itemUnit,
                        quantity = x.productQuantity,
                        discount = x.discount,
                        totalPrice = x.totalPrice

                    });
                    dg_shopcart.Columns[0].Header = "م.";
                    dg_shopcart.Columns[1].Header = "اسم الصنف";
                    dg_shopcart.Columns[2].Header = "الوحدة";
                    dg_shopcart.Columns[3].Header = "الكمية";
                    dg_shopcart.Columns[4].Header = "الخصم";
                    dg_shopcart.Columns[5].Header = "سعر الكمية";


                }


                selectedItem = null;
                cbox.Text = "";
            }
            else MessageBox.Show("empty");
        }

        private void cbox_DropDownClosed(object sender, EventArgs e)
        {
            selectItem();
            qty = 1;
            discount = 0;
            txt_qty.Text = qty.ToString();
            txt_discount.Text = discount.ToString();
            

        }

        private void dg_shopcart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var db = new DAL.dbDataContext();
            if (dg_shopcart.SelectedItem != null)
            {
                shopCart.RemoveAt(dg_shopcart.SelectedIndex);
                invTotal = invNetTotal = (float)shopCart.Sum(x => x.totalPrice);
                txt_invTotal.Text = txt_invNetTotal.Text = invTotal.ToString();

                float.TryParse(txt_invDiscount.Text, out invDiscount);
                invNetTotal = invTotal - invDiscount;
                txt_invNetTotal.Text = invNetTotal.ToString();

                dg_shopcart.ItemsSource = null;

                if (shopCart.Count > 0)
                {
                    dg_shopcart.ItemsSource = shopCart.Select(x => new {

                        index = shopCart.IndexOf(x) + 1,
                        name = db.items.Single(i => i.id == x.productID).name,
                        unitName = db.items.Single(i => i.id == x.productID).itemUnit,
                        quantity = x.productQuantity,
                        discount = x.discount,
                        totalPrice = x.totalPrice

                    });
                    dg_shopcart.Columns[0].Header = "م.";
                    dg_shopcart.Columns[1].Header = "اسم الصنف";
                    dg_shopcart.Columns[2].Header = "الوحدة";
                    dg_shopcart.Columns[3].Header = "الكمية";
                    dg_shopcart.Columns[4].Header = "الخصم";
                    dg_shopcart.Columns[5].Header = "سعر الكمية";
                }
            }
        }

        private void cbox_DropDownOpened(object sender, EventArgs e)
        {
            var db = new DAL.dbDataContext();
            cbox.ItemsSource = db.items.Where(x => x.name.Contains(cbox.Text));
            cbox.DisplayMemberPath = "name";
        }

        

        private void txt_discount_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculateSubtotal();
        }

      

        void selectItem()
        {
            
            var db = new DAL.dbDataContext();
            if (db.items.Where(x => x.name == cbox.Text).Count() > 0)

            // MessageBox.Show(cbox.Text + " هذه العنصر غير موجود");

            {
                selectedItem = (DAL.item)cbox.SelectedItem;
                txt_unit.Text = selectedItem.itemUnit.ToString();
                txt_price.Text = selectedItem.sellPrice.ToString();

                
                cbox.ItemsSource = db.items;
                cbox.DisplayMemberPath = "name";
                if (selectedItem != null)
                    cbox.Text = selectedItem.name;


                calculateSubtotal();
            }
            else if (db.items.Where(x => x.barcode == cbox.Text).Count() > 0)
            {
                selectedItem = db.items.Single(x => x.barcode == cbox.Text);
                cbox.Text = db.items.Single(x => x.barcode == cbox.Text).name;
                txt_unit.Text = selectedItem.itemUnit.ToString();
                txt_price.Text = selectedItem.sellPrice.ToString();

                cbox.ItemsSource = db.items;
                cbox.DisplayMemberPath = "name";
                if (selectedItem != null)
                    cbox.Text = selectedItem.name;

                calculateSubtotal();
            }
            else
                return;
            
        }

        void calculateSubtotal()
        {
            float.TryParse(txt_qty.Text, out qty);
            float.TryParse(txt_discount.Text, out discount);

            if (selectedItem != null)
            {
                subtotal = (float)(qty * selectedItem.sellPrice - discount);
                txt_subtotal.Text = subtotal.ToString();
            }
            
        }

        
    }
}
