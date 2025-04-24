using System;
using TicTakToe;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameSystem
{
    /// <summary>
    /// ��Ϸ�˵���UI�Ĺ�����
    /// ������һ�ι����𻵵����,ʹ��dnspy�������˲��ִ���,��������ȽϹ�
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
            timerText.text = "<color=#CC2233><size=120%>����</size></color>\n������!";
        }
        public void OnRestart()
        {
            timer.fillAmount = 1;
            timerText.text = "<size=60%><s>����</s></size>\n<color=#CC2233><size=120%>����</size></color>\n������!";
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
            Debug.Log(" 3901 ��Ϸ�����û���endmenu�ˣ�" + e);
            this.menuRoot.SetActive(true);
            this.slider.gameObject.SetActive(true);
            this.stepText.gameObject.SetActive(false);
            timer.fillAmount = 1;
            timer.color = Color.yellow;
            timerText.text = "��Ϸ����!";
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
                this.endText.text = "��Ӯ�ˣ�\r\n\r\n<size=60%>�غ�����" + StepManager.Instance.currentID.ToString() + "\r\n�Ѷȣ�" + num.ToString();
                return;
            }
            if (e == "EnemyWin")
            {
                this.endText.text = "�����ˣ�\r\n\r\n<size=60%>�غ�����" + StepManager.Instance.currentID.ToString() + "\r\n�Ѷȣ�" + num.ToString();
                return;
            }
            if (e == "Draw")
            {
                this.endText.text = "��Ȼƽ���ˣ�\r\n\r\n<size=60%>�غ�����" + StepManager.Instance.currentID.ToString() + "\r\n�Ѷȣ�" + num.ToString();
                return;
            }
            this.endText.text = "��Ȼ��Ϸ��<color=CC2233>Bug</color>�ˣ�\r\n\r\n<size=60%>�غ�����" + 114.ToString() + "\r\n�Ѷȣ�" + 514.ToString();
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
                text = "��ǰ�Ѷȣ�" + ((int)((double)aiDifficulty * 1.25)).ToString() + "/ 100%";
            }
            else
            {
                text = "��ǰ�Ѷȣ�<color=#CC2233><b><size=150%>" + (aiDifficulty + 20).ToString() + "</color></b></size>/ 100%";
            }
            this.stepText.text = string.Concat(new string[]
            {
                string.Format("ʣ��ʱ��\n{0:F1}�� / {1:F0}�� : {2:F1}%\n", num, stepDurationTime, StepManager.Instance.currentStep.stepPercent * 100f),
                "�غ�����",
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
                this.sliderText.text = "��ǰ�Ѷȣ�" + ((int)((double)num * 1.25)).ToString() + "/ 100%";
            }
            else
            {
                this.sliderText.text = "��ǰ�Ѷȣ�<color=#CC2233><b><size=150%>" + (num + 20).ToString() + "</color></b></size>/ 100%";
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
                this.timerText.text = "���ֻغ�";
                return;
            }
            if (ticType == MonoSingletonSerialized<TicTakToeManager>.Instance.playerTicType)
            {
                this.timer.color = Color.green;
                this.timerText.text = "��һغ�";
            }
        }


    }
}
