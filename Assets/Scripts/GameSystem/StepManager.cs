using System;
using System.Collections;
using TicTakToe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem
{
    /// <summary>
    /// ��Ϸ�غϵĹ������������غ��Ƹ���
    /// </summary>
    public class StepManager : MonoSingleton<StepManager>
    {

        public int currentID;

        public Tic currentTic;

        public GameStep currentStep;

        public GameStep.StepState currentState;
        public bool isStart;

        public float playerStepTime = 5f;
        public float aiStepTime = 1f;
        public float sumTime = 0.2f;
        public void StartGame()
        {
            TicTakToeManager.Instance.SetPlayerTic();
            this.currentTic = Tic.O;
            this.ChangeStep();
            TicTakToeEventHandler.ChangeStepEvent += this.ChangeStep;
            this.isStart = true;
        }
        private void Update()
        {
            if (this.isStart)
            {
                this.currentState = this.currentStep.currentState;
                if (this.currentTic == TicTakToeManager.Instance.playerTicType)
                {
                    TicTakToeManager.Instance.ChangeAllButton(true);
                    return;
                }
                if (this.currentTic == TicTakToeManager.Instance.enemyTicType)
                {
                    TicTakToeManager.Instance.ChangeAllButton(false);
                }
            }
        }
        public void OnRestart()
        {
            this.isStart = false;
            this.currentStep = null;
            this.currentID = 0;
            TicTakToeEventHandler.ChangeStepEvent -= this.ChangeStep;
        }

        public void ChangeStep()
        {
            if (!TicTakToeManager.Instance.isWin)
            {
                if (TicTakToeManager.Instance.CheckDraw(this.currentID))
                {
                    this.isStart = false;
                    UnityEvent<string> onGameEnd = GlobalGameManager.Instance.OnGameEnd;
                    if (onGameEnd != null)
                    {
                        onGameEnd.Invoke("Draw");
                    }
                    TicTakToeEventHandler.ChangeStepEvent -= this.ChangeStep;
                    return;
                }
                base.StartCoroutine(this.ChangeStepRoutine());
                return;
            }
            else
            {
                Debug.Log("3900 ��Ϸ�����ˣ�");
                if (TicTakToeManager.Instance.isPlayerWin)
                {
                    this.isStart = false;
                    UnityEvent<string> onGameEnd2 = GlobalGameManager.Instance.OnGameEnd;
                    if (onGameEnd2 != null)
                    {
                        onGameEnd2.Invoke("PlayerWin");
                    }
                    TicTakToeEventHandler.ChangeStepEvent -= this.ChangeStep;
                    return;
                }
                this.isStart = false;
                UnityEvent<string> onGameEnd3 = GlobalGameManager.Instance.OnGameEnd;
                if (onGameEnd3 != null)
                {
                    onGameEnd3.Invoke("EnemyWin");
                }
                TicTakToeEventHandler.ChangeStepEvent -= this.ChangeStep;
                return;
            }
        }
        /// <summary>
        /// ���ǵ��غ��л������й�������ʹ����Э��,������ʵû����
        /// </summary>
        /// <returns></returns>
        public IEnumerator ChangeStepRoutine()
        {
            while (this.currentStep != null)
            {
                Debug.Log(" stepManager �ȴ��ı䵱ǰStep ");
                yield return null;
            }
            this.currentID++;
            Debug.Log("StepManager �ı䵱ǰStep");
            if (this.currentTic == Tic.X)
            {
                this.currentTic = Tic.O;
            }
            else if (this.currentTic == Tic.O)
            {
                this.currentTic = Tic.X;
            }
            this.currentStep = this.AddComponent<TicTakToeStep>();
            if (this.currentTic == TicTakToeManager.Instance.playerTicType)
            {
                this.currentStep.Init(this.currentID, playerStepTime, sumTime);
            }
            else
            {
                this.currentStep.Init(this.currentID, aiStepTime, sumTime);
            }
            this.currentStep.InvokeStep();
            yield break;
        }


    }
}
