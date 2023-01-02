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
    public partial class stokEkle : Form
    {
        public stokEkle()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Baglan.connection.Open();
            if (textBox1.Text.Length > 3)
            {
                SQLiteCommand search = new SQLiteCommand("Select * From urunler where barkod = " + textBox1.Text, Baglan.connection);
                SQLiteDataReader drm = search.ExecuteReader();
                while (drm.Read())
                {
                    label4.Text = drm["isim"].ToString();
                }
            }
            Baglan.connection.Close();
        }
        int value = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            Baglan.connection.Open();
            if (textBox1.Text.Length > 3)
            {
                SQLiteCommand search = new SQLiteCommand("Select * From urunler where barkod = " + textBox1.Text, Baglan.connection);
                SQLiteDataReader drm = search.ExecuteReader();
                while (drm.Read())
                {
                    label4.Text = drm["isim"].ToString();
                    value = int.Parse(maskedTextBox1.Text) + int.Parse(drm["stok"].ToString());
                }
            }
            MessageBox.Show("Stok Güncellendi");

            Baglan.connection.Close();


            DatabaseHelper.stokEkle(value, textBox1.Text);
            textBox1.Clear();
            maskedTextBox1.Clear();
            textBox1.Focus();
            label4.Text = "";
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button1_Click(sender, e);
            }
        }

        private void stokEkle_Load(object sender, EventArgs e)
        {

        }
    }
}
