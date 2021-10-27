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
    public partial class MyTables : Form
    {
        module.mytables mt;
        DataTable mytables=null;
        public MyTables()
        {
            InitializeComponent();
            mt = new module.mytables();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private  void MyTables_Load(object sender, EventArgs e)
        {
            loadtables();
        }


        private async void  loadtables()
        {
            Task<DataTable> loadtables = new Task<DataTable>(mt.getalltables);
            loadtables.Start();
            mytables = await loadtables;
            if (mytables.Rows.Count > 0)
            {
                dataGridView1.DataSource = mytables;
            }
           

        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            AddTables at = new AddTables();
            at.ShowDialog();
            loadtables();
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            mt.tableid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
            string command = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            if (command.ToLower() == "delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this table?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    Task<int> deleteTable = new Task<int>(mt.deletebyId);
                    deleteTable.Start();
                    if (await deleteTable == 1)
                    {
                        for (int i = mytables.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = mytables.Rows[i];
                            if (int.Parse(dr["id"].ToString()) == mt.tableid)
                                dr.Delete();

                        }
                        mytables.AcceptChanges();
                    }

                }


            }


        }
    }
}
