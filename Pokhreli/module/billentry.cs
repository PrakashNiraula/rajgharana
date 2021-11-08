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
        public string recentid;
        public string recordid;

        public string vat;
        public string discount;
        public string servicecharge;
        public string priceaftercharges;



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
                var res= db.ExecuteQuery("update guest_bill set status='Checked out',service_charge='"+servicecharge+"',discount='"+discount+"',vat='"+vat+"',finalRemaining='"+priceaftercharges+"' where id='" + billid + "'");
                return res;
            }catch(Exception ex)
            {
                throw ex;
            }
            return 0;
        }


        public DataTable getrecentOrders()
        {
            try
            {
                return db.GetDataTable("select r.id as recent_id,gb.guest_name,p.name as product,r.quantity from guest_bill gb, recent r, myproducts p where r.billid=gb.id and r.productid=p.id");


            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public int deleteRecentbyId()
        {
           try
            {
                return db.ExecuteQuery("delete from recent where id='" + recentid + "' ");

            }catch(Exception ex)
            {
                throw ex;
            }
        }


        public int deletebillcontent()
        {


            try
            {

                DataTable res2 = db.GetDataTable("select * from bill_content where id='" + recordid + "'");
                string billid = res2.Rows[0]["bill_id"].ToString();
                string total= res2.Rows[0]["total"].ToString();
                var res3 = db.ExecuteQuery("update guest_bill set total=total-'" + float.Parse(total) + "',finalRemaining=finalRemaining-'" + float.Parse(total) + "' where id='" + billid + "'");
                var res=db.ExecuteQuery("delete from bill_content where id='"+ recordid + "'");
                if(res3==1 && res == 1)
                {
                    return 1;
                }
                return 0;
            }
            catch (Exception ex) {
                throw ex;
            }

        }




    }
}
