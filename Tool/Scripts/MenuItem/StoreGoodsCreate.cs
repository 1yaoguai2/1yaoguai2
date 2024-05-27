using com.clwl.scada.common.entity;
using Core;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class StoreGoodsCreate : MonoBehaviour
{
    static int lane = 4; //巷道
    static int pai = 2;  //排
    static int ceng = 14; //层
    static int lie = 51;  //列
    static Vector3 startPos = new Vector3(212.944f, 0.805f, 77.626f);
    static float disH = 1.275f;
    static float disV = 1.041f;

    [MenuItem("Tools/创建仓库货位")]
    private static void CreateStoreGoods()
    {
        var goods = Resources.Load<GameObject>(MessageCenterModel.PalletPath);
        if (!goods)
        {
            Debug.LogError($"查找仓库货物预制体{MessageCenterModel.PalletPath}失败!");
            return;
        }
        var store = GameObject.Find("Store");
        if (!store)
        {
            store = new GameObject("Store");
            store.transform.position = Vector3.zero;
        }

        for (int i = 1; i <= lie; i++)
        {
            for (int j = 1; j <= ceng; j++)
            {
                var g = Instantiate(goods, store.transform);
                g.name = $"{lane}{pai}{i.ToString().PadLeft(3, '0')}{j.ToString().PadLeft(3, '0')}1";
                g.transform.position = startPos + new Vector3((i - 1) * disV, (j - 1) * disH, startPos.z);
                g.transform.eulerAngles = new Vector3(0, 90, 0);
            }
        }
    }

    [MenuItem("Tools/保存仓库货物位置")]
    private static void SaveStoreGoodsPos()
    {
        var store = GameObject.Find("Store");
        if (!store)
        {
            Debug.LogError($"查找仓库货物‘Store’失败!");
            return;
        }
        Dictionary<string,Transform> stores = new Dictionary<string,Transform>();
        foreach (var goods in store.transform.GetFisrtLevelChildren())
        {
            stores.Add(goods.name,goods);
        }
        SaveLoc(stores);
    }

    /// <summary>
    /// 读取并跟新XML文件
    /// </summary>
    private static void SaveLoc(Dictionary<string,Transform> keyValues)
    {
        try
        {
            var doc = new XmlDocument();
            var xmlAsset = Resources.Load("Coordinate").ToString();
            doc.LoadXml(xmlAsset);
            var xmlNodeList = doc.SelectNodes("Locs/Loc");
            //if (_dicLocs == null) _dicLocs = new Dictionary<string, Dictionary<string, Loc>>();
            if (xmlNodeList == null) return;
            foreach (XmlNode xn in xmlNodeList)
            {
                var loc = new Loc();
                loc.LaneNum = xn.Attributes["LaneNum"].Value;
                loc.Lie = xn.Attributes["Lie"].Value;
                loc.Ceng = xn.Attributes["Ceng"].Value;
                loc.Pai = xn.Attributes["Pai"].Value;
                loc.LocNum = xn.Attributes["LocNum"].Value;
                //loc.XPos = float.Parse(xn.Attributes["XPos"].Value);
                //loc.YPos = float.Parse(xn.Attributes["YPos"].Value);
                //loc.ZPos = float.Parse(xn.Attributes["ZPos"].Value);
                //var area = xn.Attributes["Area"].Value;

                string goodsName = loc.LocNum;
                bool find = keyValues.TryGetValue(goodsName,out Transform goodsTrs);
                if (find)
                {
                    xn.Attributes["XPos"].Value = goodsTrs.position.x.ToString();
                    xn.Attributes["YPos"].Value = goodsTrs.position.y.ToString();
                    xn.Attributes["ZPos"].Value = goodsTrs.position.z.ToString();
                }
                else
                {
                    
                }
            }
            doc.Save(@$"{Application.streamingAssetsPath}/Coordinate.xml"); //保存
            LogManager.Log("保存货位位置成功！");
        }
        catch (Exception ex)
        {
            LogManager.LogError("加载货位位置出错" + ex);
        }
    }
}
