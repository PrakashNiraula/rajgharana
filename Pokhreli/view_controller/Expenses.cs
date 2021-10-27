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
    public partial class Expenses : Form
    {
        module.expenses expense;
        float amount;
        public Expenses()
        {
            InitializeComponent();
            expense = new module.expenses();
            labelstatus.Visible = false;
        }

        private async void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            labelstatus.Visible = true;
            if (bunifuMaterialTextbox1.Text == "")
            {
                MessageBox.Show("Expense name is required");
                return;
            }

            if (bunifuMaterialTextbox2.Text == "")
            {
                MessageBox.Show("Expense amount required");
                return;
            }

            bool res = float.TryParse(bunifuMaterialTextbox2.Text, out amount);
            if (!res)
            {
                MessageBox.Show("Invalid Amount");
                return;
            }

            expense.name = bunifuMaterialTextbox1.Text;
            expense.amount = amount;
            expense.description = textBox1.Text;
            expense.date = DateTime.Now.ToString("yyyy-MM-dd");

            Task<int> addrecord = new Task<int>(expense.addrecord);
            addrecord.Start();
           // var result = await addrecord;
            if (await addrecord != 1)
            {
                labelstatus.Text = "Someting went wrong. Try again";
                return;

            }else
            {
                labelstatus.Text = "Successfully saved";

                this.Close();
            }




        }

        private void bunifuThinButton25_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
