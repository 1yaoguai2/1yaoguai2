

using UnityEngine;

namespace Cat.Animation
{
    public class CatAnimationBehavior : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        /// <summary>
        /// CatAnimation的Mono执行器
        /// </summary>
        private void FixedUpdate()
        {
            if (CatAnimationController.AnimationPlayingHandles.Count <= 0) 
                return;


            //这里不能用foreach，迭代器的值是不允许修改的，
            //这里执行过程中，用事件回调的方式修改了AnimationPlayingHandles的值，
            //这种写法骗过了编译器，然而并没有什么鸟用,运行过程中依然会提示Collection was modified

            for (int i = 0; i < CatAnimationController.AnimationPlayingHandles.Count; i++)
            {
                CatAnimation _;
                try
                {
                    _ = CatAnimationController.AnimationPlayingHandles[i];
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    Debug.LogError("出错");
                    continue;
                }
                //CatAnimation animation = CatAnimationController.AnimationPlayingHandles[i];
                if (_.TransformSource == null)//切换场景时，或者销毁时
                {
                    CatAnimationController.DeQueue(_);//从动画集中移出
                    continue;
                }
                if (_.State == CatAnimationStatus.Paused || _.State == CatAnimationStatus.Abort)
                {
                    continue;
                }
                _.onPlayUpdate();
            }
        }
    }
}