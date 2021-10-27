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
using System.Drawing.Drawing2D;

namespace Pokhreli.view_controller
{
    public partial class GuestEntry : Form
    {
        string gender;
        guestEntry ge;
        module.Room room;
        List<String> selectedroom;
        DataTable  res = new DataTable();
        List<int> selectedids;
        string seletedroomnumber = "";
        int billno;

        public GuestEntry()
        {
            InitializeComponent();
            ge = new guestEntry();
            labelsaving.Visible = false;
            room = new module.Room();
            selectedroom = new List<string>();
            //bunifuThinButton22.Visible = false;
            //available = new List<string>();
            
        }

        private async void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            seletedroomnumber = "";
            selectedids = new List<int>();




            foreach (object item in checkedListBox1.CheckedItems)
            {
                DataRowView row = item as DataRowView;
                seletedroomnumber +=  row["room_number"]+" ";
                selectedids.Add(int.Parse(row["id"].ToString()));
            }

           
            labelsaving.Visible = Visible;
            if (bunifuMaterialTextbox1.Text == "")
            {
                MessageBox.Show("Guest Name is required");
                bunifuMaterialTextbox1.Focus();
                return;
            }
            if (bunifuMaterialTextbox2.Text == "")
            {
                MessageBox.Show("Nationality is required");
                bunifuMaterialTextbox2.Focus();
                return;
            }
            if (bunifuMaterialTextbox3.Text == "")
            {
                MessageBox.Show("Address is required");
                bunifuMaterialTextbox3.Focus();
                return;
            }

            if (bunifuMaterialTextbox7.Text == "")
            {
                MessageBox.Show("Price of room is required");
                bunifuMaterialTextbox7.Focus();
                return;
            }


            if (bunifuMaterialTextbox8.Text == "")
            {
                MessageBox.Show("Number of guest is required");
                bunifuMaterialTextbox8.Focus();
                return;
            }
            gender = bunifuMaterialTextbox5.Text + " Male " + bunifuMaterialTextbox6.Text + " Female";



            labelsaving.Text = "Saving ....";
            string selectedroom = "";
            foreach (int id in selectedids)
            {
                int idofroom = id;
               string roomnumbe=room.updatequery = "update room set status='Filled',bookedTo='"+billno+"' where id='" + idofroom + "'";

                int res = room.updateRoom();
               
            }
            labelsaving.Text = "Room status updated";



            string query = "insert into guestentry values ('"+billno+"','" + bunifuMaterialTextbox1.Text + "','" + bunifuMaterialTextbox2.Text + "','" + bunifuMaterialTextbox3.Text + "','" + bunifuMaterialTextbox4.Text + "','" + bunifuMaterialTextbox7.Text + "','" + seletedroomnumber + "','" + bunifuMaterialTextbox8.Text + "','" + gender + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + bunifuMaterialTextbox9.Text + "','" + bunifuMaterialTextbox10.Text + "','" + textBox1.Text + "','Inhotel')";
            ge.query = query;



            Task<int> insertrow = new Task<int>(ge.addguestentry);
            insertrow.Start();
            // int row1 = await updateroom;
             int row2 = await insertrow;
            if (row2 == 1 )
            {
                MessageBox.Show("Successfully Saved");
                labelsaving.Visible = false;
                bunifuThinButton22.Visible = false;
            }



        }

