using UnityEngine;

namespace GameStarter
{
    /// <summary>
    /// This is a manager class that can be used for game objects that need to be
    /// singletons. Often used to hold game data or to control interactions
    /// across multiple game objects.
    /// </summary>
    public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
    {
        private static T instance;

        public static T Instance
        {
            get { return instance; }
        }

        public virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = FindObjectOfType<T>();
            }
        }
    }
}