using System;
using GameSystem;
using UnityEngine;

namespace TicTakToe
{
    /// <summary>
    /// 井字棋使用的回合
    /// </summary>
    public class TicTakToeStep : GameStep
    {
   /// <summary>
   /// 打断回合
   /// </summary>
        public override void ChokeStep()
        {
            base.StopCoroutine(this.stepCoroutine);
            this.stepCoroutine = null;
        }

        /// <summary>
        /// 回合中
        /// </summary>
        /// <param name="elaspedTime"></param>
        public override void InStep(float elaspedTime)
        {
            this.stepPercent = elaspedTime / this.stepDurationTime;
            TicTakToeEventHandler.CallMneuUIUpdate(MonoSingleton<StepManager>.Instance.currentTic);
           // Debug.Log("0002 Step执行中，等待输入" + this.stepID.ToString() + " %" + this.stepPercent.ToString());
        }

  
        public override void InSumming(float elapsedTime)
        {
          //  Debug.Log("0009 Step结算中，等待输入" + this.stepID.ToString());
        }

        public override void OnStepEnd()
        {
            this.stepPercent = 0f;
            if (MonoSingleton<StepManager>.Instance.currentTic == MonoSingletonSerialized<TicTakToeManager>.Instance.enemyTicType)
            {
                MonoSingletonSerialized<TicTakToeManager>.Instance.aiController.ActTic();
            }
            base.StateTransition(this.currentState, GameStep.StepState.Summing);
            this.stepCoroutine = null;
          //  Debug.Log("0003 Step结束" + this.stepID.ToString());
            this.OnSumStart();
            this.sumCoroutine = base.StartCoroutine(base.PerformSum());
        }

        public override void OnStepStart()
        {
            this.stepPercent = 1f;
           // Debug.Log("0003 Step开始" + this.stepID.ToString());
            TicTakToeEventHandler.TicOperationEvent += this.WaitOp;
            base.StateTransition(this.currentState, GameStep.StepState.WaitingForOp);
        }

        private void WaitOp(Tic tic)
        {
          //  Debug.Log("0005 Step等到了输入" + this.stepID.ToString());
            this.isOped = true;
        }

        public override void OnSumEnd()
        {
            //Debug.Log("0003 Step结算结束" + this.stepID.ToString());
            base.StopCoroutine(this.sumCoroutine);
            this.sumCoroutine = null;
            base.StateTransition(this.currentState, GameStep.StepState.StepEnd);
            TicTakToeEventHandler.CallChangeStep();
            Destroy(this);
        }

        public override void OnSumStart()
        {
            Debug.Log("0003 Step结算开始" + this.stepID.ToString());
            base.StateTransition(this.currentState, GameStep.StepState.Summing);
        }

        public override void Init(int ID, float _stepDurationTime, float _sumDurationTime)
        {
            this.stepID = ID;
            this.stepDurationTime = _stepDurationTime;
            this.sumDurationTime = _sumDurationTime;
        }
    }
}
