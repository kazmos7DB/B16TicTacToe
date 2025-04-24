using System;
using UnityEngine;

/// <summary>
/// ����MonoBehaviour�ĵ���
/// ����������е��,����Ϊ�м䶪��һ�ι����ļ���dnSpy��������һ��Build
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;


    protected static bool isShuttingDown;
    protected static bool isCrossScene;
    [SerializeField]
    protected bool isDontDestoryOnLoad;
    public static T Instance
    {
        get
        {
            OnGetInstance();
            if (isShuttingDown)
            {
                Debug.LogWarning(string.Format("����ʵ��{0}�ѱ����٣����󷵻�null.", typeof(T)));
                return default(T);
            }
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);
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
        isCrossScene = this.isDontDestoryOnLoad;
        if (instance != null && instance != this)
        {
            Debug.LogWarning(string.Format("������������ʵ�� {0} , {1}�����һ���", typeof(T), base.name));
            Destroy(base.gameObject);
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




}
