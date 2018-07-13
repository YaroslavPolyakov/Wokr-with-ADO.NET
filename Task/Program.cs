using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Task
{
    class Program
    {
        private static void ReadData(string connectionString)
        {
            string select = "SELECT * FROM Sitecore";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (SqlCommand command = new SqlCommand(select, connection))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t\t\t\t\t{2}\t{3}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
                    while (reader.Read())
                    {
                        object id = reader.GetValue(0);
                        object item_id = reader.GetValue(1);
                        object entity_id = reader.GetValue(2);
                        object type = reader.GetValue(3);
                        Console.WriteLine("{0} \t{1} \t{2} \t\t{3}", id, item_id, entity_id, type);
                    }
                }
                reader.Close();
            }
        }
        private static int ReturnCount(string connectionString, string entityid)
        {
            string select = string.Format("select count(*) from Sitecore where EntityId='{0}';", entityid);
            int count = 0;

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (SqlCommand command = new SqlCommand(select, connection))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = reader.GetInt32(0);
                    }
                }
                reader.Close();
            }
            return count;
        }
        private static void Post(string connectionString, int id, string entityid, string type)
        {
            try
            {
                string sqlInsert = string.Format("insert into Sitecore (Id,ItemId,EntityId,Type) values (@Id,NEWID(),@EntityId,@Type);");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@EntityId", entityid);
                    command.Parameters.AddWithValue("@Type", type);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void Put(string connectionString, string entityid, string type)
        {
            try
            {
                string sqlInsert = string.Format("update Sitecore set Type='{0}' where EntityId='{1}';", entityid, type);
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            int count = ReturnCount(connectionString, "22");
            if (count == 0)
            {
                Post(connectionString, 22, "22", "22");
            }
            else
            {
                Put(connectionString, "1", "22");
            }
            ReadData(connectionString);
        }
    }
}
