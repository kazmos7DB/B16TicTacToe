using System;
using System.Collections;
using UnityEngine;

namespace GameSystem
{
 /// <summary>
 /// 回合制游戏定义的回合,包括回合阶段和结算阶段
 /// OnStepStart > InStep > OnStepEnd > OnSumStart > InSumming > OnSumEnd 
 /// 
 /// </summary>
    public abstract class GameStep : MonoBehaviour
    {
        public abstract void Init(int ID, float _stepDurationTime, float _sumDurationTime);
        public void WaitOp()
        {
            this.isOped = true;
        }
        public void InvokeStep()
        {
            Debug.Log("0001 Step被唤起");
            if (this.stepCoroutine == null)
            {
                this.stepCoroutine = base.StartCoroutine(this.PerformStep());
                this.StateTransition(this.currentState, GameStep.StepState.StepStart);
                this.OnStepStart();
                return;
            }
            base.StopCoroutine(this.stepCoroutine);
            this.stepCoroutine = base.StartCoroutine(this.PerformStep());
            this.StateTransition(this.currentState, GameStep.StepState.StepStart);
            this.OnStepStart();
        }

        public abstract void OnStepStart();

        public abstract void OnStepEnd();

        public abstract void ChokeStep();

        public abstract void InStep(float elaspedTime);
        public abstract void InSumming(float elapsedTime);
        public abstract void OnSumEnd();

        public abstract void OnSumStart();

        protected void InvokeSum()
        {
            if (this.stepCoroutine == null)
            {
                this.stepCoroutine = base.StartCoroutine(this.PerformSum());
            }
        }

        public IEnumerator PerformStep()
        {
            float elapsedTime = 0f;
            while (!this.isOped && elapsedTime <= this.stepDurationTime)
            {
                this.InStep(elapsedTime);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            this.OnStepEnd();
            yield break;
        }

        public IEnumerator PerformSum()
        {
            for (float elapsedTime = 0f; elapsedTime < this.sumDurationTime; elapsedTime += Time.unscaledDeltaTime)
            {
                this.InSumming(elapsedTime);
                yield return null;
            }
            this.OnSumEnd();
            yield break;
        }

        public void StateTransition(GameStep.StepState src, GameStep.StepState tar)
        {
            this.prevState = src;
            this.currentState = tar;
        }


        public int stepID;

        protected Coroutine stepCoroutine;

        protected Coroutine sumCoroutine;

        public float stepDurationTime = 5f;

        public float sumDurationTime = 2f;
        public bool isOped;
        public GameStep.StepState prevState;
        public GameStep.StepState currentState;
        public float stepPercent;
        [Serializable]
        public enum StepState
        {
            WaitingForOp,
            StepStart,
            StepEnd,
            Summing,
            Error,
            Other
        }
    }
}
