using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.OleDb;

public class Filter
{
    public string keywords;
    public string fx;
    public string cc;
    public string zy;
    public string sd;
    public string ed;
    public string a;
}
    
public class ComboboxItem
{
    public string Text { get; set; }
    public object Value { get; set; }

    public override string ToString()
    {
        return Text;
    }

    public ComboboxItem(string text, string value)
    {
        this.Text = text;
        this.Value = value;
    }
} 

/// <summary>
/// DataAccess 的摘要说明
/// </summary>
public class DB
{
    protected static OleDbConnection conn = new OleDbConnection();
    protected static OleDbCommand comm = new OleDbCommand();
    public DB()
    {
        //init
    }
    private static void openConnection()
    {
        if (conn.State == ConnectionState.Closed)
        {
            conn.ConnectionString = WindowsFormsApplication1.Properties.Settings.Default.dbconn;
            comm.Connection = conn;
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

    }//打开数据库

    private static void closeConnection()
    {
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
            conn.Dispose();
            comm.Dispose();
        }
    }//关闭数据库


    //返回执行后的第一行的数据
    public static string excuteScalar(string sqlstr)
    {
        try
        {
            openConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlstr;
            return comm.ExecuteScalar().ToString();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            closeConnection();
        }
    }

    public static void excuteSql(string sqlstr)
    {
        try
        {
            openConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlstr;
            comm.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        { closeConnection(); }
    }//执行sql语句

    public static void excuteSql(OleDbCommand cmd)
    {
        try
        {
            openConnection();
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        { closeConnection(); }
    }//执行sql语句

    public static OleDbDataReader dataReader(string sqlstr)
    {
        OleDbDataReader dr = null;
        try
        {
            openConnection();
            comm.CommandText = sqlstr;
            comm.CommandType = CommandType.Text;
            dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch
        {
            try
            {
                dr.Close();
                closeConnection();
            }
            catch { }
        }
        return dr;
    }//返回指定sql语句的OleDbDataReader对象，使用时请注意关闭这个对象。


    public static void dataReader(string sqlstr, ref OleDbDataReader dr)
    {
        try
        {
            openConnection();
            comm.CommandText = sqlstr;
            comm.CommandType = CommandType.Text;
            dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch
        {
            try
            {
                if (dr != null && !dr.IsClosed)
                    dr.Close();
            }
            catch
            {
            }
            finally
            {
                closeConnection();
            }
        }
    }//返回指定sql语句的OleDbDataReader对象,使用时请注意关闭

    public static DataSet dataSet(string sqlstr)
    {
        DataSet ds = new DataSet();
        OleDbDataAdapter da = new OleDbDataAdapter();
        try
        {
            openConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlstr;
            da.SelectCommand = comm;
            da.Fill(ds);

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            closeConnection();
        }
        return ds;
    }//返回指定sql语句的dataset

    public static void dataSet(string sqlstr, ref DataSet ds)
    {
        OleDbDataAdapter da = new OleDbDataAdapter();
        try
        {
            openConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlstr;
            da.SelectCommand = comm;
            da.Fill(ds);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            closeConnection();
        }
    }//返回指定sql语句的dataset

    public static DataTable dataTable(string sqlstr)
    {
        DataTable dt = new DataTable();
        OleDbDataAdapter da = new OleDbDataAdapter();
        try
        {
            openConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlstr;
            da.SelectCommand = comm;
            da.Fill(dt);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            closeConnection();
        }
        return dt;
    }//返回指定sql语句的datatable

    public static void dataTable(string sqlstr, ref DataTable dt)
    {
        OleDbDataAdapter da = new OleDbDataAdapter();
        try
        {
            openConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlstr;
            da.SelectCommand = comm;
            da.Fill(dt);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            closeConnection();
        }
    }//返回指定sql语句的datatable

    public static DataView dataView(string sqlstr)
    {
        OleDbDataAdapter da = new OleDbDataAdapter();
        DataView dv = new DataView();
        DataSet ds = new DataSet();
        try
        {
            openConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlstr;
            da.SelectCommand = comm;
            da.Fill(ds);
            dv = ds.Tables[0].DefaultView;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            closeConnection();
        }
        return dv;
    }
    //返回指定sql语句的dataview

}