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
   
    public partial class Form2 : Form
    {
        public productAdd urunEklePage;
        public stokEkle stokEkle;
        public fiyatguncelle fiyatGuncelle;
        public urunlerListesi tumUrunler;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            urunEklePage = new productAdd();
            urunEklePage.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stokEkle = new stokEkle();
            stokEkle.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fiyatGuncelle = new fiyatguncelle();
            fiyatGuncelle.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tumUrunler = new urunlerListesi();
            tumUrunler.Show();
            this.Close();
        }
    }
}
