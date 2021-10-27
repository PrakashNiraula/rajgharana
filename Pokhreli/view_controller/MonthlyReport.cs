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
    public partial class MonthlyReport : Form
    {

        string selectedtable;
        string selectedmonth;
        module.guestEntry ge;
        module.billentry be;
        module.Ingredients ig;
        DataTable res;
        module.expenses exp;
        public MonthlyReport()
        {
            InitializeComponent();
            ge = new module.guestEntry();
            be = new module.billentry();
            ig = new module.Ingredients();
            exp = new module.expenses();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MonthlyReport_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            // Display the date as "Mon 27 Feb 2012".  
            dateTimePicker1.CustomFormat = "yyyy-MM";
            comboBox1.SelectedIndex = 0;
            dateTimePicker1.CalendarMonthBackground = Color.Green;
                
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            check();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            check();


        }

        private async void check()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                selectedtable = "Guest Entry Report";
            }
            if (comboBox1.SelectedIndex == 1)
            {
                selectedtable = "Guest Bill Report";
            }
            if (comboBox1.SelectedIndex == 2)
            {
                selectedtable = "Purchase History Report";
            }
            if (comboBox1.SelectedIndex == 3)
            {
                selectedtable = "Expenses";
            }
            selectedmonth = dateTimePicker1.Value.ToString("yyyy-MM");


            Task<DataTable> loaddatares = new Task<DataTable>(loaddata);
            loaddatares.Start();

            dataGridView1.DataSource = await loaddatares ;
          
        }



        public DataTable loaddata()
        {
         
                if(selectedtable=="Guest Entry Report")
            {


                ge.month = selectedmonth;
             res=ge.getbyMonth();
                

            }
            
               

                if(selectedtable=="Guest Bill Report")
            {
                be.month = selectedmonth;
                res= be.getbyMonth();

            }



                if(selectedtable=="Purchase History Report")
            {
                ig.month = selectedmonth;
             res= ig.getbyMonth();

            }

            if (selectedtable == "Expenses")
            {
                exp.date = selectedmonth;
                res = exp.getmonthly();

            }

            return res;
           

        }






    }
}
