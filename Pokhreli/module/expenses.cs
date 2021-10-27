using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokhreli.module
{
    public class expenses
    {

        public int id;
        public string name;
        public float amount;
        public string description;
        public string date;
        dbConn.dbConnection db;



        public expenses()
        {
            db = new dbConn.dbConnection();

        }


        //add expense record to database
        public int addrecord()
        {
            if(name!=null && amount != 0)
            {
                try
                {
                    return db.ExecuteQuery("insert into expenses values(Null,'" + name + "','" + amount + "','" + description + "','"+date+"')");

                }
                catch(Exception ex)
                {
                    throw ex;
                }

            }
            return 0;
        }


        public int deleterecord()
        {
            if (id != 0)
            {
                try
                {
                    return db.ExecuteQuery("delete from expenses where id='" + id + "'");
                }
                catch(Exception ex)
                {
                    throw ex;
                }
           
            }
            return 0;
        }

        public DataTable getall()

        {

            try
            {
                return db.GetDataTable("select * from expenses");
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public DataTable searchbydate()
        {
            if (date != null)
            {
                try
                {
                    return db.GetDataTable("select * from expenses where date='" + date + "'");
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return null;
        }


        public DataTable getmonthly()
        {
            if (date != "")
            {
                try
                {
                    return db.GetDataTable("select * from expenses where date like '" + date + "%'");

                }catch(Exception ex)
                {
                    throw ex;
                }
            }
            return null;
        }







    }
}
