using System;
using System.Data;
using System.Threading.Tasks;
using Pokhreli.module;
using System.Windows.Forms;

namespace Pokhreli.view_controller
{

    public partial class GuestBill : Form
    {

        DataTable records;
        DataRow dr;
        int productquantity = 1;
        float productrate = 1;
        float producttotal;
        float billtotal = 0;
        float vat = 0;
        float servicecharge = 0;
        float advance = 0;
        double totalwithVat;
        module.myProducts mp;
        module.Room room;
        DataTable productlist;
        String billtype;
        string number;
        string guestentryId;
        string panorvat;
        DataTable emptytables;
        int tableid;
        int guestentryid;



        billentry be;
        module.guestEntry ge;
        DataTable guestList;
        module.Ingredients ingredients;
        module.mytables tables;
        int billid;

        public GuestBill()
        {
            InitializeComponent();
            records = new DataTable();
            be = new billentry();
            ge = new guestEntry();
            guestList = new DataTable();
            mp = new myProducts();
            ingredients = new module.Ingredients();
            // productlist = new DataTable();
            room = new module.Room();
            tables = new mytables();
            string tableid;

            records.Columns.Add("product_id", typeof(string));
            records.Columns.Add("ID", typeof(string));
            records.Columns.Add("Particular", typeof(string));
            records.Columns.Add("Rate", typeof(string));
            records.Columns.Add("Quantity", typeof(string));

            records.Columns.Add("Total", typeof(string));


            dataGridView1.DataSource = records;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;



        }



        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == Visible)
            {
                panel3.Visible = false;
            }
            else
            {
                panel3.Visible = true;
            }

        }

        private async void GuestBill_Load(object sender, EventArgs e)
        {
            labeldate.Text = "Date: " + DateTime.Now.ToString("yyyy-MM-dd");
            panel3.Visible = false;
            labelstatus.Visible = false;
            Task<DataTable> getallguest = new Task<DataTable>(ge.getallguests);
            getallguest.Start();
           // bunifuMaterialTextbox2.Visible = false;
            radioButton2.Checked = true;



            Task<string> getbillid = new Task<string>(be.gethighestid);
            getbillid.Start();
            guestList = await getallguest;
            //comboBox1.DataSource = guestList;
            //comboBox1.ValueMember = "id";
            //comboBox1.DisplayMember = "roomNumber";

            bunifuMaterialTextbox1.Text = "";
            advance = 0;
            labeladvance.Text = advance.ToString();
            billid = int.Parse(await getbillid) + 1;
            bunifuCustomLabel5.Text = "SN: A" + (billid).ToString();



            Task<DataTable> getallproducts = new Task<DataTable>(mp.getallProducts);
            getallproducts.Start();
            productlist = await getallproducts;
            comboBox2.DataSource = productlist;
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;


            Task<DataTable> loademptytables = new Task<DataTable>(tables.getemptytables);
            loademptytables.Start();
            emptytables = await loademptytables;


            comboBox1.DataSource = emptytables;
            comboBox1.DisplayMember = "table_no";
            comboBox1.ValueMember = "id";
            //comboBox1.SelectedIndex = 0;
          



        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            records.Rows.Add(comboBox2.SelectedValue, records.Rows.Count + 1, comboBox2.Text, productrate, productquantity, labeltotal.Text);
            billtotal = billtotal + producttotal;

            labelbilltotal.Text = billtotal.ToString();
            float final = billtotal - advance;
            finaltotal.Text = final+"";
            vat = 0;
           

        }

        private void quantity_OnValueChanged(object sender, EventArgs e)
        {
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




        private void bunifuMaterialTextbox4_OnValueChanged(object sender, EventArgs e)
        {

            if (bunifuMaterialTextbox4.Text == "")
            {
                return;
            }
            bool res = float.TryParse(bunifuMaterialTextbox4.Text, out servicecharge);
            if (!res)
            {
                MessageBox.Show("Invalid input");
            }
            finaltotal.Text = ((billtotal - advance) + servicecharge+vat).ToString();

        }

        private async void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            labelstatus.Text = "Saving....";
            if (bunifuMaterialTextbox1.Text == "")
            {
                MessageBox.Show("Customer name is required");
                bunifuMaterialTextbox1.Focus();
                return;
            }
            
             
            if (radioButton1.Checked == true)
            {
                billtype = "Room";
                number = comboBox1.Text;
                guestentryId = comboBox1.SelectedValue.ToString();
            }
            else
            {
                billtype = "Table";
              
                tableid = int.Parse(comboBox1.SelectedValue.ToString());
            }



            //// saveBill();

            tableid = int.Parse(comboBox1.SelectedValue.ToString());
            labelstatus.Visible = Visible;
            Task<bool> savetodb = new Task<bool>(saveBill);
            savetodb.Start();
            if (await savetodb == true)
            {
                labelstatus.Text = "Successfully Saved";
            }else
            {
                labelstatus.Text = "Error";
            }







        }

        //private void bunifuThinButton23_Click(object sender, EventArgs e)
        //{

         
        //    if (radioButton1.Checked == true)
        //    {
        //        billtype = "Room";
        //        number = comboBox1.Text;


        //    }
        //    else
        //    {
        //        billtype = "Table";
        //        number = comboBox1.Text;
        //    }

          

        //    PrintBill pb = new PrintBill(records, bunifuMaterialTextbox1.Text, billtype, textBox1.Text, advance.ToString(), number, bunifuCustomLabel5.Text, bunifuMaterialTextbox4.Text,vat.ToString(),tableid);
        //    pb.ShowDialog();
        //}





        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true){



                bunifuMaterialTextbox1.Text = guestList.Rows[comboBox1.SelectedIndex]["guest_name"].ToString();
                advance = float.Parse(guestList.Rows[comboBox1.SelectedIndex]["advance"].ToString());
                guestentryId= guestList.Rows[comboBox1.SelectedIndex]["id"].ToString();
                labeladvance.Text = advance.ToString();


                if (records.Rows.Count == 1)
                {
                    records.Rows.Clear();
                    dr["ID"] = 1;
                    dr["product_id"] = 1;
                    dr["Particular"] = "Room";
                    dr["Rate"] = guestList.Rows[comboBox1.SelectedIndex]["rateofRoom"].ToString();
                    //subtract entry date to current date
                    double noofdays = (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) - Convert.ToDateTime(guestList.Rows[comboBox1.SelectedIndex]["date"].ToString())).TotalDays;
                    if (noofdays != 0)
                    {
                        dr["Quantity"] = noofdays;
                    }
                    else
                    {
                        dr["Quantity"] = 1;
                    }



                    dr["Total"] = float.Parse(dr["Rate"].ToString()) * float.Parse(dr["Quantity"].ToString());

                    records.Rows.Add(dr);
                    billtotal = calculateTotal();
                   
                    labelbilltotal.Text = billtotal.ToString();
                   float final = billtotal - advance;
                    finaltotal.Text = (final).ToString();


                }

            }else
            {
                bunifuMaterialTextbox1.Text = comboBox1.Text;
            }

        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            billby();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            billby();
        }

        public void billby()
        {
            if (radioButton1.Checked == true)
            {
                bunifuThinButton22.ButtonText = "Save";
                comboBox1.DataSource = guestList;
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "roomNumber";
                bunifuCustomLabel20.Text = "Room no";
                //bunifuMaterialTextbox2.Visible = false;
                
                comboBox1.Visible = true;
                if (guestList.Rows.Count > 0)
                {
                    bunifuMaterialTextbox1.Text = guestList.Rows[0]["guest_name"].ToString();
                    advance = float.Parse(guestList.Rows[0]["advance"].ToString());
                    labeladvance.Text = advance.ToString();
                    dr = records.NewRow();
                    if (records.Rows.Count == 0)
                    {
                        dr["ID"] = 1;
                        dr["product_id"] = 1;
                        dr["Particular"] = "Room";
                        dr["Rate"] = guestList.Rows[0]["rateofRoom"].ToString();
                        double noofdays = (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) - Convert.ToDateTime(guestList.Rows[comboBox1.SelectedIndex]["date"].ToString())).TotalDays;
                        if (noofdays != 0)
                        {
                            dr["Quantity"] = noofdays;
                        }
                        else
                        {
                            dr["Quantity"] = 1;
                        }
                        dr["Total"] =float.Parse(dr["Rate"].ToString()) *float.Parse(dr["Quantity"].ToString());
                        records.Rows.Add(dr);
                        billtotal = calculateTotal();
                        labelbilltotal.Text = billtotal.ToString();
                        finaltotal.Text = (billtotal - advance).ToString();
                        
                    }
                }

                //bunifuMaterialTextbox3.Focus();

            }

            if (radioButton2.Checked == true)
            {
                bunifuThinButton22.ButtonText = "Start";
                bunifuMaterialTextbox1.Text = "";
                bunifuCustomLabel20.Text = "Table no";
                //bunifuMaterialTextbox2.Visible = false;
                comboBox1.Visible = true;
                //load empty tables
                comboBox1.DataSource = emptytables;
                comboBox1.DisplayMember = "table_no";
                comboBox1.ValueMember = "id";



                advance = 0;
                billtotal = 0;
                labeladvance.Text = "0";
                records.Rows.Clear();
                labelbilltotal.Text = billtotal.ToString();
                finaltotal.Text = "0";
                bunifuMaterialTextbox1.Text = comboBox1.Text;



            }


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            productrate = float.Parse(productlist.Rows[comboBox2.SelectedIndex]["price"].ToString());
            labelrate.Text = "Nrs " + productrate;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                int recordid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

                for (int i = records.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = records.Rows[i];
                    if (int.Parse(dr["ID"].ToString()) == recordid)
                    {
                        billtotal = billtotal - float.Parse(dr["Total"].ToString());
                        dr.Delete();
                    }
                      

                }
                records.AcceptChanges();
                labelbilltotal.Text = billtotal.ToString();


            }
        }


        private float calculateTotal()
        {
            float total = 0;
           
            for (int i = 0; i < records.Rows.Count; i++)
            {
                total = +float.Parse(records.Rows[i]["Total"].ToString());

            }
            //labeltotal.Text = total.ToString();
            return total;
           
        }





        public bool saveBill()
        {
            //save to bill


            if (billtype == "Room")
            {
                //if (finaltotal.Text== " ")
                //{
                //    bunifuMaterialTextbox3.Focus();
                //    return false;
                //}



                be.insertquery = "insert into guest_bill values('" + billid + "','" + bunifuMaterialTextbox1.Text + "','" + textBox1.Text + "','" + billtotal + "','" + servicecharge + "','" + vat + "','" + advance + "','0','" + finaltotal.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + billtype + "','Ongoing " + number + "','"+guestentryId+"')";
                int res = be.insertdata();

                //add to bill_content and update amount
                for (int j = 0; j < records.Rows.Count; j++)
                {
                    string prodid = records.Rows[j]["product_id"].ToString();
                    be.insertquery = "insert into bill_content values(Null,'" + billid + "','" + records.Rows[j]["product_id"] + "','" + records.Rows[j]["quantity"] + "','" + records.Rows[j]["rate"] + "','" + records.Rows[j]["total"] + "')";
                    int ress = be.insertdata();
                    if (j > 0)
                    {
                        ingredients.product_id = int.Parse(records.Rows[j]["product_id"].ToString());
                        DataTable ingredientsforProduct = ingredients.ingredientforproduct();

                        for (int i = 0; i < ingredientsforProduct.Rows.Count; i++)
                        {
                            string usedamount = ingredientsforProduct.Rows[i]["used_amount"].ToString();
                            string quantity = records.Rows[j]["Quantity"].ToString();
                            string totalused = (float.Parse(usedamount) * float.Parse(quantity)).ToString();
                            string ingredientid = ingredientsforProduct.Rows[i]["ingredient_id"].ToString();
                            be.updatequery = "update ingredients set amount=amount-'" + totalused + "', updated_on='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where id='" + ingredientid + "'";
                            int resss = be.updatedata();

                        }
                    }

                    
                }





            }

            if (billtype == "Table")
            {

                be.insertquery = "insert into guest_bill values('" + billid + "','" + bunifuMaterialTextbox1.Text + "','" + textBox1.Text + "','" + billtotal + "','" + servicecharge + "','" + vat + "','" + advance + "','0','" + finaltotal.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + billtype + "','Ongoing','"+tableid+"'); select last_insert_id();";
                int res = be.insertdata();

                //todo update selected table set status=filled
                tables.tableid = tableid;
                int ressss = tables.updatebyId();
                

                //


                int resss;
                //add to bill_content and update amount
                for (int j = 0; j < records.Rows.Count; j++)
                {
                    be.insertquery = "insert into bill_content values(Null,'" + billid + "','" + records.Rows[j]["product_id"] + "','" + records.Rows[j]["quantity"] + "','" + records.Rows[j]["rate"] + "','" + records.Rows[j]["total"] + "')";
                    int ress = be.insertdata();
                    
                        ingredients.product_id = int.Parse(records.Rows[j]["product_id"].ToString());
                        DataTable ingredientsforProduct = ingredients.ingredientforproduct();

                        for (int i = 0; i < ingredientsforProduct.Rows.Count; i++)
                        {
                            string usedamount = ingredientsforProduct.Rows[i]["used_amount"].ToString();
                            string quantity = records.Rows[j]["Quantity"].ToString();
                            string totalused = (float.Parse(usedamount) * float.Parse(quantity)).ToString();
                            string ingredientid = ingredientsforProduct.Rows[i]["ingredient_id"].ToString();
                            be.updatequery = "update ingredients set amount=amount-'" + totalused + "', updated_on='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where id='" + ingredientid + "'";
                            resss = be.updatedata();

                        }
                }
            }

           

            return true;


        }

        private void bunifuMaterialTextbox3_OnValueChanged(object sender, EventArgs e)
        {
            if (bunifuMaterialTextbox3.Text != "")
            {
                if (bunifuMaterialTextbox3.Text == "0")
                {
                    vat = 0;

                }
                var res = float.TryParse(bunifuMaterialTextbox3.Text, out vat);
                if (!res)
                {
                    MessageBox.Show("Invalid Vat Amount");
                    return;
                }


            }

            //vat = float.Parse(bunifuMaterialTextbox3.Text);
            totalwithVat = (billtotal - advance) + servicecharge + vat;
            finaltotal.Text = totalwithVat.ToString();


            //if (bunifuMaterialTextbox3.Text != "")
            //{
            //    var res = float.TryParse(bunifuMaterialTextbox3.Text, out vat);
            //    if (!res)
            //    {
            //        MessageBox.Show("Ivalid Vat Amount");
            //        return;
            //    }
            //    totalwithVat = (billtotal - advance) + servicecharge+vat;
            //    finaltotal.Text = totalwithVat.ToString();


            //}

        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {

        }
    }
}
