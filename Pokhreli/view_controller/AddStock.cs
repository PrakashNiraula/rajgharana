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
    public partial class AddStock : Form
    {

        module.Ingredients stock;
        DataTable stocklist;
        float quantity;
        float newamount;
        public AddStock()
        {
            InitializeComponent();
            stock = new module.Ingredients();
            stocklist = new DataTable();

        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void AddStock_Load(object sender, EventArgs e)
        {

            Task<DataTable> getallingredients = new Task<DataTable>(stock.getallingredients);
            getallingredients.Start();
            stocklist = await getallingredients;
            comboBox1.DataSource = stocklist;
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "name";
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stocklist.Rows.Count > 0)
            {
               
                labelunit.Text = stocklist.Rows[comboBox1.SelectedIndex]["unit"].ToString();
                currentstock.Text = "Current Stock: " + stocklist.Rows[comboBox1.SelectedIndex]["amount"].ToString() + " "+stocklist.Rows[comboBox1.SelectedIndex]["unit"].ToString();
            }
        }

        private void txtqty_OnValueChanged(object sender, EventArgs e)
        {
           if(float.TryParse(txtqty.Text,out quantity))
            {
               newamount = float.Parse(stocklist.Rows[comboBox1.SelectedIndex]["amount"].ToString()) + quantity;

                labelnewstock.Text = "New Stock: "+newamount;
            }else
            {
                labelnewstock.Text = "Invalid Amount";
            }
        }

        private async void bunifuThinButton22_Click(object sender, EventArgs e)
        {

            if(txtqty.Text=="" | txtqty.Text==" ")
            {

                labelnewstock.Text = "Enter Amount";
                txtqty.Focus();
                return;
            }

            if (txtprice.Text == "" | txtprice.Text == " ")
            {

                labelnewstock.Text = "Enter Cost price";
                txtprice.Focus();
                return;
            }

            stock.updatequery = "update ingredients set amount='"+newamount+"', updated_on='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where id='" + comboBox1.SelectedValue + "'";
            Task<int> updateamount = new Task<int>(stock.updaterecord);
            updateamount.Start();
             stock.addquery = "insert into purchase values(Null,'"+ stocklist.Rows[comboBox1.SelectedIndex]["name"].ToString() + "','"+ stocklist.Rows[comboBox1.SelectedIndex]["amount"].ToString() + "','"+newamount+"','"+txtprice.Text+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"')";
            Task<int> addpurchaserecord = new Task<int>(stock.addrecord);
            addpurchaserecord.Start();
            if(await updateamount==1 && await addpurchaserecord == 1)
            {
                labelnewstock.Text = "Successfully saved record";
            }



        }
    }
}
