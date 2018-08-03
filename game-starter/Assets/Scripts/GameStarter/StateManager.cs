using UnityEngine;

namespace GameStarter
{
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
                active = Utils.GetComponentAtMouse3D<State>();
            }
        }

        public void Clear(State s)
        {
            if (active == s)
            {
                active = null;
            }
        }
    }
}