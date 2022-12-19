using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarkodOkuyucuYS
{
    public partial class urunlerListesi : Form
    {
        public urunlerListesi()
        {
            InitializeComponent();
        }

        private void urunlerListesi_Load(object sender, EventArgs e)
        {
            string sql = "Select * from urunler";
            FilterListe(sql);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "Select * from urunler";
            FilterListe(sql);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            string sql = "Select * from urunler Where barkod Like '%" + textBox1.Text + "%'";
            FilterListe(sql);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string sql = "Select * From urunler Where isim Like '%" + textBox2.Text + "%'";
            FilterListe(sql);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = "Select * From urunler Where tur Like '%Bira%'";
            FilterListe(sql);
        }
        public void FilterListe(string sql)
        {

            dataGridView1.DataSource = DatabaseHelper.Listele(sql);
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sql = "Select * From urunler Where tur Like '%Alkol%'";
            FilterListe(sql);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "Select * From urunler Where tur Like '%Sigara%'";
            FilterListe(sql);
        }
    }
}
