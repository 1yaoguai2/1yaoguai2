using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    //光标状态
    public bool cursorStatus;
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
    void Start()
    {
        //初始化鼠标
        Cursor.visible = false;
        Cursor.lockState = 0;
        //初始化角度
        targetDirection = transform.localRotation.eulerAngles;
        oldMousePosition = Input.mousePosition;
    }

    void Update()
    {
        MoveCR();
        RotateCR();
        CursorCR();
    }

    /// <summary>
    /// 移动控制
    /// </summary>
    private void MoveCR()
    {
        y = Input.GetAxis("Vertical") * moveSpeed * (Input.GetKey(runKey) ? runMultiplier : 1);// * Time.deltaTime;
        x = Input.GetAxis("Horizontal") * moveSpeed;// * Time.deltaTime;

        if (Physics.Raycast(transform.position + Vector3.up * 10000, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            Debug.Log(hit.point.y);
            //hoverHeight = transform.position.y + transform.position.y > hit.point.y + 1.8f ? -0.1f : 0.1f;
            hoverHeight = hit.point.y + 3.6f;
        }
        //transform.Translate(new Vector3(x, transform.position.y, y));
        Vector3.MoveTowards(transform.position, new Vector3(x, hoverHeight - transform.position.y, y), moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 旋转控制
    /// </summary>
    private void RotateCR()
    {
        var mouseChange = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxis("Mouse Y")) * rotateSpeed * 100f * Time.deltaTime;
        //var mouseChange = (Input.mousePosition - oldMousePosition) * rotateSpeed * Time.deltaTime;//与鼠标锁定冲突,无法旋转
        transform.Rotate(new Vector3(-mouseChange.y, 0, 0), Space.Self);
        transform.Rotate(new Vector3(0, mouseChange.x, 0), Space.World);
        oldMousePosition = Input.mousePosition;
    }

    /// <summary>
    /// 鼠标光标控制
    /// </summary>
    private void CursorCR()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorStatus = !cursorStatus;
        }
        Cursor.lockState = cursorStatus ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !cursorStatus;
    }
}
