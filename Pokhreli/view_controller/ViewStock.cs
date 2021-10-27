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
    public partial class ViewStock : Form
    {


        DataTable stocklist;
        module.Ingredients stock;
       
        public ViewStock()
        {
            InitializeComponent();
            stock = new module.Ingredients();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            
            AddStock ap = new AddStock();
            ap.ShowDialog();
            loaddata();
        }

        private void Stock_Load(object sender, EventArgs e)
        {

            loaddata();
        }

        public async void loaddata()
        {
            Task<DataTable> getallstock = new Task<DataTable>(stock.getallingredients);
            getallstock.Start();
            stocklist = await getallstock;
            stocklist.AcceptChanges();
            dataGridView1.DataSource = stocklist;
            dataGridView1.Columns["id"].Visible = false;
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int ingredient_id;
            if (e.RowIndex >= 0)
            {
                string command = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (command.ToLower() == "delete")
                {
                    ingredient_id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                    if (MessageBox.Show("Are you sure you want to delete this ingredient?  \n  Warning: Deleting ingredient can cause problems in billing, stock and products. ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        stock.updatequery = "delete from ingredients where id='" + ingredient_id + "'";
                        Task<int> deleteingredient = new Task<int>(stock.updaterecord);
                        deleteingredient.Start();
                        if (await deleteingredient == 1)
                        {
                            for (int i = stocklist.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow dr = stocklist.Rows[i];
                                if (int.Parse(dr["id"].ToString()) == ingredient_id)
                                    dr.Delete();

                            }
                            stocklist.AcceptChanges();
                        }



                    }

                }

            }

           
        
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ingredients ig = new Ingredients();
            ig.ShowDialog();
            loaddata();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddMyProduct amp = new AddMyProduct();
            amp.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //ViewIngredients vig = new ViewIngredients();
            //vig.ShowDialog();
        }

        private void bunifuThinButton22_Click_1(object sender, EventArgs e)
        {
            Ingredients ig = new Ingredients();
            ig.ShowDialog();
            loaddata();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    dataGridView1.DataSource = stocklist.Select("name LIKE '%" + textBox1.Text + "%'").CopyToDataTable();
                    return;
                }
                catch(Exception ex)
                {
                    dataGridView1.DataSource = null;

                }
             
            }
            dataGridView1.DataSource = stocklist;
           
            


        }
    }
}
