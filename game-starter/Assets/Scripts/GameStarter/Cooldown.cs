using UnityEngine;

namespace GameStarter
{
    public class Cooldown
    {
        public float duration;
        public bool autoUse;
        public float lastUse = float.NegativeInfinity;

        public Cooldown(float duration)
        {
            this.duration = duration;
            autoUse = true;
        }

        public Cooldown(float duration, bool autoUse)
        {
            this.duration = duration;
            this.autoUse = autoUse;
        }

        public bool CanUse()
        {
            if (Time.time > (lastUse + duration))
            {
                if (autoUse)
                {
                    Use();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Use()
        {
            lastUse = Time.time;
        }

        public void Reset()
        {
            lastUse = float.NegativeInfinity;
        }

        public float RawPercent()
        {
            return (Time.time - lastUse) / duration;
        }

        public float Percent()
        {
            return Mathf.Clamp(RawPercent(), 0, 1);
        }
    }
}