        private async void GuestEntry_Load(object sender, EventArgs e)
        {
            Task<string> fetchid = new Task<string>(ge.gethighestId);
            fetchid.Start();
            Task<DataTable> getallrooms = new Task<DataTable>(room.getemptyRooms);
            getallrooms.Start();
           billno = int.Parse(await fetchid)+1;
           res = await getallrooms;
            bunifuCustomLabel5.Text ="SN: A"+billno;
            checkedListBox1.DataSource = res;
            if (res.Rows.Count > 0)
            {
                checkedListBox1.DisplayMember = "room_number";
                checkedListBox1.ValueMember = "id";
                checkedListBox1.SelectedIndex = 0;
            }
           


        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
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
            seletedroomnumber = "";
            foreach (object item in checkedListBox1.CheckedItems)
            {
                DataRowView row = item as DataRowView;
                seletedroomnumber += row["room_number"] + " ";
               
            }


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

            using (Pen dashed_pen = new Pen(Color.Black, 2))
            {

                dashed_pen.DashStyle = DashStyle.Dash;
                e.Graphics.DrawLine(dashed_pen, offset2 - 65, 0, offset2 - 65, 1200);
                //  y += 2;

            }



            g.DrawString("Hotel Hamro Pokhareli", font, new SolidBrush(Color.Black), startx, starty);
            g.DrawString("Hotel Hamro Pokhareli", font, new SolidBrush(Color.Black), offset2, starty);

            g.DrawString("Guest Entry", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 80, starty + 30);
            g.DrawString("Guest Entry", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 80, starty + 30);


            g.DrawString("Date: " + DateTime.Now.ToString(), new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 130, starty + offset);
            g.DrawString("Date: " + DateTime.Now.ToString(), new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 130, starty + offset);



            g.DrawString("Phone: 027-421026", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 20);
            g.DrawString("PAN NO: 0123456789", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 165, starty + offset + 20);

            g.DrawString("Phone: 027-421026", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 20);
            g.DrawString("PAN NO: 0123456789", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 165, starty + offset + 20);

            g.DrawString(bunifuCustomLabel5.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset);
            //g.DrawString(lblnumber.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 40);
            g.DrawString("Name: " + bunifuMaterialTextbox1.Text, new Font("Times New Roman", 14), new SolidBrush(Color.Black), startx - 50, starty + offset + 60);

            g.DrawString(bunifuCustomLabel5.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset);
            //g.DrawString(lblnumber.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 40);
            g.DrawString("Name: " + bunifuMaterialTextbox1.Text, new Font("Times New Roman", 14), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 60);




            g.DrawString("..........................................................................................................................................................................................................................................................", new Font("Times New Roman", 15), new SolidBrush(Color.Black), startx - 200, starty + offset + 70);


            g.DrawString("Nationality: " + bunifuMaterialTextbox2.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 90);
         g.DrawString("Address: " + bunifuMaterialTextbox3.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 110);
             g.DrawString("Phone: " + bunifuMaterialTextbox4.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx -50, starty + offset + 130);
            g.DrawString("No of guest: " + bunifuMaterialTextbox8.Text + "( " + bunifuMaterialTextbox5.Text + " Male - " + bunifuMaterialTextbox6.Text + " Female )", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 150);
            g.DrawString("Purpose: " + bunifuMaterialTextbox9.Text , new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 170);
            g.DrawString("Purpose: " + bunifuMaterialTextbox9.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 170);
            g.DrawString("Rooms: " + seletedroomnumber, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 190);
            g.DrawString("Price: " + bunifuMaterialTextbox7.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 210);
            g.DrawString("Advance: " + bunifuMaterialTextbox10.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 230);
            g.DrawString("Remarks: " , new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx - 50, starty + offset + 250);
            RectangleF rectF1 = new RectangleF(startx - 50, starty + offset + 270, 300, 100);
            e.Graphics.DrawString(textBox1.Text, new Font("Times New Roman", 12), Brushes.Black, rectF1);
            e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(rectF1));

            g.DrawString("Nationality: " + bunifuMaterialTextbox2.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 90);
            g.DrawString("Address: " + bunifuMaterialTextbox3.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 110);
            g.DrawString("Phone: " + bunifuMaterialTextbox4.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 130);
            g.DrawString("No of guest: " + bunifuMaterialTextbox8.Text + "( " + bunifuMaterialTextbox5.Text + " Male - " + bunifuMaterialTextbox6.Text + " Female )", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 150);
            g.DrawString("Purpose: " + bunifuMaterialTextbox9.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 170);
            g.DrawString("Rooms: " + seletedroomnumber, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 190);
            g.DrawString("Price: " + bunifuMaterialTextbox7.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 210);
            g.DrawString("Advance: " + bunifuMaterialTextbox10.Text, new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 230);
            g.DrawString("Remarks: ", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 - 50, starty + offset + 250);
            RectangleF rectF2 = new RectangleF(offset2 - 50, starty + offset + 270, 300, 100);
            e.Graphics.DrawString(textBox1.Text, new Font("Times New Roman", 12), Brushes.Black, rectF2);
            e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(rectF2));

            g.DrawString("..........................................................................................................................................................................................................................................................", new Font("Times New Roman", 15), new SolidBrush(Color.Black), startx - 200, starty + offset + 390);

            g.DrawString("Recieved by: ...............................", new Font("Times New Roman", 12), new SolidBrush(Color.Black), startx + 50, starty + offset  + 390);
            g.DrawString("Recieved by: ...............................", new Font("Times New Roman", 12), new SolidBrush(Color.Black), offset2 + 50, starty + offset  + 390);






        }



    }
}
