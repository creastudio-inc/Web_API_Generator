using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace BusinessLogic
{
    public class DataAccess
    {
        public DataTable GetInformationForDataTable(String tableName, String cnxString)
        {
            //GetInformation for table
            String query = String.Format("EXEC sp_help {0}", tableName);

            SqlConnection cnx = new SqlConnection(cnxString);
            IDataReader dr = null;

            try
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand(query, cnx);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cnx.Close();
                return ds.Tables[1];

            }
            catch (Exception ex)
            {
                cnx.Dispose();
                throw;
            }
        }
        public List<String> GetTableInDataBase(String cnxString)
        {
            SqlConnection cnx = null;
            try
            {
                List<String> lstTable = new List<string>();
                cnx = new SqlConnection(cnxString);
                cnx.Open();
                String query = "select name as tblName from sysobjects where xtype='U'";
                SqlCommand cmd = new SqlCommand(query, cnx);

                IDataReader dr = cmd.ExecuteReader();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        int ordTblName = dr.GetOrdinal("tblName");
                        if (!dr.IsDBNull(ordTblName))
                            lstTable.Add(dr.GetString(ordTblName));
                    }
                }

                return lstTable;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cnx != null)
                    cnx.Dispose();
            }
        }
        public List<String> GetLstDataBases(String cnxString)
        {
            SqlConnection cnx = null;
            try
            {
                List<String> lstDb = new List<string>();
                cnx = new SqlConnection(cnxString);

                cnx.Open();

                String query = "USE master SELECT name as bdd FROM sysdatabases Go";

                SqlCommand cmd = new SqlCommand(query, cnx);
                IDataReader dr = cmd.ExecuteReader();
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        int ordName = dr.GetOrdinal("bdd");
                        if (!dr.IsDBNull(ordName))
                            lstDb.Add(dr.GetString(ordName));
                    }
                }
                cnx.Close();
                return lstDb;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cnx != null)
                    cnx.Dispose();
            }
        }
    }
}
