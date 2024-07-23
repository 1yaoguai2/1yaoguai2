using Core;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Assets.Scripts.Models;

public class StackerStoreGoodsCreate : MonoBehaviour
{
    static int lane = 4; //巷道
    static int pai = 2;  //排
    static int ceng = 2; //层
    static int lie = 86;  //列
    static Vector3 startPos = new Vector3(290.976f, 0.9f, -170.592f); //一巷道第一个货位
    static float disX = -1.501f;
    static float disY = 3.88f - 0.9f;
    static float disZ = 2.92f;
    static float laneDis = 4.638f;
    static float doubleDis = 1.42f;
    static float hightDis = 0.08f;  //双深与单深高度差

    static List<LaneData> laneDatas = new List<LaneData>();

    [MenuItem("Tools/仓库/创建仓库货位")]
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

        //配置地方，配置每个巷道的情况
        LaneData laneData1 = new LaneData
        {
            LaneNum = 1,
            Lie = lie,
            Ceng = ceng,
            Pai = 3,
            PosX = startPos.x,
            PosY = startPos.y,
            PosZ = startPos.z
        };
        LaneData laneData2 = new LaneData
        {
            LaneNum = 2,
            Lie = lie,
            Ceng = ceng,
            Pai = pai,
            PosX = startPos.x,
            PosY = startPos.y,
            PosZ = startPos.z + (2 - 1) * laneDis
        };
        LaneData laneData3 = new LaneData
        {
            LaneNum = 3,
            Lie = lie,
            Ceng = ceng,
            Pai = pai,
            PosX = startPos.x,
            PosY = startPos.y,
            //PosZ = startPos.z + (3 - 1) * laneDis
            PosZ = -160.642f
        };
        LaneData laneData4 = new LaneData
        {
            LaneNum = 4,
            Lie = lie,
            Ceng = ceng,
            Pai = pai,
            PosX = startPos.x,
            PosY = startPos.y,
            //PosZ = startPos.z + (4 - 1) * laneDis
            PosZ = -156.004f
        };


        laneDatas.Add(laneData1);
        laneDatas.Add(laneData2);
        laneDatas.Add(laneData3);
        laneDatas.Add(laneData4);


        foreach (var laneData in laneDatas)
        {
            for (int i = 1; i <= laneData.Pai; i++)
            {
                for (int j = 1; j <= laneData.Ceng; j++)
                {
                    for (int k = 1; k <= laneData.Lie; k++)
                    {
                        var g = Instantiate(goods, store.transform);

                        if (i < 3)
                        {
                            g.name = $"{laneData.LaneNum}{i.ToString().PadLeft(2,'0')}{k.ToString().PadLeft(2, '0')}{j.ToString().PadLeft(2, '0')}{1}";
                            g.transform.position = new Vector3(laneData.PosX, laneData.PosY, laneData.PosZ) + new Vector3((k - 1) * disX, (j - 1) * disY, (i - 1) * disZ);
                        }
                        else if (i == 3)
                        {
                            g.name = $"{laneData.LaneNum}{i.ToString().PadLeft(2,'0')}{k.ToString().PadLeft(2, '0')}{j.ToString().PadLeft(2, '0')}{0}";
                            g.transform.position = new Vector3(laneData.PosX, laneData.PosY, laneData.PosZ) + new Vector3((k - 1) * disX, (j - 1) * disY + hightDis, -doubleDis);
                        }
                        else if (i == 4)
                        {
                            g.name = $"{laneData.LaneNum}{i.ToString().PadLeft(2, '0')}{k.ToString().PadLeft(2, '0')}{j.ToString().PadLeft(2, '0')}{0}";
                            g.transform.position = new Vector3(laneData.PosX, laneData.PosY, laneData.PosZ) + new Vector3((k - 1) * disX, (j - 1) * disY + hightDis, 1 * disZ + doubleDis);
                        }
                        //g.transform.eulerAngles = new Vector3(0, 90, 0);
                    }
                }
            }
        }

    }

    [MenuItem("Tools/仓库/保存仓库货物位置")]
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
            if (goods.gameObject.activeInHierarchy)
                stores.Add(goods.name, goods);
        }
        SaveLoc(stores);
    }

    /// <summary>
    /// 读取并跟新Json文件
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
            Debug.Log("存储货位位置完成！");
        }
        catch (Exception ex)
        {
            LogManager.LogError("存储货位位置出错" + ex.Message);
        }
    }

    /// <summary>
    /// 反序列化货物位置信息
    /// </summary>
    /// <param name="locs"></param>
    /// <returns></returns>
    public static List<Loc> ReadLoc(List<Loc> locs)
    {
        try
        {
            string jsonStr = File.ReadAllText($"{Application.streamingAssetsPath}/StoreCargoPos.json");
            //反序列化
            locs = new List<Loc>(JsonConvert.DeserializeObject<List<Loc>>(jsonStr));
            LogManager.Log("加载仓库货物位置信息完成！");
            return locs;
        }
        catch (Exception ex)
        {
            LogManager.LogError("加载仓库货物位置信息出错：" + ex.Message);
            return new List<Loc>();
        }

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

public class LaneData
{
    /// <summary>
    /// 巷道编号
    /// </summary>
    public int LaneNum { get; set; }
    public int Lie { get; set; }
    public int Ceng { get; set; }
    public int Pai { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
}

