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
    public partial class nakitsatis : Form
    {
        public float tutar;
        public float paraUstu;
        public string urunList;
        public float kar;

        public nakitsatis()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DatabaseHelper.satisEkle("Nakit",tutar.ToString(),kar.ToString(),urunList);
            MessageBox.Show("Nakit Satış kaydedildi");

            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label4.Text = (float.Parse(textBox1.Text) - tutar).ToString();
        }

        private void nakitsatis_Load(object sender, EventArgs e)
        {
            label3.Text = tutar.ToString();
        }
    }
}
