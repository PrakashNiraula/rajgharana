using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokhreli.dbConn;
using System.Data;

namespace Pokhreli.module
{
   public class myProducts
    {


        public string query;
        dbConnection db;
        public int product_id;
        public int record_id;
        public float updateamount;

        public myProducts()
        {
            db = new dbConnection();
        }
         

        public int addrecord()
        {
            return db.ExecuteQuery(query);
        }

        public DataTable ingredientforproduct()
        {
            return db.GetDataTable("select pig.id as record_id, ig.id as ingredient_id,ig.name,ig.unit,pig.used_amount from ingredients ig, product_ingredient pig, myproducts mp where pig.ingredient_id=ig.id and pig.product_id=mp.id and mp.id=" + product_id);
        }


        public string gethighestId()
        {
            return db.ExecuteScalar("select max(id) from myproducts").ToString();

        }

        public DataTable getallProducts()
        {
            return db.GetDataTable("select id,name,price from myproducts where visible='1'");
        }



        public int deleteMyProduct() {
            return db.ExecuteQuery("delete from myproducts where id='"+product_id+"'");

        }

        public int deleteIngredientsList()
        {
            return db.ExecuteQuery("delete from product_ingredient where product_id='" + product_id + "'");
        }


        public int deleteingredientOfproduct()
        {
            return db.ExecuteQuery("delete from product_ingredient where id='" + record_id + "'");
        }

        public DataTable addnewIngredientToProduct()
        {
           
            if (db.ExecuteQuery(query) == 1)
            {
                return ingredientforproduct();
                
            }
            return null;
        }

        public int updateamountofIngredient()
        {
            return db.ExecuteQuery("update product_ingredient set used_amount='" + updateamount + "' where id='" + record_id + "' ");

        }
    }

}
