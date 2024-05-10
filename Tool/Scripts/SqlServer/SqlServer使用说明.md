## 使用

需要在Assets下新建文件夹Plugins，将unity安装位置.

\2022.2.20f1c1\Editor\Data\MonoBleedingEdge\lib\mono\unity下的

I18N.CJK.dll

I18N.dll

I18N.Other.dll

I18N.Rera.dll

I18N.West.dll

System.Data.dll

复制到Plugins内

## 报错解决
<font color="red">SqlConnection未找到相关引用</font>
找到unity的项目设置 player/OtherSettings/Api Compatibility ,修改.net版本



## 读取数据和跟新数据

```c#
 /// <summary>
    /// 查询所有品规的烟的库存数量
    /// </summary>
    /// <returns></returns>
    public static DataTable GetAllSmokeBrand()
    {
        try
        {
            DataTable dt = new DataTable();
            string sql = "SELECT T1.ITEMID AS 规格, T1.QTY AS 库存总数 FROM ( SELECT L1.ITEMID, COUNT(*) AS QTY FROM KG_NOW L1 LEFT JOIN KG_LOC AS L2 ON L1.LOCNUM = L2.LOCNUM WHERE (L1.STATUS = '移入完成') AND (L2.LOCSTATUS = '正常') AND (L2.LOCSTORESTATUS = '载货') GROUP BY L1.ITEMID ) T1";
            SqlDataAdapter sda =
                new SqlDataAdapter(sql, SqlServerManager.conn);
            sda.Fill(dt);
            return dt;
        }
        catch(Exception ex)
        {
            Debug.LogError("读取仓库库存数据出错:" + ex.Message);
            return new DataTable();
        }
    }
}


     /// <summary>
    /// 跟新任务号，状态
    /// </summary>
    /// <param name="loggnum"></param>
    /// <param name="status"></param>
    public static bool UpdateStatus(string loggnum, string status,string agvNo = "0")
    {
        try{
            
        
        string sql = $"UPDATE KG_JOB SET STATUS = '{status.Trim()} '";
        if(agvNo != "0")
        {
            sql += $" ,REMARK = '{agvNo}'";
        }
        if ("00" == status || "04" == status)
        {
            sql += ",ENDTIME =CONVERT(varchar(100), GETDATE(), 120)";
        }
        sql += $" WHERE LOGGNUM = '{loggnum}'";
        DataTable dt = new DataTable();
        SqlDataAdapter sda =
            new SqlDataAdapter(sql, SqlServerManager.conn);
        sda.Fill(dt);
        return true;
        }
        catch(Exception ex)
        {
            Debug.LogError("跟新数据库出错:" + ex.Message);
            return false;
        }
    }

```



