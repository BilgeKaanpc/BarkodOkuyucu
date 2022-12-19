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
    public partial class veresiye : Form
    {
        public string kisiAdi;
        public float borcu;
        public float eklenen;
        public float kar;
        public string urunler;
        public veresiye()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DatabaseHelper.veresiyeEkle(textBox1.Text.ToLower(), borcu, eklenen, textBox1.Text.ToLower(), eklenen.ToString(), kar.ToString(), urunler);
            updateData();

            MessageBox.Show(textBox1.Text+ " kişisine " + eklenen+ " eklendi");
        }

        private void veresiye_Load(object sender, EventArgs e)
        {
            updateData();   
        }
        public void updateData()
        {
            string sql = "select * from veresiye";
            dataGridView1.DataSource = DatabaseHelper.Listele(sql);
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = "Select * from veresiye Where isim Like '%" + textBox1.Text.ToLower() + "%'";

            dataGridView1.DataSource = DatabaseHelper.Listele(sql);
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
    }
}
