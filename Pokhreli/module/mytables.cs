using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokhreli.module
{
    public class mytables
    {
        dbConn.dbConnection db;


        public int tableid;
        public string tableno;
        public string status;

        public mytables()
        {
            db = new dbConn.dbConnection();
        }


        public int addtable()
        {
            if (tableno != null)
            {
                string query = "insert into tables values(Null,'" + tableno + "','Empty')";
                try
                {
                    return db.ExecuteQuery(query);

                }catch(Exception ex)
                {
                    throw ex;
                }
              
            }
              return 0;
        }

        public DataTable getalltables()
        {
            try
            {
                return db.GetDataTable("select * from tables");


            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getemptytables()
        {
            try
            {
                return db.GetDataTable("select * from tables where status='Empty'");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public int deletebyId()
        {
            if (tableid != null)
            {
                try
                {
                    return db.ExecuteQuery("delete from tables where id='" + tableid + "'");
                }catch(Exception ex)
                {
                    throw ex;
                }
                
            }
            return 0;
        }


        public int updatebyId()
        {
            try
            {
                if (tableid != null)
                {
                    return db.ExecuteQuery("update tables set status='Filled' where id='" + tableid + "'");
                }
              

            }catch(Exception ex)
            {
                throw ex;
            }
            return 0;

        }

        public int emptytable()
        {
            try
            {
                if (tableid != null)
                {
                    return db.ExecuteQuery("update tables set status='Empty' where id='" + tableid + "'");
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;

        }






    }
}
