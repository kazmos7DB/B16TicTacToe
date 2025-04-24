using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TicTakToe
{

    /// <summary>
    /// 井字棋游戏的管理器，负责游戏规则的实现，胜利、平局检查、棋盘管理等
    /// 中间出事故丢了一次工程文件，dnSpy反编译自一个build
    /// 可能看起来有点怪
    /// </summary>
    public class TicTakToeManager : MonoSingletonSerialized<TicTakToeManager>
    {
        private void Start()
        {
  
            this.Init();
        }

        // Token: 0x06000128 RID: 296 RVA: 0x00003FC4 File Offset: 0x000021C4
        protected override void Init()
        {
            base.Init();
        }

        // Token: 0x06000129 RID: 297 RVA: 0x00003FCC File Offset: 0x000021CC
        private void Update()
        {
            this.isWin = (this.CheckWin(Tic.X, out this.isPlayerWin) || this.CheckWin(Tic.O, out this.isPlayerWin));
            this.isDraw = this.IsBoardFull(this.board);
        }

        // Token: 0x0600012A RID: 298 RVA: 0x0000B354 File Offset: 0x00009554
        public void SetPlayerTic()
        {
            if (this.isPlayerFirst)
            {
                this.playerTicType = Tic.X;
            }
            else
            {
                this.playerTicType = Tic.O;
            }
            this.playerController.thisTic = this.playerTicType;
            if (this.playerTicType == Tic.O)
            {
                this.enemyTicType = Tic.X;
            }
            else if (this.playerTicType == Tic.X)
            {
                this.enemyTicType = Tic.O;
            }
            this.aiController.Init(this.enemyTicType, this.aiDifficulty);
        }

        // Token: 0x0600012B RID: 299 RVA: 0x0000B3C4 File Offset: 0x000095C4
        public void ChangeAllButton(bool isPlayer)
        {
            TicTakToeButton[,] array = this.ticButtons;
            int upperBound = array.GetUpperBound(0);
            int upperBound2 = array.GetUpperBound(1);
            for (int i = array.GetLowerBound(0); i <= upperBound; i++)
            {
                for (int j = array.GetLowerBound(1); j <= upperBound2; j++)
                {
                    TicTakToeButton ticTakToeButton = array[i, j];
                    if (ticTakToeButton != null)
                    {
                        ticTakToeButton.thisButton.interactable = isPlayer;
                        if (this.board[ticTakToeButton.buttonX, ticTakToeButton.buttonY] == Tic.X)
                        {
                            ticTakToeButton.thisButton.interactable = false;
                            ticTakToeButton.thisButton.image.color = Color.green;
                        }
                        else if (this.board[ticTakToeButton.buttonX, ticTakToeButton.buttonY] == Tic.O)
                        {
                            ticTakToeButton.thisButton.interactable = false;
                            ticTakToeButton.thisButton.image.color = Color.blue;
                        }
                        else if (this.board[ticTakToeButton.buttonX, ticTakToeButton.buttonY] == Tic.Empty)
                        {
                            ticTakToeButton.thisButton.interactable = isPlayer;
                            ticTakToeButton.thisButton.image.color = Color.clear;
                        }
                        else
                        {
                            ticTakToeButton.thisButton.interactable = isPlayer;
                            ticTakToeButton.thisButton.image.color = Color.yellow;
                        }
                    }
                }
            }
        }

        // Token: 0x0600012C RID: 300 RVA: 0x00004005 File Offset: 0x00002205
        public void SetPlayerFirst()
        {
            this.isPlayerFirst = !this.isPlayerFirst;
            this.SetPlayerTic();
        }

        // Token: 0x0600012D RID: 301 RVA: 0x0000401C File Offset: 0x0000221C
        public void SetDifficulty(int difficulty)
        {
            this.aiDifficulty = difficulty;
        }

        // Token: 0x0600012E RID: 302 RVA: 0x0000B52C File Offset: 0x0000972C
        public bool IsBoardFull(Tic[,] mboard)
        {
            bool result = true;
            for (int i = 1; i <= 3; i++)
            {
                int num = 1;
                if (num <= 3 && mboard[i, num] == Tic.Empty)
                {
                    result = false;
                }
            }
            return result;
        }
        public bool CheckDraw(int step)
        {
            bool result = false;
            if (step == 9 && isWin == false)
            {
                result = true;
            }

            return result;
        }

        public bool CheckWin(Tic ticType, out bool isPlayerWin)
        {
            bool flag = false;
            for (int i = 1; i <= 3; i++)
            {
                if (this.board[i, 1] == ticType && this.board[i, 2] == ticType && this.board[i, 3] == ticType)
                {
                    flag = true;
                    break;
                }
                if (this.board[1, i] == ticType && this.board[2, i] == ticType && this.board[3, i] == ticType)
                {
                    flag = true;
                    break;
                }
            }
            if (this.board[1, 1] == ticType && this.board[2, 2] == ticType && this.board[3, 3] == ticType)
            {
                flag = true;
            }
            if (this.board[1, 3] == ticType && this.board[2, 2] == ticType && this.board[3, 1] == ticType)
            {
                flag = true;
            }
            if (!flag)
            {
                isPlayerWin = false;
            }
            else if (ticType == this.playerTicType)
            {
                isPlayerWin = true;
            }
            else
            {
                isPlayerWin = false;
            }
            return flag;
        }


        public void ChangeBoard(int x, int y, Tic ticType)
        {
            this.board[x, y] = ticType;
            this.ticButtons[x, y].UpdateButton(ticType, this.GetTicSprite(ticType));
        }

        public void ClearBoard()
        {
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    this.ChangeBoard(i, j, Tic.Empty);
                }
            }
        }
        public void OnRestart()
        {
            this.isPlayerWin = false;
            this.isDraw = false;
            this.isWin = false;
            this.ClearBoard();
        }

        public Sprite GetTicSprite(Tic ticType)
        {
            Sprite result = this.errorTicSprite;
            switch (ticType)
            {
                case Tic.Empty:
                    result = this.emptyTicSprite;
                    break;
                case Tic.X:
                    result = this.ticXSprites[Random.Range(0, this.ticXSprites.Length)];
                    break;
                case Tic.O:
                    result = this.ticOSprites[Random.Range(0, this.ticOSprites.Length)];
                    break;
                default:
                    result = this.errorTicSprite;
                    break;
            }
            return result;
        }
        //玩家执子
        public Tic playerTicType;
        //ai执子
        public Tic enemyTicType;
        // ai难度,0-100
        [Range(0,100)] public int aiDifficulty;
        //玩家是否先手
        public bool isPlayerFirst;
        //棋盘
        public Tic[,] board = new Tic[4, 4];
        public TicTakToeButton[,] ticButtons = new TicTakToeButton[4, 4];
        //棋子对应的图案
        public Sprite[] ticOSprites;
        public Sprite[] ticXSprites;
        public Sprite emptyTicSprite;
        public Sprite errorTicSprite;

        public AIBrainController aiController;
        public PlayerController playerController;

        //对局是否胜利
        public bool isWin;

        //对局是否平局
        public bool isDraw;

        //玩家是否胜利
        public bool isPlayerWin;
    }
    /// <summary>
    /// 棋子种类，Empty为空，X先手，O为后手，other做缺省防止bug
    /// </summary>
    [Serializable]
    public enum Tic
    {
        Empty,
        X,
        O,
        other
    }

    /// <summary>
    /// 事件定义
    /// </summary>
    public static class TicTakToeEventHandler
    {
        public static event Action<Tic> TicOperationEvent;

        public static void CallTicOperationEvent(Tic ticType)
        {
            Debug.Log(" 0000 Step等到了输入  " + ticType.ToString());
            TicOperationEvent?.Invoke(ticType);
        }

        public static event Action ChangeStepEvent;

        public static void CallChangeStep()
        {
    
            ChangeStepEvent?.Invoke();
        }

        public static event Action<Tic> UIMenuUpdate;

        public static void CallMneuUIUpdate(Tic ticType)
        {
            UIMenuUpdate?.Invoke(ticType);
        }

        public static event Action UIMenuReset;
        public static void CallMenuReset()
        {
            UIMenuReset?.Invoke();
        }
    }

}
