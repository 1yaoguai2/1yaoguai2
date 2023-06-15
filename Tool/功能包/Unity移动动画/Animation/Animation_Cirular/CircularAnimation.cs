#region Information
/*----------------------------------------------------------------
 * User    : Arno (Copyright (C))
 * Time    ：2021/6/24
 * CLR     ：4.0.30319.42000
 * filename：LineAnimation
 * E-male  ：iot_zx@163.com
 
 * Desc    : 匀速圆心运动
 *
 * ----------------------------------------------------------------
 * Modification
 *
 *----------------------------------------------------------------*/
#endregion

using UnityEngine;

namespace Cat.Animation
{
    /// <summary>
    /// 匀速圆心运动
    /// </summary>
    public class CircularAnimation : CatAnimation
    {
        Vector3 _target;//启动时的参考
        Vector3 _center;//圆心
        float _radius;//半径

        Vector3 _planeNormal;//圆所在平面的发现
        float _speed;

        void Construction(Vector3 center,
                             float radius,
                             Vector3 planeNormal,
                             Vector3 target,
                             float speed)
        {
            speed = speed <= 0 ? throw new System.ArgumentException(nameof(LineAnimation) + "The speed needs to be a non-zero positive integer") : speed;
            _radius = radius;
            _center = center;
            _planeNormal = planeNormal;
            _target = target;
            _speed = speed;
        }

        public CircularAnimation(Vector3 center,
                             float radius,
                             Vector3 planeNormal,
                             Vector3 target,
                             float speed)
        {
            Construction(center, radius, planeNormal, target, speed);
        }


        bool init = false;
        public override void onPlayUpdate()
        {
            if (!init)
            {
                init = true;
                base.onPlayStart();
            }
            base.onPlayUpdate();//在每一帧操作之前，先响应外部的监听事件

            if (PlayUpdate())
                base.onPlayComplete();//运动完成后，上报
        }
        private bool PlayUpdate()
        {
            var (pTarget, _) = ProjectOnCircular(this._target);
            var (p, dir) = ProjectOnCircular(base.TransformSource.position);//切点
            base.TransformSource.position = p;
            base.TransformSource.forward = dir;
            var moveDir = dir * _speed;

            base.TransformSource.Translate(moveDir, Space.World);
            //Debug.DrawLine(p, p + dir, Color.blue,0.2f);
            if (Vector3.Distance(pTarget, p) < 1E-1f)
                return true;
            return false;
        }

        (Vector3, Vector3) ProjectOnCircular(Vector3 vector)
        {
            var plane = new Plane(_planeNormal, _center);
            vector = plane.ClosestPointOnPlane(vector);
            var op = vector - _center;
            var opi = op.normalized * _radius;
            var pi = _center + opi;
            var tanLine = Vector3.Cross(pi, _planeNormal);//切线向量
            var moveDir = tanLine.normalized;//正向的运动方向，反向取负
            return (pi, moveDir);
        }

        [System.Obsolete]
        void ProjectOnCircular(in Vector3 vector, out Vector3 vectorProject, out Vector3 dir)
        {
            var plane = new Plane(_planeNormal, _center);
            var vectorOnPlane = plane.ClosestPointOnPlane(vector);
            var op = vectorOnPlane - _center;
            var opi = op.normalized * _radius;
            var pi = _center + opi;
            var tanLine = Vector3.Cross(pi, _planeNormal);//切线向量
            var moveDir = tanLine.normalized;//正向的运动方向，反向取负

            vectorProject = pi;
            dir = moveDir;
        }
    }
}
