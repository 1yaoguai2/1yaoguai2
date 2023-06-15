
using System;
using UnityEngine;

namespace Cat.Animation
{
    public delegate bool OnReturnFlagHandle();
    public enum CatAnimationStatus
    {
        None,
        Start,//第一帧
        Playing,
        Paused,
        Completed,
        Abort
    }
    public class CatAnimation
    {
        /// <summary>
        /// 需要运动的实体
        /// </summary>
        public Transform TransformSource;
        public CatAnimationStatus State { get; private set; } = CatAnimationStatus.None;
        //public CatAnimationStatus StateCache = CatAnimationStatus.None;
        public event Action<CatAnimation> OnPlayStart;
        public event Action<CatAnimation> OnPlayUpdate;
        public event Action<CatAnimation> OnPlayComplete;
        public event Action<CatAnimation> OnPause;
        public event Action<CatAnimation> OnContinue;
        public event Action<CatAnimation> OnAbort;
        public void onPlayStart()
        {
            try
            {
                State = CatAnimationStatus.Start;
                OnPlayStart?.Invoke(this);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            
        }
        public virtual void onPlayUpdate()
        {
            try
            {
                State = CatAnimationStatus.Playing;
                OnPlayUpdate?.Invoke(this);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
        }
        public void onPlayComplete()
        {
            try
            {
                State = CatAnimationStatus.Completed;
                OnPlayComplete?.Invoke(this);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        public void Pause()
        {
            try
            {
                //StateCache = State;
                State = CatAnimationStatus.Paused;
                OnPause?.Invoke(this);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        public void Continue()
        {
            try
            {
                State = CatAnimationStatus.Playing;
                OnContinue?.Invoke(this);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        public void Abort()
        {
            try
            {
                State = CatAnimationStatus.Abort;
                OnAbort?.Invoke(this);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            
        }
    }

}