using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T m_Instance = null;
    private static bool applicationIsQuitting = false;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("Singleton Instance '" + typeof(T) +
                                "' already destroyed on application quit." +
                                " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                // Instance requiered for the first time, we look for it
                if (m_Instance == null)
                {
                    m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                    // Object not found, we create a temporary one
                    if (m_Instance == null)
                    {
                        Debug.LogWarning("No instance of " + typeof(T).ToString() + ", a temporary one is created.");
                        m_Instance = new GameObject("Singleton of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();

                        // Problem during the creation, this should not happen
                        if (m_Instance == null)
                        {
                            Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                        }

                        DontDestroyOnLoad(m_Instance);
                    }
                }
                return m_Instance;
            }
        }
    }

    public static T Inst
    {
        get
        {
            return Instance;
        }
    }

    // If no other monobehaviour request the instance in an awake function
    // executing before this one, no need to search the object.
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
            m_Instance.Initializations();
        }
    }

	private void OnDestroy()
	{
		Debug.LogWarning("Destory instance of " + typeof(T).ToString() + ".");
		if(m_Instance != null)
			m_Instance.Destroy();
		m_Instance = null;
	}

    // This function is called when the instance is used the first time
    // Put all the initializations you need here, as you would do in Awake
    protected virtual void Initializations() { }
	protected virtual void Destroy() { }

    // Make sure the instance isn't referenced anymore when the user quit, just in case.
    private void OnApplicationQuit()
    {
	///	if (m_Instance != null)
///			m_Instance.Destroy();
  ///      m_Instance = null;
        applicationIsQuitting = true;
    }
}
