using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PI
{
    /// <summary>
    /// 创建者:   Harling
    /// 创建时间: 2021-07-19 14:24:37
    /// 备注:     由PIToolKit工具生成
    /// </summary>
    public class HoloManager : MonoBehaviour
    {
        [ColorUsage(true, true)]
        public Color back;
        [ColorUsage(true, true)]
        public Color color;
        [Range(0, 1)]
        public float ratio = 0.1f;
        [Range(0f, 0.2f)]
        public float lineWidth = 0.002f;
        [Range(0, 1000)]
        public float maxRange = 150f;
        private static Shader holo;
        private static Dictionary<Camera, CameraInfo> cache = new Dictionary<Camera, CameraInfo>();
        
        private void Awake()
        {
            if (holo == null) holo = Shader.Find("Hidden/HoloGraphic");
        }
        private void OnEnable()
        {
            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            CameraInfo info = null;
            foreach (var camera in cameras)
            {
                if (!cache.TryGetValue(camera,out info))
                {
                    info = new CameraInfo() { Flags = camera.clearFlags, Color = camera.backgroundColor };
                    cache.Add(camera, info);
                }
                else
                {
                    info.Flags = camera.clearFlags;
                    info.Color = camera.backgroundColor;
                }
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = back;
                camera.SetReplacementShader(holo, "RenderType");
            }
        }
        private void Update()
        {
            Shader.SetGlobalFloat("Ratio", ratio);
            Shader.SetGlobalFloat("LineWidth", lineWidth);
            Shader.SetGlobalFloat("MaxRange", maxRange);
            Shader.SetGlobalColor("HoloCol", color);
        }
        private void OnDisable()
        {
            foreach (var item in cache)
            {
                item.Key.clearFlags = item.Value.Flags;
                item.Key.backgroundColor = item.Value.Color;
                item.Key.ResetReplacementShader();
            }
            cache.Clear();
        }
        private class CameraInfo
        {
            public CameraClearFlags Flags;
            public Color Color;
        }
    }
}
