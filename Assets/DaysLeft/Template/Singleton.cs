using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance;

    /// <summary>
    /// Singleton design pattern
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                GameObject obj = new(typeof(T).Name)
                {
                    name = typeof(T).Name + "_AutoCreated"
                };
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// On awake, we initialize our instance. Make sure to call base.Awake() in override if you need awake.
    /// </summary>
    protected virtual void Awake()
    {
        _instance = this as T;
        DontDestroyOnLoad(this);
    }
}