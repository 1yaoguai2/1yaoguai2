//using Cat.Logger;
//using System;
//using UnityEngine;

//namespace Cat.Animation
//{
//    /*
//     * 2021年1月7日，
//     * 作者：张兴
//     * 至于怎么写的，重新温习下高中匀加速的物理课就完了，看这段代码是不好使的
//     */
//    /// <summary>
//    /// 匀加速直线运动（已知加速度，减速度，终点位置）
//    /// Uniform Acceleration Line
//    /// </summary>
//    public class UALAnimation : CatAnimation
//    {
//        float a;//加速度
//        float d;//减速度
//        float Vm;//最大速度
//        Vector3 start;//起点（供计算）
//        Vector3 end;//终点（供计算）
//        float s;//总长
//        Vector3 dic;//方向
//        float Vc = 0;//当前速度
//        float Ta = 0;//加速期间的耗时
//        float Tvm = 0;//最大速度运行耗时
//        float Td = 0;//减速期间的耗时
//        float sadd;//假如可以最大速度，那么达到最大速度要跑多长
//        float sdec;//假如可以最大速度，那么达到最大速度后，减速要跑多长
//        float Tvm_all;//假如可以最大速度，那么达到最大速度后，以最大速度运行时长
//        float Ta_all;//不够最大速度时，加速用时
//        float Td_all;//不够最大速度时，减速速用时
//        float Vtmp = 0;//不够最大速度时，从加速转化为减速瞬间的速度

//        /// <summary>
//        /// 匀加速直线运动配置
//        /// </summary>
//        /// <param name="acceleration">加速度</param>
//        /// <param name="deceleration">减速度</param>
//        /// <param name="maxSpeed">最大速度</param>
//        /// <param name="consultPosition">运动计算参照物（用哪个位置来测算运动距离及方向）</param>
//        /// <param name="targetPosition">目标位置</param>
//        /// <param name="axial">运动轴向</param>
//        public UALAnimation(float acceleration, float deceleration, float maxSpeed, Vector3 consultPosition, Vector3 targetPosition, LineAxial axial)
//        {
//            if (deceleration <= 0)
//                throw new ArgumentException("不要输入<=0的减速度,也没啥，就是不喜欢");
//            this.a = acceleration;
//            this.d = deceleration;//减速度在这里被取反了，所以对于外部而言，应该传入正值
//            this.Vm = maxSpeed;
//            this.start = consultPosition;
//            this.end = targetPosition;
//            switch (axial)
//            {
//                case LineAxial.X:
//                    this.end.y = this.start.y;
//                    this.end.z = this.start.z;
//                    break;
//                case LineAxial.Y:
//                    this.end.x = this.start.x;
//                    this.end.z = this.start.z;
//                    break;
//                case LineAxial.Z:
//                    this.end.x = this.start.x;
//                    this.end.y = this.start.y;
//                    break;
//            }
//            CheckingCalculation();
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="acceleration"></param>
//        /// <param name="deceleration"></param>
//        /// <param name="maxSpeed"></param>
//        /// <param name="consultPosition"></param>
//        /// <param name="targetPosition"></param>
//        /// <param name="oxyzPlane"></param>
//        public UALAnimation(float acceleration, float deceleration, float maxSpeed, Vector3 consultPosition, Vector3 targetPosition, OxyzPlane oxyzPlane)
//        {
//            if (deceleration <= 0)
//                throw new ArgumentException("不要输入<=0的减速度,也没啥，就是不喜欢");
//            this.a = acceleration;
//            this.d = deceleration;//减速度在这里被取反了，所以对于外部而言，应该传入正值
//            this.Vm = maxSpeed;
//            this.start = consultPosition;
//            this.end = targetPosition;
//            switch (oxyzPlane)
//            {
//                case OxyzPlane.OXZ:
//                    this.end.y = this.start.y;
//                    break;
//                case OxyzPlane.OZY:
//                    this.end.y = this.start.x;
//                    break;
//                case OxyzPlane.OXY:
//                    this.end.y = this.start.z;
//                    break;
//            }
//            CheckingCalculation();
//        }
//        /// <summary>
//        /// 匀加速直线运动配置
//        /// </summary>
//        /// <param name="acceleration">加速度</param>
//        /// <param name="deceleration">减速度</param>
//        /// <param name="maxSpeed">最大速度</param>
//        /// <param name="consultPosition">运动计算参照物（用哪个位置来测算运动距离及方向）</param>
//        /// <param name="targetPosition">目标位置</param>
//        public UALAnimation(float acceleration, float deceleration, float maxSpeed, Vector3 consultPosition, Vector3 targetPosition)
//        {
//            if (deceleration <= 0)
//                throw new ArgumentException("不要输入<=0的减速度,也没啥，就是不喜欢");
//            this.a = acceleration;
//            this.d = deceleration;//减速度在这里被取反了，所以对于外部而言，应该传入正值
//            this.Vm = maxSpeed;
//            this.start = consultPosition;
//            this.end = targetPosition;
//            CheckingCalculation();
//        }

