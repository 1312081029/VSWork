using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BLL
{
    public class DB
    {
   
    public static SqlConnection CreateConnection()
    {
        string dbConnString = @"server=DESKTOP-AAPGKPD\QGQ;uid=sa;pwd=512688;database=MyData";
        System.Data.SqlClient.SqlConnection con = new SqlConnection(dbConnString);
        return con;
    }
        public static DataSet GetDataSet(string sql)
        {
            using (SqlConnection con = DB.CreateConnection())
            {
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(sql, con);
                DataSet ds = new DataSet();
                ada.Fill(ds);
                ada.Dispose();
                con.Close();
                return ds;
            }
        }
    
        public static int ExecSQL(string execSQL)
        {
            using (SqlConnection conTempExec = DB.CreateConnection())
            {

                conTempExec.Open();
                SqlCommand cmdTempExec = new SqlCommand(execSQL, conTempExec);
                int ExecCount = cmdTempExec.ExecuteNonQuery();
                cmdTempExec.Dispose();
                conTempExec.Close();
                conTempExec.Dispose();
                return ExecCount;
            }
        }

        public static string ReturnOneSQL(string sSQL)
        {
            string bcValue = "";
            using (DataSet ds = DB.GetDataSet(sSQL))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow sdr = ds.Tables[0].Rows[0];
                    bcValue = sdr[0].ToString();
                }
                return bcValue;
            }
        }

     
      

        public static void ExecuteSqlTran(List<string> SQLStringList)
        {
            using (SqlConnection conn = DB.CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }
        public static bool ExecuteSqlTranRes(List<string> SQLStringList)
        {
            bool res = false;
            using (SqlConnection conn = DB.CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    res = true;
                    tx.Commit();
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
                return res;
            }
        }
        public static int GetDataCount(string sql)
        {
            using (SqlConnection con = DB.CreateConnection())
            {
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(sql, con);
                DataSet ds = new DataSet();
                ada.Fill(ds);
                ada.Dispose();
                con.Close();
                return ds.Tables[0].Rows.Count;
            }
        }
    }

}
