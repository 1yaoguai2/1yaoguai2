using UnityEngine;
using UnityEngine.Serialization;

public class FirstPersonController : MonoBehaviour
{
    //移动相关
    [Header("移动参数")] public float moveSpeed = 10f;
    private float x;
    private float z;
    public KeyCode runKey = KeyCode.LeftShift;
    public int runMultiplier = 2;

    public LayerMask groundLayer;

    //射线检测
    RaycastHit hit;

    float hoverHeight = 0f;

    //旋转相关
    [Header("旋转参数")] public float rotateSpeed = 10f;
    public Vector3 vect;
    private float xcream;
    private float ycream;

    //反转Y轴旋转
    public bool invertY;

    [Header("范围限制")] [SerializeField] private bool useLimit;
    [SerializeField] private Vector2 xLimit;
    [SerializeField] private Vector2 zLimit;

    void OnEnable()
    {
        //初始化鼠标
        //LockCursor = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //CursorCR();
        if (LockCursor) return;
        MoveCR();
        RotateCR();
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
        z = Input.GetAxis("Vertical") * moveSpeed * (Input.GetKey(runKey) ? runMultiplier : 1) * Time.deltaTime;
        x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        if (Physics.Raycast(transform.position + Vector3.up * 1000, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            //Debug.Log(hit.point.y);
            hoverHeight = hit.point.y + 1.6f;
        }

        if (useLimit)
        {
            var pos = transform.position;
            if (pos.x < xLimit.x)
                pos.x = xLimit.x;
            else if (pos.x > xLimit.y)
                pos.x = xLimit.y;
            if (pos.z < zLimit.x)
                pos.z = zLimit.x;
            else if (pos.z > zLimit.y)
                pos.z = zLimit.y;
            transform.position = pos;
        }

        transform.Translate(new Vector3(x, hoverHeight - transform.position.y, z));
    }

    /// <summary>
    /// 旋转控制
    /// </summary>
    private void RotateCR()
    {
        var mouseChange = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) *
                          (rotateSpeed * 100f * Time.deltaTime);
        if (invertY) mouseChange.y = -mouseChange.y;
        transform.Rotate(new Vector3(-mouseChange.y, 0, 0), Space.Self);
        transform.Rotate(new Vector3(0, mouseChange.x, 0), Space.World);
    }

    /// <summary>
    /// 鼠标光标控制
    /// </summary>
    private void CursorCR()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //TODO:按下鼠标左键会调回来执行改语句
            LockCursor = !LockCursor;
        }
    }

    /// <summary>
    /// 鼠标控制器
    /// </summary>
    public bool LockCursor
    {
        get { return Cursor.lockState == CursorLockMode.None; }
        set
        {
            //Cursor.visible = value;
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
    private void LimitAngleHorizontal(float angle)
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