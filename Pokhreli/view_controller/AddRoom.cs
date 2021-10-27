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
    public partial class AddRoom : Form
    {
        module.Room room;
        int res;
        public DataRow dr;
       
        public AddRoom(DataRow dr)
        {
            InitializeComponent();
            room = new module.Room();
            res = new int();
            this.dr = dr;
        }

        private async void bunifuThinButton22_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("Room number cannot be empty");
                textBox1.Focus();
                return;
            }
            if (dr == null)
            {
                room.query = "insert into room values(Null,'" + bunifuMaterialTextbox1.Text + "','" + textBox1.Text + "','Empty','0')";

              
                Task<int> runquery = new Task<int>(room.insertroom);
                labelinfo.Text = "Saving...";
                runquery.Start();
                res = await runquery;

                if (res == 1)
                {
                    labelinfo.Text = "Successfully saved.";
                    this.Close();
                }
                else
                {
                    labelinfo.Text = "Error inserting data";
                }
            }else
            {

                room.updatequery = "update room set facilities='" + textBox1.Text + "' where id='" + dr["id"].ToString() + "'";
                Task<int> updateroom = new Task<int>(room.updateRoom);
                updateroom.Start();
                if(await updateroom == 1)
                {
                    labelinfo.Text = "Successfully Updated.";
                    this.Close();
                }


            }

           

        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void AddRoom_Load(object sender, EventArgs e)
        {
            if (dr != null)
            {
                bunifuMaterialTextbox1.Text = dr["room_number"].ToString();
                textBox1.Text = dr["facilities"].ToString();
                bunifuThinButton22.ButtonText = "Update";
            }
        }
    }
}
