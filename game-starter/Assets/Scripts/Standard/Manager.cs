using UnityEngine;

/// <summary>
/// This is an empty manager class that can be used as a template for game
/// objects that need to be singletons or persist across multiple scenes.
/// Often used to hold game data that spans across a game or to control
/// interactions across multiple game objects.
/// </summary>
public class Manager : MonoBehaviour
{
    private static Manager _instance = null;

    public static Manager instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}