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
    public partial class Form1 : Form
    {
        public Form2 islemlerForm;
        veresiye veresiye;
        public nakitsatis nakitSatis;

        public Form1()
        {
            InitializeComponent();

            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            islemlerForm = new Form2();
            islemlerForm.Show();
        }

       public DataTable satisTablosu = new DataTable();
        private BindingSource bindingSource1 = new BindingSource();
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            satisTablosu.Columns.Add("Barkod", typeof(string));
            satisTablosu.Columns.Add("Tür", typeof(string));
            satisTablosu.Columns.Add("Isim", typeof(string));
            satisTablosu.Columns.Add("Fiyat", typeof(string));
            satisTablosu.Columns.Add("Stok Bilgisi", typeof(int));
            satisTablosu.Columns.Add("Adet", typeof(int));
            satisTablosu.Columns.Add("Toplam Fiyat", typeof(string));
            bindingSource1.DataSource = satisTablosu;
            dataGridView1.DataSource = bindingSource1;
            DataGridViewColumn column = dataGridView1.Columns[0];
            DataGridViewColumn column1 = dataGridView1.Columns[1];
            DataGridViewColumn column2 = dataGridView1.Columns[2];
            DataGridViewColumn column3 = dataGridView1.Columns[3];
            DataGridViewColumn column4 = dataGridView1.Columns[4];
            DataGridViewColumn column5 = dataGridView1.Columns[5];
            column.Width = 70;
            column1.Width = 70;
            column2.Width = 400;
            column3.Width = 40;
            column4.Width = 40;
            column5.Width = 40;
            textBox1.Focus();

        }
        public void fiyatAdd(int index, int adet)
        {
            

                string Fiyat = satisTablosu.Rows[index]["Fiyat"].ToString();

                float netDeger = float.Parse(Fiyat);


                satisTablosu.Rows[index]["Toplam Fiyat"] = netDeger * adet;
           
        }
        public float toplamTutar = 0;
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        public int control(SQLiteDataReader drm)
        {
            for (int i = 0; i < satisTablosu.Rows.Count; i++)
            {
                if (satisTablosu.Rows[i]["Barkod"].ToString() == textBox1.Text)
                {
                    string adet = satisTablosu.Rows[i]["Adet"].ToString();
                    int newAdet = int.Parse(adet) + 1;
                    satisTablosu.Rows[i]["Adet"] = newAdet;

                    fiyatAdd(i, newAdet);
                    return -1;
                }
                if(satisTablosu.Rows[i]["Adet"].ToString() == "0")
                {
                    satisTablosu.Rows.RemoveAt(i);
                }
            }

            DataRow dr = satisTablosu.NewRow();
            dr["Barkod"] = drm["barkod"].ToString();
            dr["Isim"] = drm["isim"].ToString();
            dr["Fiyat"] = drm["satis"].ToString();
            dr["Stok Bilgisi"] = drm["stok"].ToString();
            dr["Tür"] = drm["tur"].ToString();
            dr["Adet"] = 1;

            dr["Toplam Fiyat"] = drm["satis"].ToString();
            satisTablosu.Rows.Add(dr);


            return 1;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Baglan.connection.Open();
           SQLiteDataReader drm = DatabaseHelper.getData(textBox1.Text);
            while (drm.Read())
            {

                control(drm);

            }
            toplamTutar = 0;
            for (int i = 0; i < satisTablosu.Rows.Count; i++)
            {

                string urunFiyati = satisTablosu.Rows[i]["Toplam Fiyat"].ToString();
                if(urunFiyati != "")
                {

                    float tutar = float.Parse(urunFiyati);
                    toplamTutar = toplamTutar + tutar;
                }

            }
            label3.Text = toplamTutar.ToString();
            Baglan.connection.Close();

            textBox1.Clear();
            textBox1.Focus();
        }

        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {

            string adet = satisTablosu.Rows[e.RowIndex]["Adet"].ToString();
            if(int.Parse(adet) == 0)
            {
                satisTablosu.Rows.RemoveAt(e.RowIndex);

                bindingSource1.DataSource = satisTablosu;
                dataGridView1.DataSource = bindingSource1;
            }
            else
            {
                int newAdet = int.Parse(adet);
                fiyatAdd(e.RowIndex, newAdet);
            }

            
            toplamTutar = 0;
            for (int i = 0; i < satisTablosu.Rows.Count; i++)
            {

                string urunFiyati = satisTablosu.Rows[i]["Toplam Fiyat"].ToString();
                if(urunFiyati != "")
                {

                    float tutar = float.Parse(urunFiyati);
                    toplamTutar = toplamTutar + tutar;
                }

            }
            label3.Text = toplamTutar.ToString();
            textBox1.Focus();
        }
        public void ClearAll()
        {
            satisTablosu.Rows.Clear();
            label3.Text = "000";
            toplamTutar = 0;

        }
      
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button2_Click(sender,e);
                textBox1.Clear();
                textBox1.Focus();
            }
        }
        public void stokDusus()
        {
            for(int i = 0; i < satisTablosu.Rows.Count; i++)
            {
                if (satisTablosu.Rows[i]["Tür"].ToString() == "Sigara" || satisTablosu.Rows[i]["Tür"].ToString() == "Bira"|| satisTablosu.Rows[i]["Tür"].ToString() == "Alkol")
                {
                    int value = 0;
                    Baglan.connection.Open();
                    SQLiteCommand search = new SQLiteCommand("Select * From urunler where barkod = " + satisTablosu.Rows[i]["Barkod"].ToString(), Baglan.connection);
                    SQLiteDataReader drm = search.ExecuteReader();
                    while (drm.Read())
                    {
                        value = int.Parse(drm["stok"].ToString()) - int.Parse(satisTablosu.Rows[i]["Adet"].ToString());
                    }

                    Baglan.connection.Close();
                    DatabaseHelper.stokEkle(value, satisTablosu.Rows[i]["Barkod"].ToString());
                }
            }

        }
        public string urunlerList()
        {
            string urunler = "";

            for(int i = 0; i < satisTablosu.Rows.Count; i++)
            {
                urunler = urunler + "\n  " + satisTablosu.Rows[i]["Isim"] + " - " + satisTablosu.Rows[i]["Adet"] + " - " + satisTablosu.Rows[i]["Toplam Fiyat"];
            }


            return urunler;
        }
        float kar = 0;
        public void karHesapla()
        {
            kar = (toplamTutar * 9) / 100;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (toplamTutar == 0)
            {
                MessageBox.Show("Ürün Yok!");
                return;
            }
            stokDusus();
            nakitSatis = new nakitsatis();
            nakitSatis.tutar = toplamTutar;
            nakitSatis.urunList = urunlerList();
            karHesapla();
            nakitSatis.kar = kar;
            nakitSatis.Show();
            ClearAll();
        }
        public bool checkUrun(string date)
        {
            SQLiteCommand control = new SQLiteCommand("select * from gunsonu", Baglan.connection);

            SQLiteDataReader test = control.ExecuteReader();

            while (test.Read())
            {
                if (test["tarih"].ToString() == date)
                {
                    return true;
                }
            }
            return false;
        }
        public void gunSonu()
        {
            Baglan.connection.Open();
            int toplamSatis = 0;
            float toplamtutarim = 0;
            float toplamkar = 0;
            float nakit = 0;
            float kart = 0;
            float veresiye = 0;

            DataTable test = new DataTable();
            SQLiteDataAdapter adtr = new SQLiteDataAdapter("select * from satislar where gunsonu = 0", Baglan.connection);
            adtr.Fill(test);
            for(int i = 0; i < test.Rows.Count; i++)
            {

                bool isExist = checkUrun(test.Rows[i]["tarih"].ToString().Split()[0]);
                if (isExist)
                {
                    DataTable dtr2 = new DataTable();
                    SQLiteDataAdapter gunsonu = new SQLiteDataAdapter("select * from gunsonu where tarih Like '%" + test.Rows[i]["tarih"].ToString().Split()[0] + "%'", Baglan.connection);
                    gunsonu.Fill(dtr2);


                   
                        toplamSatis = int.Parse(dtr2.Rows[0]["toplamsatis"].ToString());
                        toplamtutarim = float.Parse(dtr2.Rows[0]["toplamtutar"].ToString());
                        toplamkar = float.Parse(dtr2.Rows[0]["toplamkar"].ToString());
                        nakit = float.Parse(dtr2.Rows[0]["nakit"].ToString());
                        kart = float.Parse(dtr2.Rows[0]["kart"].ToString());
                        veresiye = float.Parse(dtr2.Rows[0]["veresiye"].ToString());
                      

                    toplamSatis++;
                    toplamtutarim = toplamtutarim + float.Parse(test.Rows[i]["total"].ToString());
                    toplamkar = toplamkar + float.Parse(test.Rows[i]["kar"].ToString());
                    if (test.Rows[i]["tur"].ToString() == "Nakit")
                    {
                        nakit = nakit + float.Parse(test.Rows[i]["total"].ToString());
                    }
                    if (test.Rows[i]["tur"].ToString() == "Kart")
                    {
                        kart = kart + float.Parse(test.Rows[i]["total"].ToString());
                    }
                    if (test.Rows[i]["tur"].ToString().StartsWith("Vere"))
                    {
                        veresiye = veresiye + float.Parse(test.Rows[i]["total"].ToString());
                    }

                    SQLiteCommand updateSatis = new SQLiteCommand("Update gunsonu set toplamsatis= '" + toplamSatis.ToString() + " ',toplamtutar= '" + toplamtutarim.ToString() + " ',toplamkar= '" + toplamkar.ToString() + " ',nakit= '" + nakit.ToString() + " ',kart= '" + kart.ToString() + " ',veresiye= '" + veresiye.ToString() + " ' where tarih Like '%" + test.Rows[i]["tarih"].ToString().Split()[0] + "%'", Baglan.connection);

                    updateSatis.ExecuteNonQuery();


                }
                else
                {
                    toplamtutarim = float.Parse(test.Rows[i]["total"].ToString());
                    toplamkar = float.Parse(test.Rows[i]["kar"].ToString());

                    toplamSatis = 1;
                    SQLiteCommand ekle = new SQLiteCommand("insert into gunsonu (tarih,toplamsatis,toplamtutar,toplamkar,nakit,kart,veresiye) values (@k1,@k2,@k3,@k4,@k5,@k6,@k7)", Baglan.connection);
                    ekle.Parameters.AddWithValue("@k1", test.Rows[i]["tarih"].ToString().Split()[0]);
                    ekle.Parameters.AddWithValue("@k2", toplamSatis.ToString());
                    ekle.Parameters.AddWithValue("@k3", test.Rows[i]["total"].ToString());
                    ekle.Parameters.AddWithValue("@k4", test.Rows[i]["kar"].ToString());
                    if (test.Rows[i]["tur"].ToString() == "Nakit")
                    {
                        ekle.Parameters.AddWithValue("@k5", test.Rows[i]["total"].ToString());
                        ekle.Parameters.AddWithValue("@k6", 0.ToString());
                        ekle.Parameters.AddWithValue("@k7", 0.ToString());
                    }
                    if (test.Rows[i]["tur"].ToString() == "Kart")
                    {

                        ekle.Parameters.AddWithValue("@k6", test.Rows[i]["total"].ToString());
                        ekle.Parameters.AddWithValue("@k5", 0.ToString());
                        ekle.Parameters.AddWithValue("@k7", 0.ToString());
                    }
                    if (test.Rows[i]["tur"].ToString().StartsWith("Vere"))
                    {

                        ekle.Parameters.AddWithValue("@k7", test.Rows[i]["total"].ToString());
                        ekle.Parameters.AddWithValue("@k5", 0.ToString());
                        ekle.Parameters.AddWithValue("@k6", 0.ToString());
                    }
                    ekle.ExecuteNonQuery();
                }
                SQLiteCommand updateUrun = new SQLiteCommand("Update satislar set gunsonu= '" + 1.ToString() + " ' where satisID Like '%" + test.Rows[i]["satisID"].ToString().Split()[0] + "%'", Baglan.connection);
                updateUrun.ExecuteNonQuery();
            }


            Baglan.connection.Close();
        }

       
            
        

        private void button5_Click(object sender, EventArgs e)
        {
            if (toplamTutar == 0)
            {
                MessageBox.Show("Ürün Yok!");
                return;
            }

            karHesapla();
            stokDusus();
            DatabaseHelper.satisEkle("Kart",toplamTutar.ToString(),kar.ToString(),urunlerList());
            ClearAll();
            MessageBox.Show("Kart Satışı Kaydedildi.");
            GunSonuAl();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            veresiyedefteri defter = new veresiyedefteri();
            
            defter.Show();
        }
        public void GunSonuAl()
        {
            /*
            var task = new Task(() =>
            {
                gunSonu();
            });
            task.Start();
            */
            gunSonu();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if(toplamTutar == 0)
            {
                MessageBox.Show("Ürün Yok!");
                return;
            }

            stokDusus();
            veresiye = new veresiye();
            veresiye.eklenen = toplamTutar;
            karHesapla();
            veresiye.kar = kar;
            veresiye.urunler = urunlerList();
            veresiye.Show();
            ClearAll();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            ClearAll();
        }
        satislar satislar;
        private void button8_Click(object sender, EventArgs e)
        {
            satislar = new satislar();
            satislar.Show();
        }

        gunsonlari gunsonlari;
        private void button9_Click(object sender, EventArgs e)
        {
            gunsonlari = new gunsonlari();
            gunsonlari.Show();
        }
    }

}
