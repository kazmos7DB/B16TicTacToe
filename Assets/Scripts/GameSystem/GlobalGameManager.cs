using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace GameSystem
{
    /// <summary>
    /// ȫ����Ϸ������
    /// </summary>
    public class GlobalGameManager : MonoSingleton<GlobalGameManager>
    {
        private void Start()
        {
            //��ʼ���ֱ��ʺ�֡��
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
    /// ��Ϸ״̬��û����
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