#region Information
/*----------------------------------------------------------------
 * User    : Arno (Copyright (C))
 * Time    ：2021/1/8 09:37:03
 * CLR     ：4.0.30319.42000
 * filename：LineAnimation
 * E-male  ：iot_zx@163.com
 
 * Desc    : 匀加速直线运动
 *
 * ----------------------------------------------------------------
 * Modification
 *  - 至于怎么写的，重新温习下高中匀加速的物理课就完了，看这段代码是不好使的
 *  - 2021.01.27日
 *    - 发现有东西算错了，设可以最大速度，这是时候的t = Vtemp / a 
 *    - 总结出这个算法的误差来源就是下面这个开方数：
 *    Ta_all = Mathf.Sqrt(
                    (s * Mathf.Pow(d, 2)) / (a * Mathf.Pow(d, 2) / 2 + d * Mathf.Pow(a, 2) / 2)
                    );
 *----------------------------------------------------------------*/
#endregion


using System;
using UnityEngine;

namespace Cat.Animation
{
    /// <summary>
    /// 匀加速直线运动（已知加速度，减速度，终点位置）
    /// Uniform Acceleration Line
    /// </summary>
    public class UALAnimation : CatAnimation
    {
        float _a;//加速度
        float _d;//减速度
        float _Vm;//最大速度
        Vector3 _start;//起点（供计算）
        Vector3 _end;//终点（供计算）
        float _s;//总长度
        Vector3 _dic;//方向
        float _surplus;//剩余长度

        float _sacc;//假如可以最大速度，那么达到最大速度后，加速要跑多长
        float _sdec;//假如可以最大速度，那么达到最大速度后，减速要跑多长
        float _svm;//假如可以最大速度，那么达到最大速度要跑多长
        float _Vc = 0;//当前速度
        float _Vtmp = 0;//如果不能最大速度，那么加速减速转换瞬间的速度
        float _Tvm_all;//假如可以最大速度，那么达到最大速度后，以最大速度运行时长
        float _Ta_all;//不够最大速度时，加速用时
        float _Td_all;//不够最大速度时，减速速用时
        int _flow = 0;//过程
        float _proportion = 1;//分量。1为全部，0.5为做一半，以此类推
        /// <summary>
        /// 匀加速直线运动配置
        /// </summary>
        /// <param name="acceleration">加速度</param>
        /// <param name="deceleration">减速度</param>
        /// <param name="maxSpeed">最大速度</param>
        /// <param name="consultPosition">运动计算参照物（用哪个位置来测算运动距离及方向）</param>
        /// <param name="targetPosition">目标位置</param>
        public UALAnimation(float acceleration,
                            float deceleration,
                            float maxSpeed,
                            Vector3 consultPosition,
                            Vector3 targetPosition,
                            float proportion = 1)
        {
            if (deceleration <= 0)
                throw new ArgumentException("不要输入<=0的减速度,也没啥，就是不喜欢");
            this._a = acceleration;
            this._d = deceleration;//减速度在这里被取反了，所以对于外部而言，应该传入正值
            this._Vm = maxSpeed;
            this._start = consultPosition;
            this._end = targetPosition;
            this._proportion = proportion;

            CheckingCalculation();
        }
        /// <summary>
        /// 匀加速直线运动配置
        /// </summary>
        /// <param name="acceleration">加速度</param>
        /// <param name="deceleration">减速度</param>
        /// <param name="maxSpeed">最大速度</param>
        /// <param name="consultPosition">运动计算参照物（用哪个位置来测算运动距离及方向）</param>
        /// <param name="targetPosition">目标位置</param>
        /// <param name="axial">运动轴向</param>
        public UALAnimation(float acceleration,
                            float deceleration,
                            float maxSpeed,
                            Vector3 consultPosition,
                            Vector3 targetPosition,
                            LineAxial axial,
                            float proportion = 1)

        {
            if (deceleration <= 0)
                throw new ArgumentException("不要输入<=0的减速度,也没啥，就是不喜欢");
            this._a = acceleration;
            this._d = deceleration;//减速度在这里被取反了，所以对于外部而言，应该传入正值
            this._Vm = maxSpeed;
            this._start = consultPosition;
            this._end = targetPosition;
            this._proportion = proportion;

            switch (axial)
            {
                case LineAxial.X:
                    this._end.y = this._start.y;
                    this._end.z = this._start.z;
                    break;
                case LineAxial.Y:
                    this._end.x = this._start.x;
                    this._end.z = this._start.z;
                    break;
                case LineAxial.Z:
                    this._end.x = this._start.x;
                    this._end.y = this._start.y;
                    break;
            }
            CheckingCalculation();
        }

