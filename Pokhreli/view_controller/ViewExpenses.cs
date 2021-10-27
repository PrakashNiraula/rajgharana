using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pokhreli.view_controller
{
    public partial class ViewExpenses : Form
    {
        module.expenses exp;
        DataTable records;
        public ViewExpenses()
        {
            InitializeComponent();
            exp = new module.expenses();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            Expenses exp = new Expenses();
            exp.ShowDialog();
            loaddata();


        }

        private async void ViewExpenses_Load(object sender, EventArgs e)
        {
            loaddata();
        }

        private async void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            loaddata();
        }

       public async void loaddata()
        {
            exp.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            Task<DataTable> loaddata = new Task<DataTable>(exp.searchbydate);
            loaddata.Start();
            records = await loaddata;
            dataGridView1.DataSource = records;
            float sum = 0;
            foreach (DataRow dr in records.Rows)
            {

                sum += float.Parse(dr["amount"].ToString());
            }
            labeltotal.Text = "Total: Nrs " + sum;

        }
    }
}
