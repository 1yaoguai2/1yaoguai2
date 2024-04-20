using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Order : MonoBehaviour
{
    [MenuItem("Tools/����������")]
    public static void OrderChild()
    {
        try
        {
            var selectObjs = Selection.gameObjects;
            if (selectObjs.Length == 0)
            {
                throw new Exception("����ѡ��һ�����壡");
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
                    //�����뷨
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
                var newSelectObjs =  selectObjs.OrderBy(t => int.Parse(t.name)).ToList();
                for (int k = 0; k < newSelectObjs.Length; k++)
                {
                    newSelectObjs[k].SetSiblingIndex(k);
                }
            }
        }
        
                