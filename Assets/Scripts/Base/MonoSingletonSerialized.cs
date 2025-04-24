using System;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 基于Odin提供的SerializedMonoBehaviour的单例
/// 如果看起来有点怪是因为中间丢了一次工程文件，反编译自一个Build
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
                Debug.LogWarning(string.Format("单例实例 {0} 已被销毁，请求返回null.", typeof(T)));
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
            Debug.LogWarning(string.Format("发现其他单例实例 {0} , {1}，自我毁灭", typeof(T), name));
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