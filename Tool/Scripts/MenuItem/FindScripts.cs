using UnityEngine;
using UnityEditor;
using System.Linq;

public class FindScripts : EditorWindow
{
    [MenuItem("Tools/脚本处理/查找当前物体所有missing脚本")]
    static void FindMissingScriptObject()
    {
        /*******************************************************************************************
         * 1、编辑器选中n个物体
         * 2、获取这n个物体及其它们的子物体，数据集记为A
         * 3、A中全部物体判断他们是否有null的脚本（MonoBehaviour）
         *
         * 判断一个物体(object)是否有空脚本：obj.GetComponents<MonoBehaviour>().Any(mono => mono == null)
         *******************************************************************************************/
        var objs = Selection.gameObjects;
        Debug.Log($"选中的物体数量为：{objs.Length}");

        var allObjs = objs.SelectMany(obj => obj.GetComponentsInChildren<Transform>().Select(x => x.gameObject))
            .ToList();
        Debug.Log($"选中的物体及其子物体的数量为：{allObjs.Count()}");
        allObjs.ForEach(obj =>
        {
            //1、该物体是否有null的脚本
            var hasNullScript =
                obj.GetComponents<MonoBehaviour>().Any(mono => mono == null); //注意:用【MonoBehaviour】而不是用【MonoScript】
            //Debug.Log($"是否有空脚本：{hasNullScript}，物体名字：【{obj.name}】");

            //2、Debug物体名字
            if (hasNullScript)
            {
                Debug.Log($"物体 【{obj.name}】 上有Missing的脚本");
            }
        });
    }
    
    
    [MenuItem("Tools/脚本处理/移除场景中丢失的脚本")]
    public static  void RemoveMissingScript()
    {
        foreach(GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
        }
    }



}