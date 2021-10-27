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
using System.Threading;

namespace Pokhreli.view_controller
{
    public partial class AdminPanel : Form
    {
        DataTable guestlist;
        module.guestEntry ge;
        public AdminPanel()
        {
            InitializeComponent();
            guestlist = new DataTable();
            ge = new module.guestEntry();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void AdminPanel_Load(object sender, EventArgs e)
        {

            Task<DataTable> getallguest = new Task<DataTable>(ge.getallguests);
            getallguest.Start();
                   
            loadpanel("Home");
            guestlist = await getallguest;

         

        }

        public void loadpanel(String name)
        {
            panel4.Controls.Clear();
            if (name == "Home")   
            {
                Home h = new Home();
              // bunifuTileButton1.BackColor=Color.CornflowerBlue;
                highliter.Top = bunifuTileButton1.Top;
                h.TopLevel = false;
                panel4.Controls.Add(h);
                h.Dock = DockStyle.Fill;
                h.Show();


            }
            else if (name == "GuestBill")
            {
                GuestBill gb = new GuestBill();
                highliter.Top = bunifuTileButton4.Top;
                // bunifuTileButton4.BackColor = Color.CornflowerBlue;
                gb.TopLevel = false;
                panel4.Controls.Add(gb);
                gb.Dock = DockStyle.Fill;
                gb.Show();

            }
            else if (name == "GuestEntry")

            {
                GuestEntry ge = new GuestEntry();
                //bunifuTileButton2.BackColor = Color.CornflowerBlue;
                highliter.Top = bunifuTileButton2.Top;
                ge.TopLevel = false;
                panel4.Controls.Add(ge);
                ge.Dock = DockStyle.Fill;
                ge.Show();


            }else if(name=="Room"){
                Room room = new Room();
               // bunifuTileButton5.BackColor = Color.CornflowerBlue;
                highliter.Top = bunifuTileButton5.Top;
                room.TopLevel = false;
                panel4.Controls.Add(room);
                room.Dock = DockStyle.Fill;
                room.Show();


            }
            else if (name == "Stock")
            {

                
                ViewStock stock = new ViewStock();
                highliter.Top = bunifuTileButton6.Top;
                stock.TopLevel = false;
                panel4.Controls.Add(stock);
                stock.Dock = DockStyle.Fill;
                stock.Show();
              


            }
            else if (name == "Product")
            {


                ViewMyProducts prod = new ViewMyProducts();
                highliter.Top = bunifuTileButton7.Top;
                prod.TopLevel = false;
                panel4.Controls.Add(prod);
                prod.Dock = DockStyle.Fill;
                prod.Show();



            }
            else if (name == "Expenses")
            {


                ViewExpenses exp = new ViewExpenses();
                highliter.Top = bunifuTileButton8.Top;
                exp.TopLevel = false;
                panel4.Controls.Add(exp);
                exp.Dock = DockStyle.Fill;
                exp.Show();



            }

            else
            {
                ViewData vd = new ViewData();
               // bunifuTileButton4.BackColor = Color.CornflowerBlue; ;
                highliter.Top = bunifuTileButton3.Top;
                vd.TopLevel = false;
                panel4.Controls.Add(vd);
                vd.Dock = DockStyle.Fill;
                vd.Show();


            }

        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            loadpanel("Home");

        }

        private void bunifuTileButton2_Click(object sender, EventArgs e)
        {
            loadpanel("GuestEntry");

        }

        private void bunifuTileButton4_Click(object sender, EventArgs e)
        {
            loadpanel("GuestBill");

        }

        private void bunifuTileButton3_Click(object sender, EventArgs e)
        {
            loadpanel("View");
        }

       

        private void bunifuTileButton5_Click(object sender, EventArgs e)
        {
            loadpanel("Room");
        }

        private void bunifuTileButton6_Click(object sender, EventArgs e)
        {
            loadpanel("Stock");
        }

        private void bunifuTileButton7_Click(object sender, EventArgs e)
        {
            loadpanel("Product");
        }

        private void bunifuTileButton8_Click(object sender, EventArgs e)
        {
            loadpanel("Expenses");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyTables mt = new MyTables();
            mt.ShowDialog();
        }
    }
}
