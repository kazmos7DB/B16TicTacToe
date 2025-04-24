using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace GameSystem
{
    /// <summary>
    /// 全局游戏管理器
    /// </summary>
    public class GlobalGameManager : MonoSingleton<GlobalGameManager>
    {
        private void Start()
        {
            //初始化分辨率和帧数
            Screen.SetResolution(1920, 1080, true);
            if (!Screen.fullScreen)
            {
                Screen.fullScreen = true;
            }
            Application.targetFrameRate = 62;
            
            OnGameInitial?.Invoke();



        }
        public UnityEvent OnGameInitial = new UnityEvent();

        public UnityEvent OnGameStarted = new UnityEvent();

        public UnityEvent<string> OnGameEnd = new UnityEvent<string>();

        public LevelState currentState;
    }
    /// <summary>
    /// 游戏状态，没用上
    /// </summary>
    public enum LevelState
    {
        InLevel,
        Start,
        Loading,
        MainMenu,
        Pause,
        Exiting,
        Other
    }
}