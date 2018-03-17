using UnityEngine;

public class StateManager : MonoBehaviour
{
    private State _active;

    public State active
    {
        get
        {
            return _active;
        }

        set
        {
            if (_active != null)
            {
                _active.StateEnd();
            }

            _active = value;

            if (_active != null)
            {
                _active.StateStart();
            }
        }
    }

    private static StateManager _instance = null;

    public static StateManager instance
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
    }

    void Update()
    {
        if (active != null)
        {
            active.StateUpdate();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            active = GetStateAtMouse();
        }
    }

    public static State GetStateAtMouse()
    {
        State target = null;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D col = Physics2D.OverlapPoint(worldPoint);

        if (col != null)
        {
            target = col.GetComponent<State>();
        }

        return target;
    }
}
