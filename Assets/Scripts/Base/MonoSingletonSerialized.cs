using System;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// ����Odin�ṩ��SerializedMonoBehaviour�ĵ���
/// ����������е������Ϊ�м䶪��һ�ι����ļ�����������һ��Build
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingletonSerialized<T> : SerializedMonoBehaviour where T : MonoSingletonSerialized<T>
{
    public static T Instance
    {
        get
        {
            OnGetInstance();
            if (isShuttingDown)
            {
                Debug.LogWarning(string.Format("����ʵ�� {0} �ѱ����٣����󷵻�null.", typeof(T)));
                return default(T);
            }
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<T>(FindObjectsInactive.Include);
                if (instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name + " + Singleton");
                    instance = gameObject.AddComponent<T>();
                    if (isCrossScene)
                    {
                        DontDestroyOnLoad(gameObject);
                    }
                }
                else if (isCrossScene)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
                instance.Init();
            }
            return instance;
        }
    }

    protected static void OnGetInstance()
    {
        isCrossScene = true;
    }
    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning(string.Format("������������ʵ�� {0} , {1}�����һ���", typeof(T), name));
            Destroy(gameObject);
            return;
        }
        instance = (this as T);
        this.Init();
    }

    protected virtual void Init()
    {
    }
    private void OnApplicationQuit()
    {
        isShuttingDown = true;
    }
    private static T instance;


    protected static bool isShuttingDown;

    [SerializeField]
    protected bool isDontDestoryOnLoad;
    protected static bool isCrossScene;
}