using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public enum Rate { Default, Sixty, HundredTwenty, Customize };

public class GetFPS : MonoBehaviour
{
    private string FPS_Text;
    private float m_UpdateShowDeltaTime;//更新帧率的时间间隔;  
    private int m_FrameUpdate = 0;//帧数;  
    private float m_FPS = 0;//帧率

    [SerializeField]
    private bool show;
    public Rate fpsRate = new();
    public int rateNum;


    private void Awake()
    {
        Application.targetFrameRate = rateNum;//锁定最大帧率为60帧
    }

    private void OnGUI()
    {
        if (show)
            GUI.Box(new Rect(100f, 100f, 100f, 20f), FPS_Text);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8)) show = !show;
        if (!show) return;
        m_FrameUpdate++;
        m_UpdateShowDeltaTime += Time.deltaTime;
        if (m_UpdateShowDeltaTime >= 0.2)
        {
            m_FPS = m_FrameUpdate / m_UpdateShowDeltaTime;
            m_UpdateShowDeltaTime = 0;
            m_FrameUpdate = 0;
            FPS_Text = m_FPS.ToString() + "帧";
        }
    }

    private void OnValidate()
    {
        if(fpsRate == Rate.Default)
        {
            rateNum = -1;
        }
        else if (fpsRate == Rate.Sixty)
        {
            rateNum = 60;
        }
        else if(fpsRate == Rate.HundredTwenty)
        {
            rateNum = 120;
        }
    }
}
