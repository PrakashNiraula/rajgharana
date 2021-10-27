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
    public partial class Home : Form
    {
        DataTable guestlist;
        module.guestEntry ge;
        module.billentry be;
        module.expenses exp;
        public Home()
        {
            InitializeComponent();
            ge = new module.guestEntry();
            be = new module.billentry();
            exp = new module.expenses();
        }

        private async void Home_Load(object sender, EventArgs e)
        {
            loaddata();

        }

        public async void loaddata()
        {
            //loadguestlist
            Task<DataTable> getallguests = new Task<DataTable>(ge.getallguests);
            getallguests.Start();
            guestlist = await getallguests;
            bunifuTileButton1.LabelText = "Guest: " + guestlist.Rows.Count;


            //sales tooday
            be.query = "select * from guest_bill where date='" +DateTime.Now.ToString("yyyy-MM-dd") + "'";
            Task<DataTable> loadbill = new Task<DataTable>(be.getdata);
            loadbill.Start();
            DataTable res = await loadbill;
            float total=0;
            foreach(DataRow dr in res.Rows)
            {
                total = total + float.Parse(dr["total"].ToString()) + float.Parse(dr["service_charge"].ToString()) + float.Parse(dr["vat"].ToString());
            }
            bunifuTileButton3.LabelText = "Sales Today: " + total;


            //purchase today
            be.query = "select * from purchase where date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            Task<DataTable> loadpurchase = new Task<DataTable>(be.getdata);
            loadpurchase.Start();
           DataTable res2 = await loadpurchase;
            float total2 = 0;
            foreach (DataRow dr in res2.Rows)
            {

                total2 += float.Parse(dr["cost"].ToString());

            }
            bunifuTileButton4.LabelText = "Purchase Today: " + total2; 




            //expenses today
            exp.date = DateTime.Now.ToString("yyyy-MM-dd");
            Task<DataTable> expensestoday = new Task<DataTable>(exp.searchbydate);
            expensestoday.Start();
            DataTable res3 = await expensestoday;


            float total3 = 0;
            foreach (DataRow dr in res3.Rows)
            {

                total3 += float.Parse(dr["amount"].ToString());

            }
            bunifuTileButton5.LabelText = "Expense Today: " + total3;


        }

        private void bunifuTileButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
