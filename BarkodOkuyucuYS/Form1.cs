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
            satisTablosu.Columns.Add("Isim", typeof(string));
            satisTablosu.Columns.Add("Fiyat", typeof(string));
            satisTablosu.Columns.Add("Stok Bilgisi", typeof(int));
            satisTablosu.Columns.Add("Adet", typeof(int));
            satisTablosu.Columns.Add("Toplam Fiyat", typeof(string));
            bindingSource1.DataSource = satisTablosu;
            dataGridView1.DataSource = bindingSource1;
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

            }

            DataRow dr = satisTablosu.NewRow();
            dr["Barkod"] = drm["barkod"].ToString();
            dr["Isim"] = drm["isim"].ToString();
            dr["Fiyat"] = drm["satis"].ToString();
            dr["Stok Bilgisi"] = drm["stok"].ToString();
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
                float tutar = float.Parse(urunFiyati);
                toplamTutar = toplamTutar + tutar;

            }
            label3.Text = toplamTutar.ToString();
            Baglan.connection.Close();

            textBox1.Clear();
            textBox1.Focus();
        }

        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {

            string adet = satisTablosu.Rows[e.RowIndex]["Adet"].ToString();
            int newAdet = int.Parse(adet);
            fiyatAdd(e.RowIndex, newAdet);
            toplamTutar = 0;
            for (int i = 0; i < satisTablosu.Rows.Count; i++)
            {

                string urunFiyati = satisTablosu.Rows[i]["Toplam Fiyat"].ToString();
                float tutar = float.Parse(urunFiyati);
                toplamTutar = toplamTutar + tutar;

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
            nakitSatis = new nakitsatis();
            nakitSatis.tutar = toplamTutar;
            nakitSatis.urunList = urunlerList();
            karHesapla();
            nakitSatis.kar = kar;
            nakitSatis.Show();
            ClearAll();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            karHesapla();
            
            DatabaseHelper.satisEkle("Kart",toplamTutar.ToString(),kar.ToString(),urunlerList());
            ClearAll();
            MessageBox.Show("Kart Satışı Kaydedildi.");
        }
        private void button6_Click(object sender, EventArgs e)
        {
            veresiyedefteri defter = new veresiyedefteri();
            
            defter.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
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
    }

}
