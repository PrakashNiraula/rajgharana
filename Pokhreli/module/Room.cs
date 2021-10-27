using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokhreli.dbConn;
using System.Data;
namespace Pokhreli.module
{
    
   public class Room
    {
        dbConnection db;
        public string query;
       public int roomid;
        public string updatequery;
        public string billid;

        public Room()
        {
          db  = new dbConnection();
        }
        

        public int insertroom()
        {
            try
            {
                return db.ExecuteQuery(query);
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

       public DataTable getallrooms()
        {
            try
            {
                return db.GetDataTable("select id,room_number,facilities,status from room");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable executequery()
        {
            try
            {
                return db.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getemptyRooms()
        {
            try
            {
                return db.GetDataTable("select * from room where status='Empty'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    

        public int deleteRoom()
        {
            try
            {
                return db.ExecuteQuery("delete from room where id='" + roomid + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }




        }

        public int updateRoom()
        {
            try
            {
                return db.ExecuteQuery(updatequery);
            }
            catch (Exception ex)
            {
                throw ex;
            }




        }

        public DataTable getallRoomsbookedTobill()
        {
            return db.GetDataTable("select * from room where bookedTo='" + billid + "'");

        }

        

    }
}
