using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokhreli.dbConn;
using System.Data;

namespace Pokhreli.module
{
    public class guestEntry
    {
        dbConnection db;
        public string query = "";
        public string entryId;
        public string month;

        public guestEntry()
        {
            db = new dbConnection();

        }



        public string gethighestId()
        {
            string res = db.ExecuteScalar("select max(id) from guestentry");

            if (res != "")
            {
                return res;
            }
            else
            {
                return "0";
            }

        }


        public int addguestentry()
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


        public DataTable getallguests()
        {
            try
            {
                return db.GetDataTable("select * from guestentry where status='Inhotel'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable getguestRecord()
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


        public int guestentryBilled()
        {

            try
            {
                return db.ExecuteQuery("update guestentry set status='Billed at " + DateTime.Now.ToString() + "' where id='"+entryId+"'");
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }


        public DataTable getbyMonth()
        {
            try
            {
                return db.GetDataTable("select * from guestentry where date like '" + month + "%'");

            }catch(Exception ex)
            {
                throw ex;
            }
        }




    }
}
