using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 层级窗口物体从小到大排序
/// 选中一个物体时，对子物体进行名称大小排序
/// 选中多个物体时，通过名称大小进行排序
/// 循环所有子物体列表，通过数组的OrderBy得到新的数组
/// 将数组的元素通过SetSiblingIndex设置在层级窗口中的index
/// </summary>
public class Order : MonoBehaviour
{
    [MenuItem("Tools/排序子物体")]
    public static void OrderChild()
    {
        try
        {
            var selectObjs = Selection.gameObjects;
            if (selectObjs.Length == 0)
            {
                throw new Exception("至少选中一个物体！");
            }
            else if (selectObjs.Length == 1)
            {
                var childs = new Transform[selectObjs[0].transform.childCount];
                for (int i = 0; i < selectObjs[0].transform.childCount; i++)
                {
                    childs[i] = selectObjs[0].transform.GetChild(i);
                }

                var newChilds = childs.OrderBy(t => int.Parse(t.name)).ToList();
                for (int j = 0; j < newChilds.Count; j++)
                {
                    Debug.Log(newChilds[j].name);
                    //错误想法 循环判断当前对象与下一个对象的大小，将大的往后移动，之后再判断与下下个的大小关系
                    //bool orderB = int.Parse(childs[j].name) > int.Parse(childs[j + 1].name);
                    //int currentIndex = j;
                    //while (orderB)
                    //{
                    //    childs[currentIndex].SetSiblingIndex(currentIndex + 2);
                    //    currentIndex++;
                    //    if (currentIndex < childs.Length)
                    //    {
                    //        orderB = int.Parse(childs[currentIndex].name) > int.Parse(childs[currentIndex + 1].name);
                    //    }
                    //}
                    newChilds[j].SetSiblingIndex(j);
                }
            }
            else
            {
                var newSelectObjs = selectObjs.OrderBy(t => int.Parse(t.name)).ToList();
                for (int k = 0; k < newSelectObjs.Count; k++)
                {
                    newSelectObjs[k].transform.SetSiblingIndex(k);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
        
                