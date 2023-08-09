using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    //���Ƶ������
    private GameObject mainCamera;
    //�Ƿ�����ƶ�
    public bool isMove = true;
    public float moveSpeed = 10f;
    private Vector3 pos;
    //�Ƿ������ת
    public bool isRotate = true;
    public float rotateSpeed = 10f;
    private float mouseX;
    private float mouseY;

    //�Ƿ�����м�����
    public bool isMouseMiddle = true;
    public float MiddleSpeed = 10f;


    //�Ƿ���ֿ���Զ��
    public bool isScrollWheel;
    public float scrollSpeed = 10f;
    private float scrollWheel;

    private void Start()
    {
    }

    void Update()
    {
        if (Input.anyKey)
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
        }
        if (isScrollWheel)
        {
            ScrollWheelControl();
        }

    }

    /// <summary>
    /// ���̰������ư�ť����
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
    /// ����Ҽ������ӽ���ת
    /// </summary>
    public void Rotation()
    {
        if (Input.GetMouseButton(1))
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = -Input.GetAxis("Mouse Y");
            transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime * mouseY);
            transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime * mouseX);
        }
    }

    /// <summary>
    /// ����м��϶��ӽ�
    /// </summary>
    public void MouseMiddle()
    {
        if (Input.GetMouseButton(2))
        {
            pos = transform.position;
            Vector3 offset = Vector3.zero;
            offset -= Vector3.ProjectOnPlane(transform.right, transform.up) * Input.GetAxis("Mouse X") * Time.deltaTime * MiddleSpeed;
            offset -= Vector3.ProjectOnPlane(transform.forward, transform.up) * Input.GetAxis("Mouse Y") * Time.deltaTime * MiddleSpeed;
            transform.position += offset;
        }
    }

    /// <summary>
    /// �����ֿ����ӽ�Զ��
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
