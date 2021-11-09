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
    public partial class Checkout : Form
    {


        public string billid;
        //public string productTotal;
        public string billtype;
        private float producttotal=0;
        public float finalaftercharge;
        public float discountinpercentage=0;
        public float servicechargeinpercentage=0;
        public float vatinpercentage=0;
        public string tableid;
        float priceaftercharges = 0;

        module.mytables tables;
        module.billentry be;
        module.guestEntry ge;
        module.Room room;
      


        public Checkout(float producttotal,string billtype,string billno,string tableid)
        {
            InitializeComponent();
            this.producttotal = producttotal;
            this.billtype = billtype;
            this.billid = billno;
            tables = new module.mytables();
            this.tableid = tableid;
            be = new module.billentry();
            ge = new module.guestEntry();
            room = new module.Room();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            string name = "";
        }

        private void Checkout_Load(object sender, EventArgs e)
        {
            labelproducttotal.Text = producttotal.ToString();
            labelbilltype.Text = billtype.ToString();
            
       
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuMaterialTextbox6_OnValueChanged(object sender, EventArgs e)
        {
            //vat
            labelaftercharges.Text = "";

            bool res = float.TryParse(bunifuMaterialTextbox6.Text, out vatinpercentage);
            if (res == true)
            {
                calculatetotal();
            }else
            {
                labelaftercharges.Text = "Invalid vat input";
            }
           

        }

        private void bunifuMaterialTextbox5_OnValueChanged(object sender, EventArgs e)
        {
            //service charge

            labelaftercharges.Text = "";

            bool res = float.TryParse(bunifuMaterialTextbox5.Text, out servicechargeinpercentage);
            if (res == true)
            {
                calculatetotal();
            }
            else
            {
                labelaftercharges.Text = "Invalid service charge input";
            }
        }

        private void bunifuMaterialTextbox4_OnValueChanged(object sender, EventArgs e)
        {
            //discount



            labelaftercharges.Text = "";

            bool res = float.TryParse(bunifuMaterialTextbox4.Text, out discountinpercentage);
            if (res == true)
            {
                calculatetotal();
            }
            else
            {
                labelaftercharges.Text = "Invalid discount input";
            }

        }


        public void calculatetotal()
        {
            //  priceaftercharges = producttotal - ((producttotal * discountinpercentage) / 100) + ((producttotal * vatinpercentage) / 100) + ((producttotal * servicechargeinpercentage) / 100);
            // priceaftercharges = (producttotal - (producttotal * discountinpercentage / 100)+ (producttotal * discountinpercentage / 100));
            float disc = producttotal * discountinpercentage / 100;
            float pricewithdisc = producttotal - disc;
            float pricewithdiscandservice = pricewithdisc+(pricewithdisc* servicechargeinpercentage / 100);
            priceaftercharges = pricewithdiscandservice + (pricewithdiscandservice * vatinpercentage / 100);
            //priceaftercharges=

            labelaftercharges.Text = priceaftercharges.ToString();



        }

        private async void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            if (billtype == "Table")
            {
                tables.tableid = int.Parse(tableid);
                Task<int> emptytables = new Task<int>(tables.emptytable);
                emptytables.Start();

                be.billid = billid;
                be.vat = vatinpercentage + "";
                be.servicecharge = servicechargeinpercentage + "";
                be.discount = discountinpercentage + "";
                be.priceaftercharges = priceaftercharges+"";
                Task<int> updatebill = new Task<int>(be.checkoutbilling);
                updatebill.Start();

                if (await emptytables == 1 && await updatebill == 1)
                {
                    MessageBox.Show("Checked out successfully");
                    this.Close();
                }

            }
            else if (billtype == "Room")
            {
                Task<bool> updateroomandbill = new Task<bool>(emptyRooms);
                updateroomandbill.Start();
                if (await updateroomandbill == true)
                {
                    MessageBox.Show("Checked out successfully");
                    this.Close();
                }
            }
        }

        private bool emptyRooms()
        {
            //updatebill


            be.billid = billid;
            be.vat = vatinpercentage + "";
            be.servicecharge = servicechargeinpercentage + "";
            be.discount = discountinpercentage + "";
            be.priceaftercharges = priceaftercharges + "";

            int res = be.checkoutbilling();

            ge.entryId = tableid;
            int resss = ge.guestentryBilled();


            int j = 0;
            room.billid = tableid.ToString();
            DataTable allrooms = room.getallRoomsbookedTobill();
            int rows = allrooms.Rows.Count;
            for (int i = 0; i < allrooms.Rows.Count; i++)
            {
                room.query = "update room set status='Empty', bookedTo='0' where id='" + allrooms.Rows[i]["id"].ToString() + "'";
                int ress = room.insertroom();
                if (ress == 1)
                {
                    j++;
                }

            }
            if (j == allrooms.Rows.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
