
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.Animation
{
    public static class CatAnimationController
    {
       public static void PlayAnimation(this Transform source, CatAnimation ual)
        {
            //运动的本质是Transform(所以这个动画状态机，设计成了一个单transform的，也不知道以后会不会出现瓶颈)
            ual.TransformSource = source;

            //动画入队
            EnQueue(ual);

            //动画出队（动画完成后）
            ual.OnPlayComplete += (_) => { DeQueue(ual); };
        }

        public static List<CatAnimation> AnimationPlayingHandles { get; private set; } = new List<CatAnimation>();
        /// <summary>
        /// 入队（将动画放置执行队列），这个队列，没有先后概念
        /// </summary>
        static void EnQueue(CatAnimation animation)
        {
            //加入到update队列
            AnimationPlayingHandles.Add(animation);
        }
        internal static void DeQueue(CatAnimation animation)
        {
            //从update队列中移出
            AnimationPlayingHandles.Remove(animation);
        }
        static CatAnimationController()
        {
            //Debug.Log("编译动画控制器");
            //检查Unity当前Hierarchy面板上有没有一个CatAnimation
            CatAnimationBehavior[] cats = GameObject.FindObjectsOfType<CatAnimationBehavior>();
            if (cats.Length > 1)
                throw new Exception("当前场景中存在多个" + nameof(CatAnimationBehavior) + ",合理操作是一个都不挂");
            if (cats.Length == 0)
            {
                GameObject g = new GameObject("@" + nameof(CatAnimationBehavior));
                g.AddComponent<CatAnimationBehavior>();
                if (GameObject.Find("@Cat"))
                    g.transform.SetParent(GameObject.Find("@Cat").transform);
            }
        }
    }
}