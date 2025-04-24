using System;
using GameSystem;
using UnityEngine;

namespace TicTakToe
{
    /// <summary>
    /// ������ʹ�õĻغ�
    /// </summary>
    public class TicTakToeStep : GameStep
    {
   /// <summary>
   /// ��ϻغ�
   /// </summary>
        public override void ChokeStep()
        {
            base.StopCoroutine(this.stepCoroutine);
            this.stepCoroutine = null;
        }

        /// <summary>
        /// �غ���
        /// </summary>
        /// <param name="elaspedTime"></param>
        public override void InStep(float elaspedTime)
        {
            this.stepPercent = elaspedTime / this.stepDurationTime;
            TicTakToeEventHandler.CallMneuUIUpdate(MonoSingleton<StepManager>.Instance.currentTic);
           // Debug.Log("0002 Stepִ���У��ȴ�����" + this.stepID.ToString() + " %" + this.stepPercent.ToString());
        }

  
        public override void InSumming(float elapsedTime)
        {
          //  Debug.Log("0009 Step�����У��ȴ�����" + this.stepID.ToString());
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
          //  Debug.Log("0003 Step����" + this.stepID.ToString());
            this.OnSumStart();
            this.sumCoroutine = base.StartCoroutine(base.PerformSum());
        }

        public override void OnStepStart()
        {
            this.stepPercent = 1f;
           // Debug.Log("0003 Step��ʼ" + this.stepID.ToString());
            TicTakToeEventHandler.TicOperationEvent += this.WaitOp;
            base.StateTransition(this.currentState, GameStep.StepState.WaitingForOp);
        }

        private void WaitOp(Tic tic)
        {
          //  Debug.Log("0005 Step�ȵ�������" + this.stepID.ToString());
            this.isOped = true;
        }

        public override void OnSumEnd()
        {
            //Debug.Log("0003 Step�������" + this.stepID.ToString());
            base.StopCoroutine(this.sumCoroutine);
            this.sumCoroutine = null;
            base.StateTransition(this.currentState, GameStep.StepState.StepEnd);
            TicTakToeEventHandler.CallChangeStep();
            Destroy(this);
        }

        public override void OnSumStart()
        {
            Debug.Log("0003 Step���㿪ʼ" + this.stepID.ToString());
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
