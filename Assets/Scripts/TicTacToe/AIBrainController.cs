using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TicTakToe
{
    /// <summary>
    /// AI脚本,根据难度进行随即决策或者MiniMax搜索
    /// </summary>
    public class AIBrainController : TicTakToeController
    {
        public void Init(Tic ticType, int difficulty)
        {
            this.aiDifficulty = difficulty;
            this.thisTic = ticType;
            this.enemyTic = TicTakToeManager.Instance.playerController.thisTic;
            this.aiDifficulty = TicTakToeManager.Instance.aiDifficulty;
        }

        // Token: 0x0600011A RID: 282 RVA: 0x0000AF60 File Offset: 0x00009160
        public void ActTic()
        {
            this.aiResult = this.FindByDifficulty(this.aiDifficulty);
            TicTakToeManager.Instance.ticButtons[this.aiResult.Item1, this.aiResult.Item2].thisButton.onClick.Invoke();
        }

        // Token: 0x0600011B RID: 283 RVA: 0x0000AFB4 File Offset: 0x000091B4
        (int x,int y) FindByDifficulty(int difficulty)
        {
            ValueTuple<int, int> result = new ValueTuple<int, int>(0, 0);
            int num = UnityEngine.Random.Range(0, 100);
            /*Debug.Log(string.Concat(new string[]
            {
                " 2700 决策机随机决策  ",
                num.ToString(),
                " ： ",
                difficulty.ToString(),
                " / ",
                100.ToString()
            }));*/
            if (num < difficulty)
            {
                result = this.FindBest(TicTakToeManager.Instance.board);
            }
            else
            {
                result = this.FindRandom(TicTakToeManager.Instance.board);
            }
            return result;
        }

        // Token: 0x0600011C RID: 284 RVA: 0x0000B044 File Offset: 0x00009244
        (int x, int y) FindRandom(Tic[,] board)
        {
            List<ValueTuple<int, int>> list = new List<ValueTuple<int, int>>();
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (board[i, j] == Tic.Empty)
                    {
                        list.Add(new ValueTuple<int, int>(i, j));
                    }
                }
            }
            if (list.Count == 0)
            {
                Debug.LogError("2703 棋盘已满，随机失败");
                return new ValueTuple<int, int>(0, 0);
            }
            int index = Random.Range(0, list.Count);
            return list[index];
        }

      
        (int x, int y) FindBest(Tic[,] board)
        {
            int num = int.MinValue;
            ValueTuple<int, int> result = new ValueTuple<int, int>(0, 0);
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (board[i, j] == Tic.Empty)
                    {
                        board[i, j] = this.thisTic;
                        int num2 = this.MiniMax(board, 0, false);
                        board[i, j] = Tic.Empty;
                        if (num2 > num)
                        {
                            result = new ValueTuple<int, int>(i, j);
                            num = num2;
                        }
                    }
                }

            }
            return result;
        }

        // Token: 0x0600011E RID: 286 RVA: 0x0000B124 File Offset: 0x00009324
        private int MiniMax(Tic[,] board, int depth, bool isMaximizing)
        {
            int num = this.Evaluate(board);
            if (num != 0 || TicTakToeManager.Instance.IsBoardFull(board))
            {
                return num;
            }
            if (isMaximizing)
            {
                int num2 = int.MinValue;
                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        if (board[i, j] == Tic.Empty)
                        {
                            board[i, j] = this.thisTic;
                            num2 = Mathf.Max(num2, this.MiniMax(board, depth + 1, false));
                            board[i, j] = Tic.Empty;
                        }
                    }
                }
                return num2;
            }
            int num3 = int.MaxValue;
            for (int k = 1; k <= 3; k++)
            {
                for (int l = 1; l <= 3; l++)
                {
                    if (board[k, l] == Tic.Empty)
                    {
                        board[k, l] = this.enemyTic;
                        num3 = Mathf.Min(num3, this.MiniMax(board, depth + 1, true));
                        board[k, l] = Tic.Empty;
                    }
                }
            }
            return num3;
        }

        // Token: 0x0600011F RID: 287 RVA: 0x0000B208 File Offset: 0x00009408
        private int Evaluate(Tic[,] board)
        {
            int result = 0;
            for (int i = 1; i <= 3; i++)
            {
                if (board[i, 1] == board[i, 2] && board[i, 2] == board[i, 3])
                {
                    if (board[i, 1] == this.thisTic)
                    {
                        result = 10;
                    }
                    if (board[i, 1] == this.enemyTic)
                    {
                        result = -10;
                    }
                }
                if (board[1, i] == board[2, i] && board[2, i] == board[3, i])
                {
                    if (board[1, i] == this.thisTic)
                    {
                        result = 10;
                    }
                    if (board[1, i] == this.enemyTic)
                    {
                        result = -10;
                    }
                }
            }
            if (board[1, 1] == board[2, 2] && board[2, 2] == board[3, 3])
            {
                if (board[1, 1] == this.thisTic)
                {
                    result = 10;
                }
                if (board[1, 1] == this.enemyTic)
                {
                    result = -10;
                }
            }
            if (board[1, 3] == board[2, 2] && board[2, 2] == board[3, 1])
            {
                if (board[1, 3] == this.thisTic)
                {
                    result = 10;
                }
                if (board[1, 3] == this.enemyTic)
                {
                    result = -10;
                }
            }
            return result;
        }

        public Tic enemyTic;
        public int aiDifficulty;
        (int x, int y) aiResult;
    }
}
