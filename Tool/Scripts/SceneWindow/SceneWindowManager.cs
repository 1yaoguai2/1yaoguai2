using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tool.Common;
using UnityEngine;

/// <summary>
/// unity场景windows窗口管理
/// </summary>
public class SceneWindowManager : Singleton<SceneWindowManager>
{
    

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    //最小化
    const int SW_SHOWMINIMIZED = 2;
    //最大化
    const int SW_SHOWMAXIMIZED = 3;
    //还原
    const int SW_SHOWRESTORE = 1;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetSystemMetrics(int nIndex);
    private static int SM_CXSCREEN = 0; //主屏幕分辨率宽度
    private static int SM_CYSCREEN = 1; //主屏幕分辨率高度
    private static int SM_CYCAPTION = 4; //标题栏高度
    private static int SM_CXFULLSCREEN = 16; //最大化窗口宽度（减去任务栏）
    private static int SM_CYFULLSCREEN = 17; //最大化窗口高度（减去任务栏）

    public int screenWidth => GetSystemMetrics(SM_CXSCREEN);
    public int screenHight => GetSystemMetrics(SM_CYSCREEN);

    public int resolutionRatioWidth => Screen.width;
    public int resolutionRatioHeight => Screen.height;


    #region 修改分辨率
    //替换/转换，是否全屏，默认全屏
    public static bool switchover = true;

    /// <summary>
    /// 转换窗口的分辨率，用来缩放窗口，注意传入的分辨率
    /// </summary>
    public void SceneWindowSizeControl(int wight = 1920, int hight = 1080, bool switchB = true)
    {
        Screen.SetResolution(wight, hight, switchB);
        switchover = switchB;
    }
    #endregion
    /// <summary>
    /// 最小化
    /// 分辨率不变
    /// </summary>
    public void WinMinimize()
    {
        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
    }
    /// <summary>
    /// 最大化
    /// 分辨率不变
    /// </summary>
    public void WinMax()
    {
        ShowWindow(GetForegroundWindow(), SW_SHOWMAXIMIZED);
    }
    /// <summary>
    /// 还原
    /// </summary>
    public void WinRestore()
    {
        ShowWindow(GetForegroundWindow(), SW_SHOWRESTORE);
    }

    /// <summary>
    /// 窗口变化
    /// </summary>
    /// <param name="i">分辨倍率</param>
    /// <param name="show">是否全屏</param>
    public void WindowSet(int i,bool show = true)
    {
        SceneWindowSizeControl(GetSystemMetrics(SM_CXSCREEN)/i, GetSystemMetrics(SM_CYSCREEN)/i,show);
    }

    #region 实际使用，设置playerSettings - Fullscreen Mode - windowed
    //是否退出
    bool isExit = false;
    private void OnGUI()
    {
        if (isExit)
        {
            int w = SceneWindowManager.Instance.resolutionRatioWidth;
            int h = SceneWindowManager.Instance.resolutionRatioHeight;

            GUI.Box(new Rect(w / 2 - 200, h / 2 - 100, 400, 200), "退出软件");

            if (GUI.Button(new Rect(w / 2 - 150, h / 2, 100, 50), "退出"))
            {
                Exit();
            }
            if (GUI.Button(new Rect(w / 2 + 50, h / 2, 100, 50), "取消"))
            {
                isExit = false;
            }
        }
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
    #endregion

}




