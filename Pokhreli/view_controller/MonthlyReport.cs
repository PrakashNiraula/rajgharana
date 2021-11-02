using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using excel = Microsoft.Office.Interop.Excel;

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

        public object ErrorHandler { get; private set; }

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

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {

                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                 string   selectedFolder = dialog.SelectedPath+"\\report.xls";


                    try
                    {
                        int ColumnsCount;

                        if (res == null || (ColumnsCount = res.Columns.Count) == 0)
                            throw new Exception("ExportToExcel: Null or empty input table!\n");

                        // load excel, and create a new workbook
                        Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                        Excel.Workbooks.Add();

                        // single worksheet
                        Microsoft.Office.Interop.Excel._Worksheet Worksheet = Excel.ActiveSheet;

                        object[] Header = new object[ColumnsCount];

                        // column headings               
                        for (int i = 0; i < ColumnsCount; i++)
                            Header[i] = res.Columns[i].ColumnName;

                        Microsoft.Office.Interop.Excel.Range HeaderRange = Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, ColumnsCount]));
                        HeaderRange.Value = Header;
                        HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        HeaderRange.Font.Bold = true;

                        // DataCells
                        int RowsCount = res.Rows.Count;
                        object[,] Cells = new object[RowsCount, ColumnsCount];

                        for (int j = 0; j < RowsCount; j++)
                            for (int i = 0; i < ColumnsCount; i++)
                                Cells[j, i] = res.Rows[j][i];

                        Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[2, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[RowsCount + 1, ColumnsCount])).Value = Cells;

                        // check fielpath
                        if (selectedFolder != null && selectedFolder != "")
                        {
                            try
                            {
                                Worksheet.SaveAs(selectedFolder);
                                Excel.Quit();
                                MessageBox.Show("Excel file saved!");
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                                    + ex.Message);
                            }
                        }
                        else    // no filepath is given
                        {
                            Excel.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: \n" + ex.Message);
                    }
                }





            }

        


            //OpenFileDialog folderBrowser = new OpenFileDialog();
            //// Set validate names and check file exists to false otherwise windows will
            //// not let you select "Folder Selection."
            //folderBrowser.ValidateNames = false;
            //folderBrowser.CheckFileExists = false;
            //folderBrowser.CheckPathExists = true;
            //// Always default to Folder Selection.
            //folderBrowser.FileName = "report.xlx";
            //if (folderBrowser.ShowDialog() == DialogResult.OK)
            //{
            //    string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
            //    string name = "";
            //}



            //using (var fbd = new FolderBrowserDialog())
            //{
            //    DialogResult result = fbd.ShowDialog();

            //    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            //    {
            //        string[] files = Directory.GetFiles(fbd.SelectedPath);

            //        System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
            //    }
            //}


            //var lines = new List<string>();

            //string[] columnNames = res.Columns
            //    .Cast<DataColumn>()
            //    .Select(column => column.ColumnName)
            //    .ToArray();

            //var header = string.Join(",", columnNames.Select(name => $"\"{name}\""));
            //lines.Add(header);

            //var valueLines = res.AsEnumerable()
            //    .Select(row => string.Join(",", row.ItemArray.Select(val => $"\"{val}\"")));

            //lines.AddRange(valueLines);

            //File.WriteAllLines("excel.csv", lines);
        }
    }

}
