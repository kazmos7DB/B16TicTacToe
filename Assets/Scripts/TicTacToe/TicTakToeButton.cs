using System;
using GameSystem;
using UnityEngine;
using UnityEngine.UI;

namespace TicTakToe
{
/// <summary>
/// 棋盘按钮的控制脚本
/// </summary>
    [RequireComponent(typeof(Button))]
    public class TicTakToeButton : MonoBehaviour
    {
        public void OnTicButton()
        {
            this.thisTicState = MonoSingleton<StepManager>.Instance.currentTic;
            MonoSingletonSerialized<TicTakToeManager>.Instance.ChangeBoard(this.buttonX, this.buttonY, this.thisTicState);
            TicTakToeEventHandler.CallTicOperationEvent(this.thisTicState);
        }

        public void UpdateButton(Tic ticType, Sprite ticSprie)
        {
            this.thisTicState = ticType;
            this.thisImage.sprite = ticSprie;
        }


        public Button thisButton;

        [SerializeField]
        private Image thisImage;

        public int buttonX;

        public int buttonY;

        public Tic thisTicState;
    }
}
