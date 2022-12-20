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
    public partial class satislar : Form
    {
        public satislar()
        {
            InitializeComponent();
        }

        private void satislar_Load(object sender, EventArgs e)
        {
            string sql = "Select * From satislar";
            DataTable dt = new DataTable();
            SQLiteDataAdapter adtr = new SQLiteDataAdapter(sql, Baglan.connection);
            adtr.Fill(dt);
            dataGridView1.DataSource = dt;

            DataGridViewColumn column = dataGridView1.Columns[0];
            DataGridViewColumn column1 = dataGridView1.Columns[1];
            DataGridViewColumn column2 = dataGridView1.Columns[2];
            DataGridViewColumn column3 = dataGridView1.Columns[3];
            DataGridViewColumn column4 = dataGridView1.Columns[4];
            DataGridViewColumn column5 = dataGridView1.Columns[5];
            column.Width = 40;
            column1.Width = 100;
            column2.Width = 60;
            column3.Width = 60;
            column4.Width = 200;
            column5.Width = 150;
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

          
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            label3.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            label8.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            label2.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            label6.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            richTextBox1.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
