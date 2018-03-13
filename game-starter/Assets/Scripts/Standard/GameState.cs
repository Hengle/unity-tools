using UnityEngine;

public class GameState : MonoBehaviour
{
    public virtual void StateStart() { }
    public virtual void StateUpdate() { }
    public virtual void StateEnd() { }
}
