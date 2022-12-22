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
    public partial class mekanlar : Form
    {
        public mekanlar()
        {
            InitializeComponent();
        }

        
        public void addPlace()
        {
            Baglan.connection.Open();

            SQLiteDataAdapter addToList = new SQLiteDataAdapter("select * from urunler", Baglan.connection);
            DataTable urunlerListim = new DataTable();
            addToList.Fill(urunlerListim);

            int index = urunlerListim.Columns.IndexOf("tur");
            var colums = urunlerListim.Columns.Cast<DataColumn>().Skip(index).ToList();

            foreach (DataColumn colum in colums)
            {
                comboBox1.Items.Add(colum.ColumnName);
            }


            Baglan.connection.Close();

        }
        private void mekanlar_Load(object sender, EventArgs e)
        {
            addPlace();
            string sql = "select * from mekanlar";
            dataGridView1.DataSource = DatabaseHelper.Listele(sql);

        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            
            //this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 1)
            {

                Baglan.connection.Open();

                SQLiteCommand addMekan = new SQLiteCommand("insert into mekanlar (mekanismi) values (@k1)", Baglan.connection);
                addMekan.Parameters.AddWithValue("@k1", textBox1.Text.ToString());

                addMekan.ExecuteNonQuery();
                SQLiteCommand addColumn = new SQLiteCommand("ALTER TABLE urunler ADD COLUMN '" + textBox1.Text.ToString() + " '  TEXT DEFAULT 0", Baglan.connection);
                addColumn.ExecuteNonQuery();

                Baglan.connection.Close();
                comboBox1.Items.Clear();
                addPlace();
                Form1 mainForm = new Form1();
                mainForm.addPlace();
                DatabaseHelper.showMessage(textBox1.Text.ToString() + " sisteme eklendi.","Bilgi", this);

                textBox1.Clear();
            }
            else
            {
                MessageBox.Show("Mekan ismi girilmedi!");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 1 && textBox1.Text.All(char.IsDigit))
            {


                Baglan.connection.Open();
                DataTable dt = new DataTable();
                SQLiteDataAdapter getProduct = new SQLiteDataAdapter("select * from urunler where barkod Like '%" + textBox2.Text + "%'", Baglan.connection);
                getProduct.Fill(dt);
                if(dt.Rows.Count != 0)
                {

                    label5.Text = dt.Rows[0][comboBox1.SelectedItem.ToString()].ToString();
                    label4.Text = dt.Rows[0]["isim"].ToString();
                }


                Baglan.connection.Close();
            }
            else
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Baglan.connection.Open();
            SQLiteCommand updatePrice = new SQLiteCommand("Update urunler set " + comboBox1.SelectedItem.ToString() + " = " + textBox3.Text.ToString() + "  where barkod = " + textBox2.Text, Baglan.connection);
            updatePrice.ExecuteNonQuery();

            DatabaseHelper.showMessage("Ürün Fiyatı Güncellendi","Bilgi", this);

            textBox3.Clear();
            textBox2.Clear();

            Baglan.connection.Close();
        }
    }
}
