using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    //控制的摄像机
    private GameObject mainCamera;
    //是否控制移动
    public bool isMove = true;
    public float moveSpeed = 10f;
    private Vector3 pos;
    //是否控制旋转
    public bool isRotate = true;
    public float rotateSpeed = 10f;
    private float mouseX;
    private float mouseY;
    Vector3 newMousePos;
    Vector3 oldMousePos;
    public Vector3 OldMousePos
    {
        get => oldMousePos;
        set => oldMousePos = value;
    }

    //是否鼠标中键控制
    public bool isMouseMiddle = true;
    public float MiddleSpeed = 100f;


    //是否滚轮控制远近
    public bool isScrollWheel = true;
    public float scrollSpeed = 100f;
    private float scrollWheel;



    void Update()
    {
        if (isMove)
        {
            Moving();
        }
        if (isRotate)
        {
            Rotation();
        }
        if (isMouseMiddle)
        {
            MouseMiddle();
        }
        if (isScrollWheel)
        {
            ScrollWheelControl();
        }
        oldMousePos = Input.mousePosition;
    }

    /// <summary>
    /// 键盘按键控制按钮按下
    /// </summary>
    public void Moving()
    {
        pos = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            pos += transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            pos -= transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos += transform.right * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            pos -= transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.C))
        {
            pos -= transform.up * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            pos += transform.up * moveSpeed * Time.deltaTime;
        }
        transform.position = pos;
    }

    /// <summary>
    /// 鼠标右键控制视角旋转
    /// </summary>
    public void Rotation()
    {
        if (Input.GetMouseButton(1))
        {
            newMousePos = Input.mousePosition;
            Vector3 dis = newMousePos - oldMousePos;
            float angleX = dis.x * rotateSpeed / 100f;//* Time.deltaTime;
            float angleY = dis.y * rotateSpeed / 100f;// * Time.deltaTime;
            transform.Rotate(new Vector3(-angleY, 0, 0), Space.Self);
            transform.Rotate(new Vector3(0, angleX, 0), Space.World);
            //mouseX = Input.GetAxis("Mouse X");
            //mouseY = -Input.GetAxis("Mouse Y");
            //transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime * mouseY);
            //transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime * mouseX);
        }
    }

    /// <summary>
    /// 鼠标中键拖动视角
    /// </summary>
    public void MouseMiddle()
    {
        if (Input.GetMouseButton(2))
        {
            pos = transform.position;
            Vector3 offset = Vector3.zero;
            offset -= transform.right * Input.GetAxis("Mouse X") * Time.deltaTime * MiddleSpeed;
            offset -= Vector3.up * Input.GetAxis("Mouse Y") * Time.deltaTime * MiddleSpeed;
            transform.position += offset;
        }
    }

    /// <summary>
    /// 鼠标滚轮控制视角远近
    /// </summary>
    public void ScrollWheelControl()
    {
        scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0)
        {
            pos = transform.position;
            pos += (scrollWheel > 0 ? 1 : -1) * scrollSpeed * Time.deltaTime * transform.forward;
            transform.position = pos;
        }
    }

}
