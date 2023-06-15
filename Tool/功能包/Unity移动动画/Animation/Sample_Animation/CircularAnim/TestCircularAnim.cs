using Cat.Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCircularAnim : MonoBehaviour
{
    public Transform MoveObj;
    public Transform center;
    public float radius;
    public float speed;
    public Transform tra;
    public Transform trb;
    public void Test()
    {
        var a = tra.position;
        var b = trb.position;
        CircularAnimation anim = new CircularAnimation(center.position, radius, center.up, a, speed);
        MoveObj.PlayAnimation(anim);
    }
    // Start is called before the first frame update
    void Start()
    {
        Test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float smoothness = 0.1f; // 值越低圆环越平滑
    public Color m_Color = Color.green; // 线框颜色

    void OnDrawGizmos()
    {
        smoothness = Mathf.Max(smoothness, 1 * -4f);

        //Gizmos.matrix = this.center.localToWorldMatrix;
        Vector3 beginPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        for (float step = 0; step < 2 * Mathf.PI; step += smoothness)
        {
            //var p = center.position + Vector3.Cross(znormal, plane.up);
            //Vector3 endPoint = p;
            float x = radius * Mathf.Cos(step);
            float z = radius * Mathf.Sin(step);
            Vector3 endPoint = new Vector3(x, 0, z);
            if (step == 0)
                firstPoint = endPoint;
            else
                Gizmos.DrawLine(beginPoint, endPoint);
            beginPoint = endPoint;
        }
        Gizmos.DrawLine(firstPoint, beginPoint);



        var plane = new Plane(center.up, center.position);
        var a = plane.ClosestPointOnPlane(tra.position);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(a, .05f);

        var b = plane.ClosestPointOnPlane(trb.position);
        Gizmos.DrawSphere(b, .05f);
        void ProjectOnCircular(Vector3 vector)
        {
            var op = vector - center.position;
            var opi = op.normalized * radius;
            var pi = center.position + opi;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pi, .05f);

            var tanLine = Vector3.Cross(opi,center.up);
            var moveDir = tanLine.normalized;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(pi, moveDir);
            Gizmos.color = Color.black;
            Gizmos.DrawRay(pi, -moveDir);
        }
        ProjectOnCircular(a);
        ProjectOnCircular(b);
    }
}
