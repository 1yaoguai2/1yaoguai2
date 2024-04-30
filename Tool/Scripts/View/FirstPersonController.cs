using Static.Model;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    //移动相关
    [Header("移动参数")]
    public float moveSpeed = 10f;
    private float x;
    private float y;
    public KeyCode runKey = KeyCode.LeftShift;
    public int runMultiplier = 2;
    public LayerMask groundLayer;
    //射线检测
    RaycastHit hit;
    float hoverHeight = 0f;
    //旋转相关
    [Header("旋转参数")]
    Vector2 targetDirection;
    Vector3 oldMousePosition;
    public float rotateSpeed = 10f;
    public Vector3 vect;
    private float xcream;
    private float ycream;

    //反转Y轴旋转
    public bool invertY;


    void OnEnable()
    {
        //初始化鼠标
        LockCursor = false;
        //初始化角度
        targetDirection = transform.localRotation.eulerAngles;
        oldMousePosition = Input.mousePosition;
        StaticModel.currentCamera = transform;
    }

    void Update()
    {
        MoveCR();
        RotateCR();
        CursorCR();
    }
    private void LateUpdate()
    {
        LimitAngle(80f);
        //LimitAngleUandD(170f);
    }

    /// <summary>
    /// 移动控制
    /// </summary>
    private void MoveCR()
    {
        y = Input.GetAxis("Vertical") * moveSpeed * (Input.GetKey(runKey) ? runMultiplier : 1) * Time.deltaTime;
        x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        if (Physics.Raycast(transform.position + Vector3.up * 10000, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            //Debug.Log(hit.point.y);
            hoverHeight = hit.point.y + 3.6f;
        }
        transform.Translate(new Vector3(x, hoverHeight - transform.position.y, y));
    }

    /// <summary>
    /// 旋转控制
    /// </summary>
    private void RotateCR()
    {
        //if (!Input.GetMouseButton(1)) return;
        var mouseChange = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * rotateSpeed * 100f * Time.deltaTime;
        //var mouseChange = (Input.mousePosition - oldMousePosition) * rotateSpeed * Time.deltaTime;//与鼠标锁定冲突,无法旋转
        if (invertY) mouseChange.y = - mouseChange.y;
        transform.Rotate(new Vector3(-mouseChange.y, 0, 0), Space.Self);
        transform.Rotate(new Vector3(0, mouseChange.x, 0), Space.World);
        oldMousePosition = Input.mousePosition;
    }

    /// <summary>
    /// 鼠标光标控制
    /// </summary>
    private void CursorCR()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            LockCursor = !LockCursor;   //TODO:按下鼠标左键会调回来执行改语句
        }
    }

    /// <summary>
    /// 鼠标控制器
    /// </summary>
    public bool LockCursor
    {
        get { return Cursor.lockState == CursorLockMode.Locked ? false : true; }
        set
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// 限制相机上下视角的角度
    /// </summary>
    /// <param name="angle">角度</param>
    private void LimitAngle(float angle)
    {
        vect = this.transform.eulerAngles;
        //当前相机x轴旋转的角度(0~360)
        xcream = IsPosNum(vect.x);
        if (xcream > angle)
            this.transform.rotation = Quaternion.Euler(angle, vect.y, 0);
        else if (xcream < -angle)
            this.transform.rotation = Quaternion.Euler(-angle, vect.y, 0);
    }

    /// <summary>
    /// 限制相机左右视角的角度
    /// </summary>
    /// <param name="angle"></param>
    private void LimitAngleUandD(float angle)
    {
        vect = this.transform.eulerAngles;
        //当前相机y轴旋转的角度(0~360)
        ycream = IsPosNum(vect.y);
        if (ycream > angle)
            this.transform.rotation = Quaternion.Euler(vect.x, angle, 0);
        else if (ycream < -angle)
            this.transform.rotation = Quaternion.Euler(vect.x, -angle, 0);
    }

    /// <summary>
    /// 将角度转换为-180~180的角度
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private float IsPosNum(float x)
    {
        x -= 180;
        if (x < 0)
            return x + 180;
        else return x - 180;
    }
}
