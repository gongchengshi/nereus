using System;
using Nereus.Models;
using System.Data.SqlClient;

namespace DatabaseInitialization
{
    class Program
   {
      static void Main(string[] args)
      {
         TestData.Initialize(new NereusDb());
      }

      static bool TableExists(SqlConnection dbConn, string tableName)
      {
         try
         {
            var cmd = new SqlCommand("select 1 from " + tableName + " where 1 = 0", dbConn);
            cmd.ExecuteNonQuery();
            return true;
         }
         catch(Exception)
         {
            return false;
         }         
      }
   }
}
