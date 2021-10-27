using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokhreli.dbConn;
using System.Data;

namespace Pokhreli.module
{
    public class Ingredients
    {
        dbConnection db;
        public string addquery;
        public string updatequery;
        public int product_id;
        public string month;

        public Ingredients()
        {
            db = new dbConnection();
        }
        public int addrecord()
        {
            return db.ExecuteQuery(addquery);
        }

        public int updaterecord()
        {
            return db.ExecuteQuery(updatequery);
        }


        public DataTable getallingredients()
        {

            return db.GetDataTable("select * from ingredients");

        }

        public DataTable ingredientforproduct()
        {
            return db.GetDataTable("select ig.id as ingredient_id,ig.name,ig.unit,pig.used_amount from ingredients ig, product_ingredient pig, myproducts mp where pig.ingredient_id=ig.id and pig.product_id=mp.id and mp.id=" + product_id);
        }


        public DataTable getbyMonth()
        {
            try
            {
                return db.GetDataTable("select * from purchase where date like '" + month + "%'");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
