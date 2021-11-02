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
    public partial class NewOrders : Form
    {
        module.billentry be;
        DataTable recents;
        public NewOrders()
        {
            InitializeComponent();
            be = new module.billentry();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void NewOrders_Load(object sender, EventArgs e)
        {
            Task<DataTable> loadrecents = new Task<DataTable>(be.getrecentOrders);
            loadrecents.Start();
            recents = await loadrecents;
            dataGridView1.DataSource = recents;


        }

        private async void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            DialogResult result = printPreviewDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument1.Print();
                

            }
            Task<bool> deleterecent = new Task<bool>(deleterecents);
            deleterecent.Start();
            bool res = await deleterecent;
            MessageBox.Show("KOT created and recents celared");
            recents.Rows.Clear();
            recents.AcceptChanges();



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

            int y = 0;

            //using (Pen dashed_pen = new Pen(Color.Black, 2))
            //{

            //    dashed_pen.DashStyle = DashStyle.Dash;
            //    e.Graphics.DrawLine(dashed_pen, offset2 - 65, 0, offset2 - 65, 1200);
            //    //  y += 2;

            //}


            g.DrawString("Hotel Rajgharana", font, new SolidBrush(Color.Black), startx+250, starty);
            //g.DrawString("Hotel Rajgharana", font, new SolidBrush(Color.Black), offset2, starty);

            g.DrawString("KOT", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 300, starty + 30);
          //  g.DrawString("KOT", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 80, starty + 30);


            g.DrawString("Printed On: "+DateTime.Now.ToString("yyyy-MM-dd::hh:mm"), new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx+500 , starty + 60);
           // g.DrawString("Printed On: "+ DateTime.Now.ToString("yyyy-MM-dd::hh:mm"), new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 , starty + 60);


            g.DrawString("Table", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx , starty + offset + 90);
            g.DrawString("Product", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx+250, starty + offset + 90);
            g.DrawString("Quantity", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 450, starty + offset + 90);


            //g.DrawString("Table", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 60, starty + offset + 90);
            //g.DrawString("Product", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 50, starty + offset + 90);
            //g.DrawString("Quantity", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 200, starty + offset + 90);


            int off;
            for (int i = 0; i < recents.Rows.Count; i++)
            {
                 off = (i * 20) + 30;

                g.DrawString(recents.Rows[i]["guest_name"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), startx , starty + offset + 100 + off);
                g.DrawString(recents.Rows[i]["product"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), startx+250, starty + offset + 100 + off);
                g.DrawString(recents.Rows[i]["quantity"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), startx + 450, starty + offset + 100 + off);
              
                //g.DrawString(recents.Rows[i]["guest_name"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), offset2 - 60, starty + offset + 100 + off);
                //g.DrawString(recents.Rows[i]["product"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), offset2+50, starty + offset + 100 + off);
                //g.DrawString(recents.Rows[i]["quantity"].ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), offset2 + 200, starty + offset + 100 + off);
              



            }



        }



        private bool deleterecents()
        {
            int j = 0;
            for (int i = 0; i < recents.Rows.Count; i++)
            {
                be.recentid = recents.Rows[i]["recent_id"].ToString();
                int res = be.deleteRecentbyId();
                j++;
            }

            if (recents.Rows.Count == j)
            {
                return true;
            }
            return false;
        }
    }
}
