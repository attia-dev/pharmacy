using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace pharmacy
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }

    

     


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            users usersPage = new users();
            usersPage.ShowDialog();


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            invoice inv = new invoice();
            inv.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            item itm = new item();
            itm.ShowDialog();
        }

        private void ListBox_CleanUpVirtualizedItem(object sender, CleanUpVirtualizedItemEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            category cat = new category();
            cat.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            unit un = new unit();
            un.ShowDialog();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            itemsList itmList = new itemsList();
            itmList.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!CheckDatabaseExist())
           {
                GenerateDatabase();
            } 
        }

        private bool CheckDatabaseExist()
        {
            SqlConnection Connection = new SqlConnection(@"Data Source=.\;Initial Catalog=pharmacy;Integrated Security=True ");
            //connetionString = @"Data Source=WIN-50GP30FGO75;Initial Catalog=Demodb;User ID=sa;Password=demol23";

            //Data Source=.\sqlexpress;Initial Catalog=master;Integrated Security=True
            try
            {
                Connection.Open();
                MessageBox.Show("database found and opened");
                Connection.Close();
                return true;
               
            }
            catch
            {
                MessageBox.Show("database not found");
                return false;
            }

        }

        private void GenerateDatabase()
        {
            List<string> cmds = new List<string>();
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Script.sql"))
            {
                TextReader tr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Script.sql");
                string line = "";
                string cmd = "";
                while ((line = tr.ReadLine()) != null)
                {
                    if (line.Trim().ToUpper() == "GO")
                    {
                        cmds.Add(cmd);
                        cmd = "";
                    }
                    else
                    {
                        cmd += line + "\r\n";
                    }
                }
                if (cmd.Length > 0)
                {
                    cmds.Add(cmd);
                    cmd = "";
                }
                tr.Close();
            }
            if (cmds.Count > 0)
            {
                SqlCommand command = new SqlCommand();
                command.Connection = new SqlConnection(@"Data Source=.\;Initial Catalog=master;Integrated Security=True ");
                //connetionString = @"Data Source=WIN-50GP30FGO75;Initial Catalog=Demodb;User ID=sa;Password=demol23";

                command.CommandType = System.Data.CommandType.Text;
                command.Connection.Open();
                MessageBox.Show("database established");

                for (int i = 0; i < cmds.Count; i++)
                {
                    command.CommandText = cmds[i];
                    command.ExecuteNonQuery();
                }
            }
           
        }
    }
}
