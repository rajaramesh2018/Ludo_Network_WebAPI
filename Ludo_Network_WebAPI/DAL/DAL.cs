using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

namespace Ludo_Network_WebAPI.DAL
{
    public class DAL
    {

        static string con_str = System.Configuration.ConfigurationManager.ConnectionStrings["Ludo_Con1"].ConnectionString;

        static public DataSet getDtata(string query)
        {
            SqlDataAdapter da = new SqlDataAdapter(query, con_str);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }





    }
}