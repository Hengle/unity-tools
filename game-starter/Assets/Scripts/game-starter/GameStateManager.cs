using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private GameState active;

    public GameState Active
    {
        get
        {
            return active;
        }

        set
        {
            if (active != null)
            {
                active.StateEnd();
            }

            active = value;

            if (active != null)
            {
                active.StateStart();
            }
        }
    }

    private static GameStateManager instance = null;

    public static GameStateManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        if (Active != null)
        {
            Active.StateUpdate();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Active = GetStateAtMouse();
        }
    }

    public static GameState GetStateAtMouse()
    {
        GameState target = null;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D col = Physics2D.OverlapPoint(worldPoint);

        if (col != null)
        {
            target = col.GetComponent<GameState>();
        }

        return target;
    }
}
