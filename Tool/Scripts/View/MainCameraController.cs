using com.clwl.trflk.common.entity;
using com.clwl.web;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
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
    Vector3 newMousePos;
    Vector3 oldMousePos;

    //�Ƿ�����м�����
    public bool isMouseMiddle = true;
    public float MiddleSpeed = 10f;


    //�Ƿ���ֿ���Զ��
    public bool isScrollWheel;
    public float scrollSpeed = 10f;
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
