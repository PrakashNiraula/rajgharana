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
    public partial class ViewData : Form
    {
        string table;
        string date;
        billentry be;
        guestEntry ge;
        module.Room room;
        DataTable res;
        DataTable records;
        

        public ViewData()
        {
            InitializeComponent();
           be= new billentry();
            ge = new guestEntry();
            room = new module.Room();
            res = new DataTable();
            records = new DataTable();
         
            records.Columns.Add("ID", typeof(string));
            records.Columns.Add("Particular", typeof(string));
            records.Columns.Add("Rate", typeof(string));
            records.Columns.Add("Quantity", typeof(string));

            records.Columns.Add("Total", typeof(string));



        }

        private async void ViewData_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
           be.date = DateTime.Now.ToString("yyyy-dd-MM");
            be.table = "guestentry";
            dataGridView1.Columns[0].Visible = false;
            





        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Visible = true;
            fordate.Visible = true;
            if (comboBox1.SelectedIndex == 2)
            {
                executequery("rooms","");


            }
            else if (comboBox1.SelectedIndex == 0)
            {
                executequery("guestentry",DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (comboBox1.SelectedIndex == 1)
            {

                executequery("guest_bill", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (comboBox1.SelectedIndex == 3)
            {

                executequery("purchase", DateTime.Now.ToString("yyyy-MM-dd"));
            }






        }

        private async void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == 0)
            {
                executequery("guestentry", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (comboBox1.SelectedIndex == 1)
            {

                executequery("guest_bill", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (comboBox1.SelectedIndex == 3)
            {

                executequery("purchase", DateTime.Now.ToString("yyyy-MM-dd"));
            }




        }

        private async void executequery(string table,string date)
        {
            dateTimePicker1.Visible = true;
            fordate.Visible = true;
            dataGridView1.Columns[0].Visible = false;
            if(table == "rooms")
            {
                ge.query = "select * from room";
                Task<DataTable> loadrooms = new Task<DataTable>(ge.getguestRecord);
                loadrooms.Start();
                res = await loadrooms;
                dataGridView1.DataSource = res;
                dateTimePicker1.Visible = false;
                fordate.Visible = false;
                return;
            }

            if (table== "guestentry")
            {
                ge.query = "select * from guestentry where date='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'";
                Task<DataTable> loadguestentry = new Task<DataTable>(ge.getguestRecord);
                loadguestentry.Start();
                res = await loadguestentry;
                dataGridView1.DataSource = res;
                return;
            }
            if(table== "guest_bill")
            {
                be.query = "select * from guest_bill where date='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'";
                Task<DataTable> loadbill = new Task<DataTable>(be.getdata);
                loadbill.Start();
                res = await loadbill;
                dataGridView1.DataSource = res;
                dataGridView1.Columns[0].Visible = true;
                return;
            }

            if (table == "purchase")
            {
                be.query = "select * from purchase where date='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'";
                Task<DataTable> loadpurchase = new Task<DataTable>(be.getdata);
                loadpurchase.Start();
                res = await loadpurchase;
                dataGridView1.DataSource = res;
                return;
            }



        }



        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            DataTable res2=null ;
            
         
            int billid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
            string command = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (command.ToLower() == "view")
            {
                DataRow dr = res.Rows[e.RowIndex];
                //select mp.name,bct.quantity,bct.rate,bct.total from myproducts mp,bill_content bct, guest_bill gb where bct.bill_id=gb.id and mp.id=bct.product_id and bct.bill_id=21
                be.query = "select mp.name,bct.quantity,bct.rate,bct.total from myproducts mp,bill_content bct, guest_bill gb where bct.bill_id=gb.id and mp.id=bct.product_id and bct.bill_id='" + billid + "'";


                Task<DataTable> getbillcontet = new Task<DataTable>(be.getdata);
                getbillcontet.Start();
               res2 = await getbillcontet;

                for (int i = 0; i < res2.Rows.Count; i++)
                {

                    

                    records.Rows.Add((i + 1), res2.Rows[i]["name"].ToString(), res2.Rows[i]["rate"].ToString(), res2.Rows[i]["quantity"].ToString(), res2.Rows[i]["total"].ToString());


                }
                
                PrintBill pb = new PrintBill(records,dr["guest_name"].ToString(),dr["billtype"].ToString(),dr["description"].ToString(),dr["advance"].ToString(),dr["status"].ToString(),billid.ToString(),dr["service_charge"].ToString(),dr["vat"].ToString(), int.Parse(dr["tid"].ToString()));
                pb.ShowDialog();
                records.Clear();

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            MonthlyReport mr = new MonthlyReport();
            mr.ShowDialog();
        }
    }
}
