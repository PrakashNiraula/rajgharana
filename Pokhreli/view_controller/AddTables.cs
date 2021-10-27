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
    public partial class AddTables : Form
    {
        module.mytables tables;
        public AddTables()
        {
            InitializeComponent();
            tables = new module.mytables();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            labelstatus.Text = "";
            if (txttableno.Text == "")
            {
                labelstatus.Text = "Error: Table no is required";
                return;
            }
            tables.tableno = txttableno.Text;

            Task<int> addtable = new Task<int>(tables.addtable);
            addtable.Start();
           if(await addtable == 1)
            {
                labelstatus.Text = "Table Successfully added";
                this.Close();
            }else
            {
                labelstatus.Text = "Error adding table";

            }



        }
    }
}
