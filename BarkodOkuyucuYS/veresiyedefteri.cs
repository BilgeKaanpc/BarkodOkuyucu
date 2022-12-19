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
    public partial class veresiyedefteri : Form
    {
        public veresiyedefteri()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = "Select * from veresiye Where isim Like '%" + textBox1.Text.ToLower() + "%'";

            dataGridView1.DataSource = DatabaseHelper.Listele(sql);
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        private void veresiyedefteri_Load(object sender, EventArgs e)
        {
            string sql = "Select * from veresiye";

            dataGridView1.DataSource = DatabaseHelper.Listele(sql);
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
    }
}
