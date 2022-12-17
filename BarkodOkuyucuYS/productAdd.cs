using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
namespace BarkodOkuyucuYS
{
    public partial class productAdd : Form
    {
        public productAdd()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

      

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if(maskedTextBox4.Text.Length < 1 || textBox2.Text.Length<1 ||maskedTextBox3.Text.Length<1)
            {

                if (maskedTextBox4.Text.Length < 1)
                {
                    MessageBox.Show("Ürünün Barkodunu Girmediniz!");
                }
                if (textBox2.Text.Length < 1)
                {
                    MessageBox.Show("Ürünün Adını Girmediniz!");
                }
                if (maskedTextBox3.Text.Length < 1)
                {
                    MessageBox.Show("Ürünün Satış Fiyatını Girmediniz!");
                }


            }
            else
            {

                DatabaseHelper.DataAdd(maskedTextBox4, textBox2, maskedTextBox2, maskedTextBox3, maskedTextBox1);
            }


        }

        private void productAdd_Load(object sender, EventArgs e)
        {
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
        
    }
    public class DatabaseHelper
    {

        static DataTable dt;
        public static void DataAdd(MaskedTextBox t1, TextBox t2, MaskedTextBox t3, MaskedTextBox t5, MaskedTextBox t4)
        {
            Baglan.connection.Open();

            SQLiteCommand ekle = new SQLiteCommand("insert into urunler (barkod,isim,satis,alis,stok) values (@k1,@k2,@k3,@k4,@k5)", Baglan.connection);


            ekle.Parameters.AddWithValue("@k1", t1.Text);
            ekle.Parameters.AddWithValue("@k2", t2.Text);
            ekle.Parameters.AddWithValue("@k4", t3.Text);

            if (t5.Text.Length > 1)
            {

                ekle.Parameters.AddWithValue("@k3", t5.Text);
            }
            else
            {
                ekle.Parameters.AddWithValue("@k3", "");
            }
            if (t4.Text.Length > 1)
            {

                ekle.Parameters.AddWithValue("@k5", int.Parse(t4.Text));
            }
            else
            {

                ekle.Parameters.AddWithValue("@k5", 0);
            }
            ekle.ExecuteNonQuery();

            Baglan.connection.Close();
            t1.Clear();
            t2.Clear();
            t3.Clear();
            t4.Clear();
            t5.Clear();
        }

        public static DataTable Listele(string sql)
        {
            dt = new DataTable();
            SQLiteDataAdapter adtr = new SQLiteDataAdapter(sql, Baglan.connection);
            adtr.Fill(dt);
            return dt;
        }
    }
    public class Baglan
    {

        static string fileName = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.ApplicationData), "veritabani.db");
        static string sqliteConnection = string.Format("Data Source={0};Version=3;", fileName);
        static string path = Directory.GetCurrentDirectory().ToString();
        static public string newPath = path.Replace(@"\", "/");

        public static SQLiteConnection connection = new SQLiteConnection("Data source=" + newPath + "/veritabani.db;Versiyon=3");


    }
}
