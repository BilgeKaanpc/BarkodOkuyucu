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
    public partial class gunsonlari : Form
    {
        public gunsonlari()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gunsonlari_Load(object sender, EventArgs e)
        {
            string sql = "select * from gunsonu";
            dataGridView1.DataSource = DatabaseHelper.Listele(sql);
        }
    }
}
