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
        public string tutar;
        public veresiye()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void veresiye_Load(object sender, EventArgs e)
        {
            string sql = "select * from veresiye";
            dataGridView1.DataSource = DatabaseHelper.Listele(sql);
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
    }
}
