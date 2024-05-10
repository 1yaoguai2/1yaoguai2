using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Xml;
using UnityEngine;

public class SqlServerManager : MonoBehaviour
{
    //下面的账号和密码是你下载数据库时设置的账号和密码，而server后面的IP地址是最开始设置的IP地址
    string connsql = @"server=192.168.3.41; database=cqysk; uid=sa; pwd=DBM001"; // 使用sql验证的方式连接数据库
    public static SqlConnection conn; //创建一个数据库连接

    public static DataTable loginUserDT; //登录的用户数据表

    public void SQLServerToConnection()
    {
        try
        {
            conn = new SqlConnection(connsql);
            //判断数据库是否处于关闭状态
            /*if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    Debug.Log("连接成功");
                }
            }*/
        }
        catch (Exception e)
        {
            XLogger.LogError("连接数据出错");
        }
    }

    public void SQLServerToClosed()
    {
        try
        {
            //判断数据库是否处于关闭状态
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
                if (conn.State == ConnectionState.Closed)
                {
                    XLogger.LogInformation("断开成功");
                }
            }
        }
        catch (Exception e)
        {
            XLogger.LogError("关闭数据出错" + e);
        }
    }

    private void Awake()
    {
        LoadParam();
        SQLServerToConnection();
    }

    private void OnApplicationQuit()
    {
        SQLServerToClosed();
    }

    /// <summary>
    /// 读取XML文件获取Date地址
    /// </summary>
    private void LoadParam()
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(Application.streamingAssetsPath + "/Plc/CONN.xml");
        XmlNodeList xmlNodeList = xml.SelectNodes("Conns/Conn");
        foreach (XmlNode Url in xmlNodeList)
        {
            connsql = @$"server={Url.Attributes["server"].Value}; database={Url.Attributes["database"].Value}; uid={Url.Attributes["uid"].Value}; pwd={Url.Attributes["pwd"].Value}";
        }
    }
}