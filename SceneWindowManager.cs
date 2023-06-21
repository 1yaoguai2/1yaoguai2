using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tool.Common;
using UnityEngine;

/// <summary>
/// unity����windows���ڹ���
/// </summary>
public class SceneWindowManager : Singleton<SceneWindowManager>
{
    #region �޸ķֱ���
    //�滻/ת�����Ƿ�ȫ����Ĭ��ȫ��
    private static bool switchover = true;

    /// <summary>
    /// ת�����ڵķֱ���
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
    //��С��
    const int SW_SHOWMINIMIZED = 2;
    //���
    const int SW_SHOWMAXIMIZED = 3;
    //��ԭ
    const int SW_SHOWRESTORE = 1;

    /// <summary>
    /// ��С��
    /// </summary>
    public void WinMinimize()
    {
        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
    }
    /// <summary>
    /// ���
    /// </summary>
    public void WinMax()
    {
        ShowWindow(GetForegroundWindow(), SW_SHOWMAXIMIZED);
    }
    /// <summary>
    /// ��ԭ
    /// </summary>
    public void WinRestore()
    {
        ShowWindow(GetForegroundWindow(), SW_SHOWRESTORE);
    }

}




