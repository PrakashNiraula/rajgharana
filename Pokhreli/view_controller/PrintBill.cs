using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pokhreli.view_controller
{
    public partial class PrintBill : Form
    {

        DataTable products;
        module.billentry be;
        string name;
        string billtype;
        string description;
        string advance="0";
        string number;
        string billno;
        int off;
        string vat="0";
        string servicecharge="0";
        int productquantity = 1;
       float producttotal = 0;
        float productrate = 1;
        int productid;
        string totalforproduct;
        

        string discount="0";




        float total = 0;
        float final=0;
        int tableid;
        module.myProducts mp;
        DataTable allproducts;
        module.mytables tables;
        module.Room room;
        module.guestEntry ge;

        public PrintBill(DataTable products,string name,string billtype,string description,string advance,string number,string billno,string servicecharge,string vat,int tableid,string discount,string producttotal)
        {
            InitializeComponent();
            this.products = products;
            this.name = name;
            this.billtype = billtype;
            this.description = description;
            this.advance = advance;
            this.number = number;
            this.billno = billno;
            this.servicecharge = servicecharge;
            this.vat = vat;
            this.tableid = tableid;
            this.discount = discount;
            this.totalforproduct = producttotal;
           


            panel3.Visible = false;
            mp = new module.myProducts();
            be = new module.billentry();
            tables = new module.mytables();
            room = new module.Room();
            ge = new module.guestEntry();

        }

        private async void PrintBill_Load(object sender, EventArgs e)
        {

            if (billtype == "Room")
            {
                lblnumber.Text = "Room no:" + number;

            }else
            {
                lblnumber.Text = "Table no: " +name ;
            }
            lbldate.Text = "Date "+DateTime.Now.ToString("yyyy-MM-dd");
            lblcusname.Text = name;
            textBox1.Text = description;
            labeladvance.Text = advance;
            labeldiscount.Text = discount;
            dataGridView1.DataSource = products;

            lblbillno.Text = "Bill no: A" + billno;
            // bunifuCustomLabel3.Text = panorvat;
            //labelbilltotal.Text;
            calculatetotal();


            //loadallproducts
           
            Task<DataTable> getallproducts = new Task<DataTable>(mp.getallProducts);
            getallproducts.Start();
            allproducts = await getallproducts;
            comboBox2.DataSource = allproducts;
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;


            labelbilltotal.Text = totalforproduct;



            labelstatus.Text = "Status: " + number;
            if(number=="Checked out")
            {
                bunifuThinButton22.Visible = false;
                bunifuThinButton21.Visible = false;
                dataGridView1.Columns[0].Visible = false;
            }


        }

        public void calculatetotal()
        {
            producttotal = 0;

            if (products.Rows.Count > 0)
            {
                for (int i = 0; i < products.Rows.Count; i++)
                {
                    producttotal = producttotal + float.Parse(products.Rows[i]["Total"].ToString());
                    products.Rows[i]["ID"] = (i+1).ToString();

                }
                labelbilltotal.Text = total + "";
                labeldiscount.Text = discount + "";
                labelwithvat.Text = vat.ToString();
                labelservice.Text = servicecharge.ToString();
               final = producttotal + float.Parse(vat) * producttotal / 100 + float.Parse(servicecharge) * producttotal / 100 - float.Parse(discount) * producttotal / 100 - float.Parse(advance);

                labelbilltotal.Text = final + "";
               // final = total + float.Parse(vat) + float.Parse(servicecharge) - float.Parse(advance);
                 finaltotal.Text = final + "";
            }
         
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            DialogResult result = printPreviewDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {


            Graphics g = e.Graphics;
            Font font = new Font("Times New Roman", 15);
            float fontheight = font.GetHeight();
            int startx = 100;
            int starty = 40;
            int offset = 50;
            int offset2 = 500;
            //1240 x 1754 ay paper size for this display
            float leftmargin = e.MarginBounds.Left;
            float topmargin = e.MarginBounds.Top;

            // e.Graphics.DrawLine(new Pen(Color.Black), offset2-65, 0, offset2-65, 1200);

            //int y = 0;

            //using (Pen dashed_pen = new Pen(Color.Black, 2))
            //{

            //    dashed_pen.DashStyle = DashStyle.Dash;
            //    e.Graphics.DrawLine(dashed_pen, offset2 - 65, 0, offset2 - 65, 1200);
            //  //  y += 2;

            //}



            g.DrawString("Hotel Rajgharana", font, new SolidBrush(Color.Black), startx+200, starty);
            //g.DrawString("Hotel Rajgharana", font, new SolidBrush(Color.Black), offset2, starty);

            g.DrawString("Invoice", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx+80+200, starty+30);
           // g.DrawString("Invoice", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 80, starty + 30);
           

            g.DrawString("Date: "+DateTime.Now.ToString(), new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx+130+370, starty+offset);
//g.DrawString("Date: " + DateTime.Now.ToString(), new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2+130, starty + offset);



            g.DrawString("Phone: 9812190060" , new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx-50, starty + offset+20);
           // g.DrawString(panorvat, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx+165, starty + offset + 20);

          //  g.DrawString("Phone: 027-421026", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 20);
           // g.DrawString(panorvat, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 165, starty + offset + 20);

            g.DrawString("Bill no: "+billno, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset);
            g.DrawString(lblnumber.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 40);
            g.DrawString("Name: " + name, new Font("Times New Roman", 14), new SolidBrush(Color.Black), startx - 50, starty + offset + 60);

            //g.DrawString(billno, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset);
            //g.DrawString(lblnumber.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 40);
            //g.DrawString("Name: " + name, new Font("Times New Roman", 14), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 60);




            g.DrawString(".........................................................................................................................................................................................................................................................." , new Font("Times New Roman", 15), new SolidBrush(Color.Black), startx - 200, starty + offset + 70);

            ////id,particular,rate,qty,total

            g.DrawString("ID", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx, starty + offset + 90);
            g.DrawString("Particular", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx+150 , starty + offset + 90);
            g.DrawString("Rate", new Font("Times New Roman", 12),new SolidBrush(Color.Black), startx +150+200, starty + offset + 90);
            g.DrawString("Qty", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 200+250, starty + offset + 90);
            g.DrawString("Total", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 250+350, starty + offset + 90);


            //g.DrawString("ID", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 60, starty + offset + 90);
            //g.DrawString("Particular", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2, starty + offset + 90);
            //g.DrawString("Rate", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 150, starty + offset + 90);
            //g.DrawString("Qty", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 200, starty + offset + 90);
            //g.DrawString("Total", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 250, starty + offset + 90);

            for (int i = 0; i < products.Rows.Count; i++)
            {
                off = (i * 20) + 30;

                g.DrawString(products.Rows[i]["ID"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), startx , starty + offset + 100 + off);
                g.DrawString(products.Rows[i]["Particular"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), startx+150, starty + offset + 100 + off);
                g.DrawString(products.Rows[i]["Rate"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), startx + 150+200, starty + offset + 100 + off);
                g.DrawString(products.Rows[i]["Quantity"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), startx + 200+250, starty + offset + 100 + off);
                g.DrawString(products.Rows[i]["Total"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), startx + 250+350, starty + offset + 100 + off);


                //g.DrawString(products.Rows[i]["ID"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), offset2 - 60, starty + offset + 100 + off);
                //g.DrawString(products.Rows[i]["Particular"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), offset2, starty + offset + 100 + off);
                //g.DrawString(products.Rows[i]["Rate"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), offset2 + 150, starty + offset + 100 + off);
                //g.DrawString(products.Rows[i]["Quantity"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), offset2 + 200, starty + offset + 100 + off);
                //g.DrawString(products.Rows[i]["Total"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), offset2 + 250, starty + offset + 100 + off);


              

            }

            g.DrawString("...................................................................................................................................................................................................................................................................................", new Font("Times New Roman", 15), new SolidBrush(Color.Black), startx - 200, starty + offset + off+110);
            g.DrawString("Total: " + "Nrs " + total.ToString(), new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 500, starty + offset + off + 130);
            g.DrawString("VAT(in Nrs):" + vat, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 500, starty + offset + off + 150);
            g.DrawString("Service Charge(in Nrs):" + servicecharge, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 500, starty + offset + off + 170);

            //g.DrawString("Total: " + "Nrs " + total.ToString(), new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 180, starty + offset + off + 130);
            //g.DrawString("VAT(in Nrs): Nrs " + vat, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 110, starty + offset + off + 150);
            //g.DrawString("Service Charge(in Nrs): Nrs " + servicecharge, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 110, starty + offset + off + 170);



            if (billtype == "Room")
            {
                g.DrawString("Advance: Nrs " + advance, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 500, starty + offset + off + 190);
                 g.DrawString("Final Total: Nrs "+final, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 500, starty + offset + off + 210);
                g.DrawString("Recieved by: ...............................", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 450, starty + offset + off + 250);



                //g.DrawString("Advance: Nrs " + advance, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 155, starty + offset + off + 190);
                //g.DrawString("Final Total: Nrs " + final, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 155, starty + offset + off + 210);
                //g.DrawString("Recieved by: ...............................", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 90, starty + offset + off + 250);




            }
            if (billtype == "Table")
            {
                 g.DrawString("Final Total: Nrs " + final, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 500, starty + offset + off + 190);
                g.DrawString("Recieved by: ...............................", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 450, starty + offset + off + 230);


                //   g.DrawString("Final Total: Nrs " + final, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 155, starty + offset + off + 190);
                //g.DrawString("Recieved by: ...............................", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 90, starty + offset + off + 230);

            }


        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == true)
            {
                panel3.Visible = false;
                return;
            }
            panel3.Visible = true;


        }

        private async void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            productid = int.Parse(comboBox2.SelectedValue.ToString());
            Task<bool> updatebill = new Task<bool>(updatebillcontent);
            updatebill.Start();
            if(await updatebill == true)
            {
                dataGridView1.DataSource = products;
                products.AcceptChanges();
               
                calculatetotal();
                MessageBox.Show("Successfully Added");


            }
            else
            {
                MessageBox.Show("Error saving");

            }

        }


        private bool updatebillcontent()
        {

            try
            {
                be.insertquery = "insert into bill_content values(Null,'" + billno + "','" + productid + "','" + productquantity + "','" + productrate + "','" + productquantity * productrate + "')";
                int ress = be.insertdata();

                //update main bill
                be.billid = billno;
                be.newamount = productquantity * productrate;

                int res = be.updatebill();

                be.query = "select bct.id as record_id,mp.name,bct.quantity,bct.rate,bct.total from myproducts mp,bill_content bct, guest_bill gb where bct.bill_id=gb.id and mp.id=bct.product_id and bct.bill_id='" + billno + "'";


                var resss = be.getdata();
                products.Rows.Clear();
                for (int i = 0; i < resss.Rows.Count; i++)
                {
                    products.Rows.Add(resss.Rows[i]["record_id"].ToString(), (i + 1), resss.Rows[i]["name"].ToString(), resss.Rows[i]["rate"].ToString(), resss.Rows[i]["quantity"].ToString(), resss.Rows[i]["total"].ToString());
                }


            }
            catch(Exception ex)
            {
                throw ex;
            }
            

           
            return true;









        }

        private void quantity_OnValueChanged(object sender, EventArgs e)
        {
            
               // productquantity = int.Parse(quantity.Text);
                productrate = float.Parse(allproducts.Rows[comboBox2.SelectedIndex]["price"].ToString());

                if (quantity.Text == "")
            {
                return;
            }
            bool res = int.TryParse(quantity.Text.ToString(), out productquantity);
            if (!res)
            {
                MessageBox.Show("Invalid Input");
                return;
            }
            producttotal = productrate * productquantity;
            labeltotal.Text = producttotal.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            quantity.Text = "1";
          
            productrate = float.Parse(allproducts.Rows[comboBox2.SelectedIndex]["price"].ToString());
            producttotal = productrate * productquantity;
            labeltotal.Text = producttotal.ToString();
            labelrate.Text = "Nrs " + productrate;

        }

        private async void bunifuThinButton22_Click(object sender, EventArgs e)
        {



            Checkout chk = new Checkout(final,billtype,billno,tableid+"");
           // chk.productTotal = labelbilltotal.Text;
            chk.billid = billno;
            chk.billtype = billtype;
            chk.ShowDialog();
            bunifuThinButton22.Visible = false;
            dataGridView1.Columns[0].Visible = false;








            //if (billtype == "Table")
            //{
            //    tables.tableid = tableid;
            //    Task<int> emptytables = new Task<int>(tables.emptytable);
            //    emptytables.Start();

            //    be.billid = billno;
            //    Task<int> updatebill = new Task<int>(be.checkoutbilling);
            //    updatebill.Start();

            //    if (await emptytables == 1 && await updatebill == 1)
            //    {
            //        MessageBox.Show("Checked out successfully");
            //    }

            //}else if (billtype == "Room")
            //{
            //    Task<bool> updateroomandbill = new Task<bool>(emptyRooms);
            //    updateroomandbill.Start();
            //    if(await updateroomandbill == true)
            //    {
            //        MessageBox.Show("Checked out successfully");
            //    }
            //}
        }


       

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

           // int billid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
            string command = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            if (command.ToLower() == "delete")
            {
               string recordid=be.recordid = products.Rows[e.RowIndex]["record_id"].ToString();

                Task<int> deleterecord = new Task<int>(be.deletebillcontent);
                deleterecord.Start();
                var res = await deleterecord;
                if(res == 1)
                {
                    for (int i = products.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = products.Rows[i];
                        if (int.Parse(dr["record_id"].ToString()) == int.Parse(recordid))
                        {
                            dr.Delete();
                        }
                        products.AcceptChanges();


                    }
                    calculatetotal();

                }

            }

        }
    }
}
