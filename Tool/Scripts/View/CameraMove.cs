using Cat.Animation;
using System.Collections.Generic;
using UnityEngine;

public enum CameraFollow { S100, S101, S102, S200, S201, S202, S300, S301, S302, S400, S401, S402, S1001, S1002, S1003, A601, A602, None }
public class CameraMove : MonoBehaviour
{
    private MainCameraController mainCameraController;
    public List<Transform> cameraPoint = new List<Transform>();
    public bool isMove;
    public int i = 0;
    LineAnimation cameraAnim;
    private Transform cameraTra;
    public List<float> moveSpeed = new List<float>();
    [SerializeField]
    private float rotationSpeed = 0.1f;
    private CameraFollow follow = CameraFollow.None;
    public CameraFollow Follow
    {
        get => follow;
        set
        {
            if (value != follow)
            {
                follow = value;
                SetMainCameraFollow();
            }
        }
    }

    public CameraFollow cameraFollow = CameraFollow.None;

    public List<Transform> followObj = new List<Transform>();
    void Start()
    {
        isMove = false;
        cameraTra = Camera.main.transform;
        mainCameraController = cameraTra.GetComponent<MainCameraController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isMove = true;
        }
        Follow = cameraFollow;
    }

    private void FixedUpdate()
    {

        if (isMove)
        {
            isMove = false;
            if (i == 0)
            {
                cameraTra.position = cameraPoint[i].position;
                cameraTra.rotation = cameraPoint[i].rotation;
            }
            if (i < cameraPoint.Count - 1)
            {
                i++;
                PlayCameraAnim();
            }
            else
            {
                cameraTra.position = cameraPoint[0].position;
                cameraTra.rotation = cameraPoint[0].rotation;
                i = 0;
            }
        }
    }



    /// <summary>
    /// 播放动画
    /// </summary>
    void PlayCameraAnim()
    {
        if (cameraAnim != null) return;
        cameraAnim = new LineAnimation(
            cameraTra.position,
            cameraPoint[i].position,
            moveSpeed[i]
            );
        cameraAnim.OnPlayUpdate += (_) =>
        {
            //cameraTra.Rotate(cameraPoint[i].eulerAngles, Space.Self);
            cameraTra.rotation = Quaternion.Slerp(cameraTra.rotation, cameraPoint[i].rotation, rotationSpeed / moveSpeed[i] * Time.deltaTime);
        };
        cameraAnim.OnPlayComplete += (_) =>
        {
            isMove = false;
            cameraAnim = null;
            mainCameraController.OldMousePos = Input.mousePosition;
        };
        cameraTra.PlayAnimation(cameraAnim);
    }


    private void SetMainCameraFollow()
    {
        Transform trans;
        switch (follow)
        {
            case CameraFollow.None:
                trans = null;
                break;
            default:
                string followName = follow.ToString().Substring(1);
                trans =followObj.Find(t => t.name == followName);
                break;
        }
        cameraTra.transform.parent = trans;
        mainCameraController.OldMousePos = Input.mousePosition;
    }
}
