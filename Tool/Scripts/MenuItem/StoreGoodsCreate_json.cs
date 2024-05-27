using Core;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Assets.Scripts.Models;

public class StoreGoodsCreate : MonoBehaviour
{
    static int lane = 1; //巷道
    static int pai = 20;  //排
    static int ceng = 2; //层
    static int lie = 28;  //列
    static Vector3 startPos = new Vector3(10.62f, 0.7f, -6.3f);
    static float disX = 1.4f;
    static float disY = 3.02f;
    static float disZ = 1.57f;
    static float disOne = 3.37f - 1.4f;
    static float disTwo = 3.167f - 1.4f;

    [MenuItem("Tools/创建仓库货位")]
    private static void CreateStoreGoods()
    {
        var goods = Resources.Load<GameObject>(MessageCenterModel.StorePrefab);
        if (!goods)
        {
            Debug.LogError($"查找仓库货物预制体{MessageCenterModel.StorePrefab}失败!");
            return;
        }
        var store = GameObject.Find("Store");
        if (!store)
        {
            store = new GameObject("Store");
            store.transform.position = Vector3.zero;
        }

        for (int i = 1; i <= pai; i++)
        {
            for (int j = 1; j <= ceng; j++)
            {
                for (int k = 1; k <= lie; k++)
                {
                    var g = Instantiate(goods, store.transform);
                    g.name = $"L{i.ToString().PadLeft(2, '0')}{j.ToString().PadLeft(2, '0')}{k.ToString().PadLeft(2, '0')}";
                    if (i >= 3)
                    {
                        if (i >= 18)
                            g.transform.position = startPos + new Vector3((i - 1) * disX, (j - 1) * disY, (k - 1) * disZ) + new Vector3(disOne + disTwo, 0, 0);
                        else
                            g.transform.position = startPos + new Vector3((i - 1) * disX, (j - 1) * disY, (k - 1) * disZ) + new Vector3(disOne, 0, 0);
                    }
                    else
                        g.transform.position = startPos + new Vector3((i - 1) * disX, (j - 1) * disY, (k - 1) * disZ);

                    //g.transform.eulerAngles = new Vector3(0, 90, 0);
                }
            }
        }
    }

    [MenuItem("Tools/保存仓库货物位置")]
    private static void SaveStoreGoodsPos()
    {
        var store = GameObject.Find("Store");
        if (!store)
        {
            Debug.LogError($"查找仓库货物父对象‘Store’失败!");
            return;
        }
        Dictionary<string, Transform> stores = new Dictionary<string, Transform>();
        foreach (var goods in FirstLeveChild.GetFisrtLevelChildren(store.transform))
        {
            stores.Add(goods.name, goods);
        }
        SaveLoc(stores);
    }

    /// <summary>
    /// 读取并跟新XML文件
    /// </summary>
    private static void SaveLoc(Dictionary<string, Transform> keyValues)
    {
        try
        {
            List<Loc> locs = new List<Loc>();
            foreach (var key in keyValues)
            {
                var newLoc = new Loc()
                {
                    LocNum = key.Key,
                    PosX = key.Value.position.x,
                    PosY = key.Value.position.y,
                    PosZ = key.Value.position.z
                };
                locs.Add(newLoc);
            }
            var str = JsonConvert.SerializeObject(locs);
            File.WriteAllText($"{Application.streamingAssetsPath}/StoreCargoPos.json", str);
        }
        catch (Exception ex)
        {
            LogManager.LogError("存储货位位置出错" + ex);
        }
    }

    /// <summary>
    /// 反序列化货物位置信息
    /// </summary>
    /// <param name="locs"></param>
    /// <returns></returns>
    private static List<Loc> ReadLoc(List<Loc> locs)
    {
        string jsonStr = File.ReadAllText($"{Application.streamingAssetsPath}/StoreCargoPos.json");

        //反序列化
        locs = new List<Loc>(JsonConvert.DeserializeObject<List<Loc>>(jsonStr));

        return locs;
    }
}



static class FirstLeveChild
{
    public static List<Transform> GetFisrtLevelChildren(Transform parent)
    {
        List<Transform> childrens = new List<Transform>();
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            childrens.Add(parent.GetChild(i));
        }
        return childrens;
    }
}

//public class Loc
//{
//    //public string LaneNum { get; set; }
//    //public string Lie { get; set; }
//    //public string Ceng { get; set; }
//    //public string Pai { get; set; }

//    public string LocNum { get; set; }
//    public float PosX { get; set; }
//    public float PosY { get; set; }
//    public float PosZ { get; set; }
//}