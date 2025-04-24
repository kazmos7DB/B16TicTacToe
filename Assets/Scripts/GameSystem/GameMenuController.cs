using System;
using TicTakToe;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameSystem
{
    /// <summary>
    /// 游戏菜单和UI的管理器
    /// 遇到了一次工程损坏的情况,使用dnspy反编译了部分代码,看起来会比较怪
    /// </summary>
    public class GameMenuController : MonoSingleton<GameMenuController>
    {
        public GameObject menuRoot;

        private GlobalGameManager ggm;

        [SerializeField]
        private TMP_Text endText;

        [SerializeField]
        private Button startButton;

        [SerializeField]
        private TMP_Text stepText;

        [SerializeField]
        private Slider slider;

        [SerializeField]
        private TMP_Text sliderText;

        [SerializeField]
        private Image timer;
        [SerializeField]
        private TMP_Text timerText;
        private void Start()
        {
            DontDestroyOnLoad(base.gameObject);
            this.ggm = GlobalGameManager.Instance;
            GlobalGameManager.Instance.OnGameEnd.AddListener(new UnityAction<string>(this.EndMenu));
            this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnDifficultySlideChanged));
            TicTakToeEventHandler.UIMenuUpdate += this.UpdateUI;
            this.OnDifficultySlideChanged(this.slider.value);
            this.menuRoot.SetActive(false);
            stepText.text = "";
            timer.fillAmount = 1;
            timerText.text = "<color=#CC2233><size=120%>超级</size></color>\n井字棋!";
        }
        public void OnRestart()
        {
            timer.fillAmount = 1;
            timerText.text = "<size=60%><s>还是</s></size>\n<color=#CC2233><size=120%>超级</size></color>\n井字棋!";
            stepText.text = "";
            this.OnDifficultySlideChanged(this.slider.value);
            this.menuRoot.SetActive(false);
            this.startButton.gameObject.SetActive(true);
            this.slider.gameObject.SetActive(true);
        }
        public void OnExitButton()
        {
            Application.Quit();
        }
        public void EndMenu(string e)
        {
            Debug.Log(" 3901 游戏结束该唤起endmenu了！" + e);
            this.menuRoot.SetActive(true);
            this.slider.gameObject.SetActive(true);
            this.stepText.gameObject.SetActive(false);
            timer.fillAmount = 1;
            timer.color = Color.yellow;
            timerText.text = "游戏结束!";
            int num = MonoSingletonSerialized<TicTakToeManager>.Instance.aiDifficulty;
            if (num <= 80)
            {
                num = (int)((double)num * 1.25);
            }
            else
            {
                num += 20;
            }
            if (e == "PlayerWin")
            {
                this.endText.text = "你赢了！\r\n\r\n<size=60%>回合数：" + StepManager.Instance.currentID.ToString() + "\r\n难度：" + num.ToString();
                return;
            }
            if (e == "EnemyWin")
            {
                this.endText.text = "你输了！\r\n\r\n<size=60%>回合数：" + StepManager.Instance.currentID.ToString() + "\r\n难度：" + num.ToString();
                return;
            }
            if (e == "Draw")
            {
                this.endText.text = "显然平局了！\r\n\r\n<size=60%>回合数：" + StepManager.Instance.currentID.ToString() + "\r\n难度：" + num.ToString();
                return;
            }
            this.endText.text = "显然游戏出<color=CC2233>Bug</color>了！\r\n\r\n<size=60%>回合数：" + 114.ToString() + "\r\n难度：" + 514.ToString();
        }

        public void OnStartButton()
        {
            this.menuRoot.SetActive(false);
            StepManager.Instance.StartGame();
            this.startButton.gameObject.SetActive(false);
            this.stepText.gameObject.SetActive(true);
            this.slider.gameObject.SetActive(false);
        }
        public void OnChangePlayerFirst()
        {
            MonoSingletonSerialized<TicTakToeManager>.Instance.SetPlayerFirst();
        }

        public void OnRestartButton()
        {
            StepManager.Instance.OnRestart();
            this.OnRestart();
            MonoSingletonSerialized<TicTakToeManager>.Instance.OnRestart();
        }

        private void UpdateUI(Tic ticType)
        {
            this.UpdateTimer(ticType);
            this.UpdateStepTextUI();
        }


        private void UpdateStepTextUI()
        {
            float stepDurationTime = StepManager.Instance.currentStep.stepDurationTime;
            float num = StepManager.Instance.currentStep.stepPercent * stepDurationTime;
            int aiDifficulty = MonoSingletonSerialized<TicTakToeManager>.Instance.aiDifficulty;
            string text;
            if (aiDifficulty <= 80)
            {
                text = "当前难度：" + ((int)((double)aiDifficulty * 1.25)).ToString() + "/ 100%";
            }
            else
            {
                text = "当前难度：<color=#CC2233><b><size=150%>" + (aiDifficulty + 20).ToString() + "</color></b></size>/ 100%";
            }
            this.stepText.text = string.Concat(new string[]
            {
                string.Format("剩余时间\n{0:F1}秒 / {1:F0}秒 : {2:F1}%\n", num, stepDurationTime, StepManager.Instance.currentStep.stepPercent * 100f),
                "回合数：",
                StepManager.Instance.currentID.ToString(),
                "\n",
                text
            });
        }

        private void OnDifficultySlideChanged(float slideValue)
        {
            int num = (int)slideValue;
            if (num <= 80)
            {
                this.sliderText.text = "当前难度：" + ((int)((double)num * 1.25)).ToString() + "/ 100%";
            }
            else
            {
                this.sliderText.text = "当前难度：<color=#CC2233><b><size=150%>" + (num + 20).ToString() + "</color></b></size>/ 100%";
            }
            MonoSingletonSerialized<TicTakToeManager>.Instance.SetDifficulty(num);
        }

        private void UpdateTimer(Tic ticType)
        {
            if (this.timer == null)
            {
                Debug.Log("timer Image == null");
                this.timer = GameObject.Find("Timer").GetComponent<Image>();
            }
            this.timer.fillAmount = 1 - StepManager.Instance.currentStep.stepPercent;
            if (ticType == MonoSingletonSerialized<TicTakToeManager>.Instance.enemyTicType)
            {
                this.timer.color = Color.red;
                this.timerText.text = "对手回合";
                return;
            }
            if (ticType == MonoSingletonSerialized<TicTakToeManager>.Instance.playerTicType)
            {
                this.timer.color = Color.green;
                this.timerText.text = "玩家回合";
            }
        }


    }
}
