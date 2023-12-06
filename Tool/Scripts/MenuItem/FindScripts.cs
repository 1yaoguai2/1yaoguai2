using UnityEngine;
using UnityEditor;
using System.Linq;

public class FindScripts : EditorWindow
{
    [MenuItem("Tools/�ű�����/���ҵ�ǰ��������missing�ű�")]
    static void FindMissingScriptObject()
    {
        /*******************************************************************************************
         * 1���༭��ѡ��n������
         * 2����ȡ��n�����弰�����ǵ������壬���ݼ���ΪA
         * 3��A��ȫ�������ж������Ƿ���null�Ľű���MonoBehaviour��
         *
         * �ж�һ������(object)�Ƿ��пսű���obj.GetComponents<MonoBehaviour>().Any(mono => mono == null)
         *******************************************************************************************/
        var objs = Selection.gameObjects;
        Debug.Log($"ѡ�е���������Ϊ��{objs.Length}");

        var allObjs = objs.SelectMany(obj => obj.GetComponentsInChildren<Transform>().Select(x => x.gameObject))
            .ToList();
        Debug.Log($"ѡ�е����弰�������������Ϊ��{allObjs.Count()}");
        allObjs.ForEach(obj =>
        {
            //1���������Ƿ���null�Ľű�
            var hasNullScript =
                obj.GetComponents<MonoBehaviour>().Any(mono => mono == null); //ע��:�á�MonoBehaviour���������á�MonoScript��
            //Debug.Log($"�Ƿ��пսű���{hasNullScript}���������֣���{obj.name}��");

            //2��Debug��������
            if (hasNullScript)
            {
                Debug.Log($"���� ��{obj.name}�� ����Missing�Ľű�");
            }
        });
    }
    
    
    [MenuItem("Tools/�ű�����/�Ƴ������ж�ʧ�Ľű�")]
    public static  void RemoveMissingScript()
    {
        foreach(GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
        }
    }



}