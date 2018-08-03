using UnityEngine;

namespace GameStarter
{
    public class State : MonoBehaviour
    {
        public virtual void StateStart() { }
        public virtual void StateUpdate() { }
        public virtual void StateEnd() { }
    }
}