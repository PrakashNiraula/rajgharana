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
    public partial class ViewIngredients : Form
    {
        module.myProducts products;
        DataTable productlist = new DataTable();
        DataTable ingredientlist;
        DataTable allingredients;
        module.Ingredients ingredients;
        private int recordid;
        public ViewIngredients(DataTable productss)
        {

            InitializeComponent();
            products = new module.myProducts();
            ingredients = new module.Ingredients();
            productlist = productss;
            ingredientlist = new DataTable();
            panel2.Visible = false;
            panel3.Visible = false;
           

        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewIngredients_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = productlist;
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "name";
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            loadingredients();



        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (productlist.Rows.Count > 0)
            {
                products.product_id = int.Parse(productlist.Rows[comboBox1.SelectedIndex]["id"].ToString());
                Task<DataTable> getingredientforproduct = new Task<DataTable>(products.ingredientforproduct);
                getingredientforproduct.Start();

                ingredientlist = await getingredientforproduct;
                dataGridView1.DataSource = ingredientlist;
                if (dataGridView1.Columns.Count > 2)
                {
                    dataGridView1.Columns["ingredient_id"].Visible = false;
                }
              
                panel2.Visible = false;
            }


        }

        private void bunifuThinButton21_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        
            if (e.RowIndex >= 0)
            {
                string command = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (command.ToLower() == "delete")
                {
                    recordid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["record_id"].Value);
                    if (MessageBox.Show("Are you sure you want to delete this ingredient? ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        products.record_id = recordid;
                        Task<int> deleterecord = new Task<int>(products.deleteingredientOfproduct);
                        deleterecord.Start();
                        if(await deleterecord == 1)
                        {

                            for (int i = ingredientlist.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow dr = ingredientlist.Rows[i];
                                if (int.Parse(dr["record_id"].ToString()) == recordid)
                                    dr.Delete();

                            }
                            ingredientlist.AcceptChanges();

                        }


                    }



                }


                if (command.ToLower() == "update")
                {
                    recordid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["record_id"].Value);

                    panel3.Visible = true;
                    panel3.Location = new Point(200,100);
                    label3.Text = "Used Amount in ("+dataGridView1.Rows[e.RowIndex].Cells["unit"].Value.ToString()+")";
                }
            }
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == false)
            {
                panel2.Visible = true;
                
                return;
            }
            panel2.Visible = false;
        }

        public async void loadingredients()
        {
            Task<DataTable> loadallingredients = new Task<DataTable>(ingredients.getallingredients);
            loadallingredients.Start();
            allingredients = await loadallingredients;
            comboBox2.DataSource = allingredients;
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "name";
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            amountlabel.Text = "Amount in("+allingredients.Rows[comboBox2.SelectedIndex]["unit"].ToString()+")";
        }

        private async void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Amount cannot be 0 or character");
                return;
            }
            products.query = "insert into product_ingredient values(Null,'" + comboBox1.SelectedValue + "','" + comboBox2.SelectedValue + "','" + textBox1.Text + "')";
            products.product_id = int.Parse(comboBox1.SelectedValue.ToString());
            Task<DataTable> addnewIngredienttoProduct = new Task<DataTable>(products.addnewIngredientToProduct);
            addnewIngredienttoProduct.Start();
            dataGridView1.DataSource = await addnewIngredienttoProduct;
            panel2.Visible = false;
           
         
        }

        private void bunifuThinButton25_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
           
        }

        private async void bunifuThinButton24_Click(object sender, EventArgs e)
        {

            if (textBox2.Text == "")
            {
                MessageBox.Show("Invalid Amount ");
                return;
            }
            products.record_id = recordid;
            products.updateamount =float.Parse(textBox2.Text);
            Task<int> updateingredientamount = new Task<int>(products.updateamountofIngredient);
            updateingredientamount.Start();
            if(await updateingredientamount == 1)
            {
                for (int i = ingredientlist.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = ingredientlist.Rows[i];
                    if (int.Parse(dr["record_id"].ToString()) == recordid)
                        dr["used_amount"] = textBox2.Text;
                    panel3.Visible = false;

                }
                ingredientlist.AcceptChanges();

            }
            

        }
    }
}