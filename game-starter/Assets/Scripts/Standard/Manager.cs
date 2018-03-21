using UnityEngine;

/// <summary>
/// This is a manager class that can be used for game objects that need to be
/// singletons. Often used to hold game data or to control interactions
/// across multiple game objects.
/// </summary>
public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
{
    private static T _instance = null;

    public static T instance
    {
        get { return _instance; }
    }

    public virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = FindObjectOfType<T>();
        }
    }
}