        /// <summary>
        /// 【这个签名是本地坐标系！】匀加速直线运动配置
        /// </summary>
        /// <param name="acceleration">加速度</param>
        /// <param name="deceleration">减速度</param>
        /// <param name="maxSpeed">最大速度</param>
        /// <param name="consultPosition">运动计算参照物（用哪个位置来测算运动距离及方向）</param>
        /// <param name="targetPosition">目标位置</param>
        /// <param name="localAxial">本地运动轴向</param>
        /// <param name="consultRotation">运动计算参考物朝向，使用该值后，运动计算轴向将变成本地空间</param>
        public UALAnimation(float acceleration,
                            float deceleration,
                            float maxSpeed,
                            Vector3 consultPosition,
                            Vector3 targetPosition,
                            LineAxial localAxial,
                            Quaternion consultRotation,
                            float proportion = 1)

        {
            if (deceleration <= 0)
                throw new ArgumentException("不要输入<=0的减速度,也没啥，就是不喜欢");
            this._a = acceleration;
            this._d = deceleration;//减速度在这里被取反了，所以对于外部而言，应该传入正值
            this._Vm = maxSpeed;
            this._start = consultPosition;
            this._end = targetPosition;
            this._proportion = proportion;


            Vector3 localDir = Vector3.zero;
            switch (localAxial)
            {
                case LineAxial.X:
                    localDir = consultRotation * Vector3.right;
                    break;
                case LineAxial.Y:
                    localDir = consultRotation * Vector3.up;
                    break;
                case LineAxial.Z:
                    localDir = consultRotation * Vector3.forward;
                    break;
            }
            var cos = Vector3.Dot(localDir, (_end - _start));
            this._end = _start + (localDir * cos);
            //Debug.DrawLine(this.start, this.end, Color.red, 88);
            CheckingCalculation();
        }

        /// <summary>
        /// 匀加速直线运动配置
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="deceleration"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="consultPosition"></param>
        /// <param name="targetPosition"></param>
        /// <param name="oxyzPlane"></param>
        public UALAnimation(float acceleration,
                            float deceleration,
                            float maxSpeed,
                            Vector3 consultPosition,
                            Vector3 targetPosition,
                            LinePlane oxyzPlane,
                            float proportion = 1)
        {
            if (deceleration <= 0)
                throw new ArgumentException("不要输入<=0的减速度,也没啥，就是不喜欢");
            this._a = acceleration;
            this._d = deceleration;//减速度在这里被取反了，所以对于外部而言，应该传入正值
            this._Vm = maxSpeed;
            this._start = consultPosition;
            this._end = targetPosition;
            this._proportion = proportion;
            switch (oxyzPlane)
            {
                case LinePlane.OXZ:
                    this._end.y = this._start.y;
                    break;
                case LinePlane.OZY:
                    this._end.x = this._start.x;
                    break;
                case LinePlane.OXY:
                    this._end.z = this._start.z;
                    break;
            }
            CheckingCalculation();
        }

