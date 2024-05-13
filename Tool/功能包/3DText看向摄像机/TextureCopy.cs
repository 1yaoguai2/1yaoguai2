#if Development
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 创建者:   x
/// 创建时间: 2024-05-13 16:15:06
/// 备注:     由PIToolKit工具生成
/// </summary>
public class TextureCopy : MonoBehaviour
{
    public Material target;
    private void Awake()
    {
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        target.SetTexture("_MainTex", renderers[0].sharedMaterial.mainTexture);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sharedMaterial = target;
        }
        DestroyImmediate(this);
    }
}

#endif
