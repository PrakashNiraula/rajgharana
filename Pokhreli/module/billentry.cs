using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokhreli.dbConn;
using System.Data;

namespace Pokhreli.module
{
    public class billentry
    {
        dbConnection db;
      public  string date;
       public string table;
        public string query;
        public string insertquery;
        public string updatequery;
        public string month;
        public string billid;
        public float newamount;
        public billentry()
        {
            db = new dbConnection();

        }

        public string gethighestid()
        {
            string res = db.ExecuteScalar("select max(id) from guest_bill");

            if (res!= "")
            {
                return res;
            }
            else
            {
                return "0";
            }

        }


        public DataTable getdata()
        {
            
            return db.GetDataTable(query);


        }

        public int insertdata()
        {
            try
            {
                return db.ExecuteQuery(insertquery);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int updatedata()
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





        public DataTable getbyMonth()
        {
            try
            {
                return db.GetDataTable("select * from guest_bill where date like '" + month + "%'");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int updatebill()
        {
            try
            {
                if (billid != null)
                {
                    return db.ExecuteQuery("update guest_bill set total=total+'" + newamount + "',finalRemaining=finalRemaining+'"+newamount+"' where id='" + billid + "'");
                }
               

            }catch(Exception ex)
            {
                throw ex;
            }
            return 0;

        }


        public int checkoutbilling()
        {
            try
            {
                return db.ExecuteQuery("update guest_bill set status='Checked out' where id='" + billid + "'");

            }catch(Exception ex)
            {
                throw ex;
            }
            return 0;
        }




    }
}
