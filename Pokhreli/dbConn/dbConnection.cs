using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace Pokhreli.dbConn
{
    public class dbConnection
    {


        MySqlConnection conn;

        public dbConnection()
        {
         conn = new MySqlConnection("server = database-1.cmn2hbmgvegk.ap-south-1.rds.amazonaws.com; user id= admin; password =#pvug209y; database = rajgharana; persistsecurityinfo = False;");
           // conn = new MySqlConnection("server = localhost; user id= root; password =; database = hotel_pokhreli; persistsecurityinfo = False;");

        }

        public DataTable GetDataTable(string query)
        {





            DataTable dt;
            MySqlDataReader dr;
            MySqlCommand cmd = new MySqlCommand(query, conn);
            try
            {

                cmd.Connection = conn;
                cmd.Connection.Open();
                var dataReader = cmd.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(dataReader);
                dataReader.Close();
                return dataTable;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();


            }











            //MySqlDataAdapter da;

            //try
            //{

            //    da = new MySqlDataAdapter(query, conn);
            //    DataSet ds = new DataSet();
            //    da.Fill(ds);
            //    if (ds.Tables.Count > 0)
            //        return ds.Tables[0];

            //    else
            //        return null;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{

            //    conn.Close();
            //}

        }
        public int ExecuteQuery(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            try
            {

                cmd.Connection = conn;
                cmd.Connection.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        public string ExecuteScalar(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            try
            {

                cmd.Connection = conn;
                cmd.Connection.Open();
                return Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                return "0";
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }




    }




}
