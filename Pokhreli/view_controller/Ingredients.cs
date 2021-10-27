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
    public partial class Ingredients : Form
    {
        module.Ingredients ingredients;
        public Ingredients()
        {
            InitializeComponent();
            ingredients = new module.Ingredients();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            if(txtname.Text=="" | txtname.Text==" ")
            {
                labelstatus.Text = "Name is required";
                txtname.Focus();
                return;
            }
            if(txtqty.Text=="" | txtqty.Text==" ")
            {
                labelstatus.Text = "Amount is required";
                txtqty.Focus();
                return;
            }

            if(txtunit.Text=="" | txtunit.Text==" ")
            {
                labelstatus.Text = "Unit is required";
                txtunit.Focus();
                return;
            }
            ingredients.addquery = "insert into ingredients values(Null,'"+txtname.Text+"','"+txtunit.Text+"','"+txtqty.Text+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"')";
            Task<int> addrecord = new Task<int>(ingredients.addrecord);
            addrecord.Start();
            if(await addrecord == 1)
            {
                labelstatus.Text = "Successfully Added";
                this.Close();

            }else
            {
                labelstatus.Text = "Error Saving";
            }
          
        }
    }
}
