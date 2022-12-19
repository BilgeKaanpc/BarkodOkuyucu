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

                DatabaseHelper.DataAdd(maskedTextBox4, textBox2, maskedTextBox2, maskedTextBox3, maskedTextBox1,radioButton1,radioButton2,radioButton3);
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

        public static bool checkBarkod(string barkod)
        {
            SQLiteCommand control = new SQLiteCommand("select * from urunler",Baglan.connection);

            SQLiteDataReader test = control.ExecuteReader();

            while (test.Read())
            {
                if (test["barkod"].ToString() == barkod)
                {
                    return false;
                }
            }
            return true;
        }
        public static void DataAdd(MaskedTextBox t1, TextBox t2, MaskedTextBox t3, MaskedTextBox t5, MaskedTextBox t4,RadioButton r1,RadioButton r2,RadioButton r3)
        {
            Baglan.connection.Open();

            SQLiteCommand ekle = new SQLiteCommand("insert into urunler (barkod,isim,satis,alis,stok,tur) values (@k1,@k2,@k3,@k4,@k5,@k6)", Baglan.connection);

             bool canAdd = checkBarkod(t1.Text);
            if (canAdd)
            {

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

                if (r1.Checked)
                {
                    ekle.Parameters.AddWithValue("@k6", "Alkol");
                    r1.Checked = false;
                }
                else if (r2.Checked)
                {
                    ekle.Parameters.AddWithValue("@k6", "Bira");
                    r2.Checked = false;
                }
                else if (r3.Checked)
                {
                    ekle.Parameters.AddWithValue("@k6", "Sigara");
                    r3.Checked = false;
                }
                else
                {
                    ekle.Parameters.AddWithValue("@k6", "Diğer");
                }

                ekle.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Ürün Zaten Eklenmiş");
            }


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

        public static void DataUpdate(string valueSatis,string valueAlis,string barkod)
        {
            Baglan.connection.Open();

            SQLiteCommand update = new SQLiteCommand("Update urunler set satis= '" + valueSatis + " ' where barkod = " + barkod , Baglan.connection);
            SQLiteCommand updatealis = new SQLiteCommand("Update urunler set alis= '" + valueAlis + " ' where barkod = " + barkod, Baglan.connection);
            update.ExecuteNonQuery();
            updatealis.ExecuteNonQuery();
            Baglan.connection.Close();

            
        }

        public static SQLiteDataReader getData(string barkod)
        {
            SQLiteCommand get = new SQLiteCommand("Select * from urunler where barkod = "+barkod,Baglan.connection);
            SQLiteDataReader drm = get.ExecuteReader();

            return drm;
        }

        public static void stokEkle(int value,string barkod)
        {
            Baglan.connection.Open();
            SQLiteCommand update = new SQLiteCommand("Update urunler set stok= '" + value.ToString() + " ' where barkod = " + barkod, Baglan.connection);
            update.ExecuteNonQuery();
            Baglan.connection.Close();
        }

        public static void satisEkle(string tur,string total,string kar,string urunler)
        {
            Baglan.connection.Open();
            SQLiteCommand ekle = new SQLiteCommand("insert into satislar (tur,total,kar,urunler,tarih) values (@k1,@k2,@k3,@k4,@k5)", Baglan.connection);
            ekle.Parameters.AddWithValue("@k1", tur);
            ekle.Parameters.AddWithValue("@k2", total);
            ekle.Parameters.AddWithValue("@k3", kar);
            ekle.Parameters.AddWithValue("@k4", urunler);
            ekle.Parameters.AddWithValue("@k5", DateTime.Now.ToString());
            ekle.ExecuteNonQuery();

            Baglan.connection.Close();
        }

        public static int veresiyeName;
        public static float borc;
        public static bool checkVeresiye(string isim)
        {
            SQLiteCommand control = new SQLiteCommand("select * from veresiye", Baglan.connection);

            SQLiteDataReader test = control.ExecuteReader();

            while (test.Read())
            {
                if (test["isim"].ToString() == isim)
                {
                    veresiyeName = int.Parse(test["kisiID"].ToString());
                    borc = float.Parse(test["ToplamBorc"].ToString());
                    return true;
                }
            }
            return false;
        }
        public static void veresiyeEkle(string isim, float borcu,float newValue,string tur, string total, string kar, string urunler)
        {
            Baglan.connection.Open();

            bool exist = checkVeresiye(isim);
            if (exist)
            {
                borc = borc + newValue;
                SQLiteCommand updateBorc = new SQLiteCommand("Update veresiye Set ToplamBorc = '" + borc.ToString() + " ' Where kisiID = " + veresiyeName, Baglan.connection);
                updateBorc.ExecuteNonQuery();
            }
            else
            {

                SQLiteCommand ekle = new SQLiteCommand("insert into veresiye (isim,ToplamBorc) values (@k1,@k2)", Baglan.connection);
                ekle.Parameters.AddWithValue("@k1", isim);
                ekle.Parameters.AddWithValue("@k2", newValue.ToString());

                ekle.ExecuteNonQuery();
            }


            SQLiteCommand satisekle = new SQLiteCommand("insert into satislar (tur,total,kar,urunler,tarih) values (@k1,@k2,@k3,@k4,@k5)", Baglan.connection);
            satisekle.Parameters.AddWithValue("@k1", tur);
            satisekle.Parameters.AddWithValue("@k2", total);
            satisekle.Parameters.AddWithValue("@k3", kar);
            satisekle.Parameters.AddWithValue("@k4", urunler);
            satisekle.Parameters.AddWithValue("@k5", DateTime.Now.ToString());
            satisekle.ExecuteNonQuery();

            Baglan.connection.Close();
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