//        bool init = false;
//        public override void onPlayUpdate()
//        {
//            if (!init)
//            {
//                init = true;
//                base.onPlayStart();
//            }
//            base.onPlayUpdate();//在每一帧操作之前，先响应外部的监听事件

//            if (PlayUpdate())
//                base.onPlayComplete();//运动完成后，上报
//        }

//        int flow = 0;
//        public bool PlayUpdate()
//        {
//            //如果每次调用周期大概消耗0.02S，那么运动距离和m/s方程式完全温和
//            if (Tvm_all > 0)//能到达到最大速度
//            {
//                if (flow == 0)
//                {
//                    Debug.Log("加速阶段");
//                    if (Vc < Vm && Ta < Ta_all)
//                    {
//                        Ta += Time.fixedDeltaTime;
//                        Vc = a * Ta;
//                        Vc = Vc > Vm ? Vm : Vc;
//                        if (Ta > Ta_all)
//                        {
//                            Vc = a * Ta_all;
//                            Debug.Log("修正");
//                        }
//                    }
//                    else flow = 1;
//                }
//                if (flow == 1)
//                {
//                    Debug.Log("最大速度阶段");
//                    if (Vc == Vm && Tvm < Tvm_all)
//                    {
//                        Tvm += Time.fixedDeltaTime;
//                        Tvm = Tvm > Tvm_all ? Tvm_all : Tvm;
//                    }
//                    else flow = 2;
//                }
//                if (flow == 2)
//                {
//                    Debug.Log("减速阶段");
//                    if (Vc > 0 && Td < Td_all)
//                    {
//                        Td += Time.fixedDeltaTime;
//                        Vc = Vm - d * Td;
//                        Vc = 0 > Vc ? 0 : Vc;
//                        if (Td > Td_all)
//                        {
//                            Vc = Vm - d * Td_all;
//                            Debug.Log("修正");
//                        }
//                    }
//                    else return true;
//                }
//            }
//            else//不能达到最大速度
//            {
//                Debug.Log("不能最大速度");
//                if (flow == 0)//加速阶段
//                {
//                    Debug.Log("加速阶段");
//                    if (Ta < Ta_all)
//                    {
//                        Ta += Time.fixedDeltaTime;
//                        //Ta = Ta > Ta_all ? Ta_all : Ta;
//                        Vc = a * Ta;
//                        if (Ta > Ta_all)
//                        {
//                            Vc = a * Ta_all;
//                            Debug.Log("修正");
//                        }
//                    }
//                    else
//                    {
//                        flow = 1;
//                        Vtmp = Vc;
//                    };
//                }
//                if (flow == 1)
//                {
//                    if (Td < Td_all)//减速阶段
//                    {
//                        Debug.Log("减速阶段");
//                        Td += Time.fixedDeltaTime;
//                        Vc = Vtmp - d * Td;
//                        if (Td > Td_all)
//                        {
//                            Vc = Vtmp - d * Td_all;
//                            Debug.Log("修正");
//                        }
//                    }
//                    else return true;
//                }
//            }
//            if (Vc < 0)
//            {
//                Debug.Log("负速度" + Vc);
//            }
//            base.TransformSource.Translate(dic * (Time.fixedDeltaTime * Vc), Space.World);
//            return false;
//        }
//        void CheckingCalculation()
//        {
//            dic = end - start;
//            dic.Normalize();
//            s = Vector3.Distance(start, end);
//            //假如可以最大速度，那么达到最大速度要跑多长
//            sadd = a * Mathf.Pow(Vm / a, 2) / 2;
//            sdec = d * Mathf.Pow(Vm / d, 2) / 2;
//            Tvm_all = (s - (sadd + sdec)) / Vm;//计算达到最大速度后，以最大速度运行多久（注意这个时间在邻接值时，是0，不处理）
//            //不够最大速度时，加速用时
//            Ta_all = Mathf.Sqrt(
//                    (s * Mathf.Pow(d, 2)) / (a * Mathf.Pow(d, 2) / 2 + d * Mathf.Pow(a, 2) / 2)
//                    );
//            //不够最大速度时，减速用时
//            Td_all = a * Ta_all / d;
//        }
//    }
//}