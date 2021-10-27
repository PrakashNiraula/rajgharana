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
using Pokhreli.dbConn;
namespace Pokhreli.view_controller
{
    public partial class Room : Form
    {
        module.Room room;
        DataTable res;
        dbConnection db;
        public Room()
        {
            InitializeComponent();
            room = new module.Room();
        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            AddRoom ar = new AddRoom(null);
            ar.ShowDialog();
            loaddata();
         
        }

        public async void  loaddata()
        {

            Task<DataTable> loadrooms = new Task<DataTable>(room.getallrooms);
            loadrooms.Start();
            res = await loadrooms;
            res.AcceptChanges();
            dataGridView1.DataSource = res;
          
            return;
        }

        private void Room_Load(object sender, EventArgs e)
        {
            loaddata();

        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            room.roomid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
            string command = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (command.ToLower() == "update")
            {
                DataRow dtr = res.Rows[e.RowIndex];
                AddRoom ar = new AddRoom(dtr);
                ar.ShowDialog();
                loaddata();

            }
            if (command.ToLower() == "delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this room?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    Task<int> deleteRoom = new Task<int>(room.deleteRoom);
                    deleteRoom.Start();
                    if(await deleteRoom == 1)
                    {
                        for (int i = res.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = res.Rows[i];
                            if (int.Parse(dr["id"].ToString()) == room.roomid)
                                dr.Delete();
                          
                        }
                        res.AcceptChanges();
                    }
                 
                }
               

            }
        }

    }
}