        /// <summary>
        /// 【这个签名是本地坐标系！】匀加速直线运动配置
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="deceleration"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="consultPosition"></param>
        /// <param name="targetPosition"></param>
        /// <param name="localOxyzPlane"></param>
        public UALAnimation(float acceleration,
                            float deceleration,
                            float maxSpeed,
                            Vector3 consultPosition,
                            Vector3 targetPosition,
                            LinePlane localOxyzPlane,
                            Quaternion consultRotation,
                            float proportion = 1)
        {
            if (deceleration <= 0)
                throw new ArgumentException("不要输入<=0的减速度,也没啥，就是不喜欢");
            this._a = acceleration;
            this._d = deceleration;//减速度在这里被取反了，所以对于外部而言，应该传入正值
            this._Vm = maxSpeed;
            this._start = consultPosition;
            this._end = targetPosition;
            this._proportion = proportion;
            Vector3 normal;
            switch (localOxyzPlane)
            {
                case LinePlane.OXZ:
                    normal = consultRotation * Vector3.up;
                    break;
                case LinePlane.OZY:
                    normal = consultRotation * Vector3.right;
                    break;
                case LinePlane.OXY:
                    normal = consultRotation * Vector3.forward;
                    break;
                default: throw new Exception("plane");
            }
            //以normal为法线的平面，用第一个参数过去求dot
            //也可以理解为一个向量在这个平面上的投影
            var destination = Vector3.ProjectOnPlane(targetPosition - consultPosition, normal);
            this._end = this._start + destination;
            CheckingCalculation();
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

        public bool PlayUpdate()
        {
            //Debug.Log("当前速度" + Vc);
            if (_dic.magnitude == 0)//如果方向向量的模=0，说明物体已经在目标位置上了，不用算了
                return true;
            if (_svm > 0)//可以达到最大速度
            {
                if (_flow == 0)
                {
                    //Debug.Log("加速阶段");
                    if (_surplus < _sacc)
                    {
                        //如果单位距离比总长还长？
                        var nowdis = _dic.magnitude * Time.fixedDeltaTime * _Vc;// 速度*0.02m/0.02s
                        nowdis = _surplus + nowdis > _sacc ? _sacc - _surplus : nowdis;//速度过大，那就一帧到位
                        base.TransformSource.Translate(_dic * nowdis, Space.World);
                        _surplus += nowdis;
                        _Vc += _a * Time.fixedDeltaTime;
                    }
                    else
                    {
                        _flow = 1;
                        _surplus = 0;
                        _Vc = _Vm;
                    }
                }
                if (_flow == 1)
                {
                    //Debug.Log("最大速度阶段");
                    if (_surplus < _svm)
                    {
                        var nowdis = _dic.magnitude * Time.fixedDeltaTime * _Vc;
                        nowdis = _surplus + nowdis > _svm ? _svm - _surplus : nowdis;//速度过大，那就一帧到位
                        base.TransformSource.Translate(_dic * nowdis, Space.World);
                        _surplus += nowdis;
                    }
                    else
                    {
                        _flow = 2;
                        _surplus = 0;
                        _Vc = _Vm;
                    }
                }
                if (_flow == 2)
                {
                    //Debug.Log("减速阶段");
                    if (_surplus < _sdec)
                    {
                        var nowdis = _dic.magnitude * Time.fixedDeltaTime * _Vc;
                        nowdis = _surplus + nowdis > _sdec ? _sdec - _surplus : nowdis;//速度过大，那就一帧到位
                        base.TransformSource.Translate(_dic * nowdis, Space.World);
                        _surplus += nowdis;
                        _Vc -= _d * Time.fixedDeltaTime;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (_flow == 0)
                {
                    //Debug.Log("加速阶段");
                    if (_surplus < _sacc)
                    {
                        var nowdis = _dic.magnitude * Time.fixedDeltaTime * _Vc;// 速度*0.02m/0.02s
                        nowdis = _surplus + nowdis > _sacc ? _sacc - _surplus : nowdis;//速度过大，那就一帧到位
                        base.TransformSource.Translate(_dic * nowdis, Space.World);
                        _surplus += nowdis;
                        _Vc += _a * Time.fixedDeltaTime;
                    }
                    else
                    {
                        _flow = 1;
                        _surplus = 0;
                        _Vtmp = _Vc;
                    }
                }
                if (_flow == 1)
                {
                    //Debug.Log("减速阶段");
                    if (_surplus < _sdec)
                    {
                        var nowdis = _dic.magnitude * Time.fixedDeltaTime * _Vc;
                        nowdis = _surplus + nowdis > _sdec ? _sdec - _surplus : nowdis;//速度过大，那就一帧到位
                        base.TransformSource.Translate(_dic * nowdis, Space.World);
                        _surplus += nowdis;
                        _Vc -= _d * Time.fixedDeltaTime;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 计算辅助参数
        /// </summary>
        void CheckingCalculation()
        {
            _dic = (_end - _start).normalized;
            _s = Vector3.Distance(_start, _end) * _proportion;
            //(假如)可以最大速度，那么达到最大速度要跑多长
            _sacc = _a * Mathf.Pow(_Vm / _a, 2) / 2;
            //(假如)可以最大速度，那么从最大速度减到0要跑多长
            _sdec = _d * Mathf.Pow(_Vm / _d, 2) / 2;
            //(假设)计算达到最大速度后，以最大速度运行多久（注意这个时间在邻接值时，是0，不处理）
            _Tvm_all = (_s - (_sacc + _sdec)) / _Vm;
            //(假设)不够最大速度时，加速用时
            _Ta_all = Mathf.Sqrt(
                    (_s * Mathf.Pow(_d, 2)) / (_a * Mathf.Pow(_d, 2) / 2 + _d * Mathf.Pow(_a, 2) / 2)
                    );
            //(假设)不够最大速度时，减速用时
            _Td_all = _a * _Ta_all / _d;

            if (_Tvm_all >= 0)//可以达到最大速度
            {
                _svm = _s - (_sacc + _sdec);//计算达到最大速度后，以最大速度运行多长
            }
            else//还不能达到最大速度，就需要减速
            {
                //假如可以最大速度，那么达到最大速度要跑多长
                _sacc = _a * Mathf.Pow(_Ta_all, 2) / 2;
                //sdec = d * Mathf.Pow(Td_all, 2) / 2;
                _sdec = _s - _sacc;//修正，解决掉精度问题
            }
            //Debug.Log("实际总长" + s);
            //Debug.Log("计算加速需要距离" + sacc);
            //Debug.Log("计算最大速度距离" + svm);
            //Debug.Log("计算减速需要减速" + sdec);
        }
    }
}