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
    public partial class AddMyProduct : Form
    {

        module.myProducts products;
        module.Ingredients ingredients;
        DataTable ingredientlist;
        DataTable ingredientforProduct;
        int id = 1;
        public AddMyProduct()
        {
            InitializeComponent();
            products = new module.myProducts();
            ingredients = new module.Ingredients();
            ingredientforProduct = new DataTable();


        }

        private async void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            if (txtname.Text == "" | txtname.Text == " ")
            {
                labelstatus.Text = "Product name is required";
                txtname.Focus();
                return;
            }

            if (txtPrice.Text == "" | txtPrice.Text == " ")
            {
                labelstatus.Text = "Sales price is required";
                txtPrice.Focus();
                return;
            }
            products.query = "insert into myproducts values(Null,'" + txtname.Text + "','" + txtPrice.Text + "','1')";
            Task<int> addrecord = new Task<int>(products.addrecord);
            addrecord.Start();
            if (await addrecord == 1)
            {
                labelstatus.Text = "Successfully added . Now select ingredients for your products";

                dataGridView1.Visible = true;
                bunifuThinButton22.Visible = true;
                bunifuThinButton24.Visible = true;


            }
            else
            {
                labelstatus.Text = "Successfully added";
            }


        }

        private async void AddMyProduct_Load(object sender, EventArgs e)
        {

            ingredientforProduct.Columns.Add("id");
            ingredientforProduct.Columns.Add("ingredient_id");
            ingredientforProduct.Columns.Add("ingredient_name");
            ingredientforProduct.Columns.Add("amount");
            dataGridView1.DataSource = ingredientforProduct;
            dataGridView1.Columns["ingredient_id"].Visible = false;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Visible = false;
            bunifuThinButton22.Visible = false;
            bunifuThinButton24.Visible = false;


            panel2.Visible = false;
            Task<DataTable> getallingredients = new Task<DataTable>(ingredients.getallingredients);
            getallingredients.Start();

            ingredientlist = await getallingredients;
            comboBox1.DataSource = ingredientlist;
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == true)
            {
                panel2.Visible = false;
                return;
            }
            panel2.Visible = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            amountlabel.Text = "Amount( in " + ingredientlist.Rows[comboBox1.SelectedIndex]["unit"] + " )";
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {


            DataRow dr = ingredientforProduct.NewRow();
            dr["id"] = id;
            dr["ingredient_id"] = comboBox1.SelectedValue;
            dr["ingredient_name"] = comboBox1.Text;
            dr["amount"] = textBox1.Text;
            ingredientforProduct.Rows.Add(dr);
            id++;



        }


        public bool addingredients()
        {
            string productid = products.gethighestId();

            for (int i = 0; i < ingredientforProduct.Rows.Count; i++)
            {
                ingredients.addquery = "insert into product_ingredient values(Null,'" + productid + "'," + ingredientforProduct.Rows[i]["ingredient_id"] + ",'" + ingredientforProduct.Rows[i]["amount"] + "')";
                int res = ingredients.addrecord();
                if (res != 1)
                {
                    labelstatus.Text = "Error adding record";
                }


            }


            return true;





        }

        private async void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            labelstatus.Text = "Saving .......";
            Task<bool> savedata = new Task<bool>(addingredients);
            savedata.Start();
            bool res = await savedata;
            if (res)
            {
                labelstatus.Text = "Successfully Saved";
            }


        }



        private void bunifuThinButton25_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string command = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (command.ToLower() == "remove")
                {
                    int record_id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                    if (MessageBox.Show("Are you sure you want to delete this ingredient record? ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        for (int i = ingredientforProduct.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = ingredientforProduct.Rows[i];
                            if (int.Parse(dr["id"].ToString()) == record_id)
                                dr.Delete();

                        }
                        ingredientforProduct.AcceptChanges();

                    }
                }
            }
        }
    }
}
