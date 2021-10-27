using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pokhreli.module;

namespace Pokhreli.view_controller
{
    public partial class ViewMyProducts : Form
    {
        myProducts mp;
        DataTable myProducts;

        
        public ViewMyProducts()
        {
            InitializeComponent();
            mp = new myProducts();
        }

        private async void ViewMyProducts_Load(object sender, EventArgs e)
        {
            getProducts();


        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                int productId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                string command = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (command.ToLower() == "delete")
                {
                    if (MessageBox.Show("Are you sure you want to delete this product?  \n  Warning: Deleting product will also delete ingredient list of this product. ", "Confirm to delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        mp.product_id = productId;
                        Task<int> deleteproduct = new Task<int>(mp.deleteMyProduct);
                        Task<int> deleteingredientsList = new Task<int>(mp.deleteIngredientsList);
                        deleteproduct.Start();
                        deleteingredientsList.Start();

                        int res = await deleteproduct;
                        int res2 = await deleteingredientsList;

                        if (res > 0 && res2 > 0)
                        {

                            MessageBox.Show("Deleted successfully");
                            getProducts();
                        }


                    }


                }
            }
           
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            AddMyProduct amp = new AddMyProduct();
            amp.ShowDialog();
        }


        private async void getProducts()
        {
            Task<DataTable> getallproducts = new Task<DataTable>(mp.getallProducts);
            getallproducts.Start();
            myProducts = await getallproducts;
            dataGridView1.DataSource = myProducts;
            dataGridView1.Columns["id"].Visible = false;
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            ViewIngredients vig = new ViewIngredients(myProducts);
            vig.ShowDialog();
        }
    }
}
