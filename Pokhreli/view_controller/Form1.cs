using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pokhreli.view_controller;
using Pokhreli.dbConn;

namespace Pokhreli
{
    public partial class Form1 : Form
    {
        dbConnection db;
        string[] details;

        public Form1()
        {
            InitializeComponent();
            db = new dbConnection();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private async void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            if (bunifuMetroTextbox1.Text == "")
            {
                bunifuMetroTextbox1.Focus();
                return;
            }
            if (bunifuMetroTextbox2.Text == "")
            {
                bunifuMetroTextbox2.Focus();
                return;
            }

            labelstatus.Visible = true;
            Task<string[]> getdetails = new Task<string[]>(checkdetails);
            getdetails.Start();


            details = await getdetails;
            if (details[0]!= null)
            {
                labelstatus.Visible = false;
                AdminPanel ap = new AdminPanel();
                ap.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Credentials");
                labelstatus.Visible = false;
            }




            

        }


        private string[] checkdetails()
        {
            String[] names = new String[6];

            string query = "select * from user where username='" + bunifuMetroTextbox1.Text + "' and password='" + bunifuMetroTextbox2.Text + "'";
            try
            {
                DataTable res = db.GetDataTable(query);

                if (res.Rows.Count > 0)
                {
                    names[0] = res.Rows[0]["id"].ToString();
                    //names[1] = res.Rows[0]["fname"].ToString();
                   // names[2] = res.Rows[0]["role"].ToString();
                    return names;
                }else
                {
                    return names;
                }
                

            }catch(Exception ex)
            {
                throw ex;
            }

            


        }
    }
}
