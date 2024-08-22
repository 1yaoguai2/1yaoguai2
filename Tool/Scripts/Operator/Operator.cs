using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.EquipController.Base
{
    public enum OperatorStatus
    {   
        //未调用
        NotCall,
        //正在执行
        Calling,
        //已经完成的
        Complete,
    }
    public class Operator
    {
         public Operator()
        {

        }
        public Operator(Action operatorAction)
        {
            if (operatorAction == null)
                throw new ArgumentNullException();
            OperatorDoAction = operatorAction;
        }
        
        /// <summary>
        /// 前置操作
        /// </summary>
        private List<Operator> _priors;
        
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc;
        
        /// <summary>
        /// 当前操作
        /// </summary>
        public Action OperatorDoAction;
        
        /// <summary>
        /// 当操作完成的回调
        /// </summary>
        public event Action OnComplete;
        
        /// <summary>
        /// 当前操作的状态
        /// </summary>
        public OperatorStatus Status { get; private set; } = OperatorStatus.NotCall;
        public Operator AddPrior(Operator prior)
        {
            if (_priors == null)
                _priors = new List<Operator>();
            _priors.Add(prior);
            return this;
        }
        /// <summary>
        /// 检查能否执行
        /// </summary>
        /// <returns></returns>
        public bool IsAllow()
        {
            //已经完成的，不允许再操作，（处理的好的话，完成的时候，应该从内存里面丢掉了才对）
            if (this.Status == OperatorStatus.Complete)
                return false;
            //已经开始的，不允许
            if (this.Status == OperatorStatus.Calling)
                return false;
            //没有前置，可以操作
            if (_priors == null)
                return true;

            //前置条件中，全部成功，可以操作
            var allPriorsIsComplete = false;
            foreach (var op in _priors)
            {
                allPriorsIsComplete = op.Status == OperatorStatus.Complete;
                if (!allPriorsIsComplete) break;
            }
            return allPriorsIsComplete;
        }
        
        /// <summary>
        /// 执行
        /// </summary>
        public void Do()
        {
            this.Status = OperatorStatus.Calling;
            this.OperatorDoAction?.Invoke();
        }
        public void DoComplete()
        {
            this.Status = OperatorStatus.Complete;
            OnComplete?.Invoke();
        }
        public void Abort()
        {
            this.OnComplete = null;
            this.OperatorDoAction = null;
        }
        
        /// <summary>
        /// 多操作符都完成时
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="ops"></param>
        public static void MultiComplete(Action callback, params Operator[] ops)
        {
            if (ops.Where(p => p.Status != OperatorStatus.Complete).Count() <= 0)//全完成
            {
                callback?.Invoke();
            }
        }
    }
}