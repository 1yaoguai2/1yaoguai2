using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.EquipController.Base;
using UnityEngine;

public class RunningOperator : MonoBehaviour
{
    public List<Operator> _operators = new List<Operator>();
    public List<Operator> _operatorsElements = new List<Operator>();

    private void Start()
    {
        CreateOpertor();
        //或者
        CreateOpertorElements();
    }

    private void Update()
    {
        DoubleRunning();
        //或者
        Running();
    }

    public void CreateOpertor()
    {
        var op1 = new Operator();
        var op2 = new Operator();

        op1.OperatorDoAction += () =>
        {
            CreateOpertorElements(op1);
            op1.DoComplete();
        };

        op2.OperatorDoAction += () =>
        {
            CreateOpertorElements(op2);
            op2.DoComplete();
        };
        op2.AddPrior(op1);

        op1.OnComplete += () => { };
        op2.OnComplete += () => { };

        //增加
        _operatorsElements.Add(op1);
        _operatorsElements.Add(op2);
    }
    
    public void CreateOpertorElements(Operator op = null)
    {
        var op1 = new Operator();
        var op2 = new Operator();
        var op3 = new Operator();

        op1.OperatorDoAction += () => { op1.DoComplete(); };

        op2.OperatorDoAction += () => { op2.DoComplete(); };
        op2.AddPrior(op1);

        op3.OperatorDoAction += () => { op3.DoComplete(); };
        op3.AddPrior(op2);

        op1.OnComplete += () => { };
        op2.OnComplete += () => { };
        op3.OnComplete += () =>
        {
            //TODO: 结束上一层操作
            op?.DoComplete();
        };

        //增加
        _operatorsElements.Add(op1);
        _operatorsElements.Add(op2);
        _operatorsElements.Add(op3);
    }


    /// <summary>
    /// 双层
    /// </summary>
    private void DoubleRunning()
    {
        if (_operators.Count > 0) //命令段
        {
            foreach (var t in _operators)
            {
                if (t.IsAllow())
                    t.Do();
            }

            var allComplete = false;
            foreach (var t in _operators)
            {
                allComplete = t.Status == OperatorStatus.Complete;
                if (!allComplete) break;
            }

            if (allComplete)
            {
                //耗时记录

                //MotionAnalysis.Push(crane, TimeSpan.FromSeconds(TotalElapsed).Multiply((int)m_rate));
                //Logger.Log($"提交时间{TotalElapsed}");

                _operators.Clear();
            }
        }

        if (_operatorsElements.Count > 0) //操作内容
        {
            foreach (var t in _operatorsElements)
            {
                if (t.IsAllow())
                    t.Do();
            }

            var allComplete = false;
            foreach (var t in _operatorsElements)
            {
                allComplete = t.Status == OperatorStatus.Complete;
                if (!allComplete) break;
            }

            if (!allComplete) return;

            _operatorsElements.Clear();
        }
    }

    /// <summary>
    /// 单层
    /// </summary>
    private void Running()
    {
        if (_operatorsElements.Count > 0) //操作内容
        {
            foreach (var t in _operatorsElements)
            {
                if (t.IsAllow())
                    t.Do();
            }

            var allComplete = false;
            foreach (var t in _operatorsElements)
            {
                allComplete = t.Status == OperatorStatus.Complete;
                if (!allComplete) break;
            }

            if (!allComplete) return;

            _operatorsElements.Clear();
        }
    }
}