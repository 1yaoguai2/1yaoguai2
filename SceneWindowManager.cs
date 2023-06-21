using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tool.Common;
using UnityEngine;

/// <summary>
/// unity场景windows窗口管理
/// </summary>
public class SceneWindowManager : Singleton<SceneWindowManager>
{
    #region 修改分辨率
    //替换/转换，是否全屏，默认全屏
    private static bool switchover = true;

    /// <summary>
    /// 转换窗口的分辨率
    /// </summary>
    public void SceneWindowSizeControl(int hight = 1920, int wight = 1080)
    {
        Screen.SetResolution(hight, wight, switchover);
        Screen.fullScreen = switchover;
        switchover = !switchover;
    }
    #endregion

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

    /// <summary>
    /// 最小化
    /// </summary>
    public void WinMinimize()
    {
        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
    }
    /// <summary>
    /// 最大化
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

}




