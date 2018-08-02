using UnityEngine;

public class StateManager : Manager<StateManager>
{
    private State active;

    public State Active
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

    void Update()
    {
        if (active != null)
        {
            active.StateUpdate();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            active = GetComponentAtMouse3D<State>();
        }
    }

    public static T GetComponentAtMouse3D<T>()
    {
        T target = default(T);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
        {
            target = hit.transform.GetComponent<T>();
        }

        return target;
    }

    public static T GetComponentAtMouse2D<T>()
    {
        T target = default(T);

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D col = Physics2D.OverlapPoint(worldPoint);

        if (col != null)
        {
            target = col.GetComponent<T>();
        }

        return target;
    }

    public void Clear(State s)
    {
        if (active == s)
        {
            active = null;
        }
    }
}
