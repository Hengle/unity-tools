using UnityEngine;

namespace GameStarter
{

    /*  Future Improvements
     *    Make a 3D version or make this one class work for 2D or 3D with a toggle.
     *    
     *    Use Trauma instead of a linear shake amount
     *      trauma ranges from 0 to 1
     *      damage adds trauma (+= 0.2 or 0.5)
     *      trauma decreases over time linearly
     *      cameraShake = trauma ^ 2 (or ^ 3)
     *    
     *    Shake In 2D
     *      angle = maxAngle * shake * GetRandomFloatNegOneToOne();
     *      offsetX = maxOffset * shake * GetRandomFloatNegOneToOne();
     *      offsetY = maxOffset * shake * GetRandomFloatNegOneToOne();
     *  
     *      shakyCamera.angle = camera.angle + angle;
     *      shakyCamera.center = camera.center + Vec2(offsetX, offsetY);
     *    
     *    Shake in 3D (no translational shake!)
     *      yaw = maxYaw * shake * GetRandomFloatNegOneToOne();
     *      pitch = maxPitch * shake * GetRandomFloatNegOneToOne();
     *      roll = maxRoll * shake * GetRandomFloatNegOneToOne();
     *  
     *    Use Perlin Noise instead of GetRandomFloatNegOneToOne (not sure if I agree with this)
     *
     *  Math for Game Programmers: Juicing Your Cameras With Math - https://www.youtube.com/watch?v=tu-Qe66AvtY
     */
    public class CameraFollow2D : MonoBehaviour
    {
        public float followPercent = 0.1f;

        float shakeAmount;
        float shakeEnd = Mathf.NegativeInfinity;

        Transform player;

        void Start()
        {
            player = GameObject.Find("Player").transform;
        }

        void Update()
        {
            Vector3 offset = Vector3.zero;

            if (shakeEnd >= Time.time)
            {
                offset = Random.insideUnitSphere * shakeAmount;
            }

            Vector3 pos = transform.position;
            pos.x += ((player.position.x - pos.x) * followPercent) + offset[0];
            pos.y += ((player.position.y - pos.y) * followPercent) + offset[1];
            transform.position = pos;
        }

        public void Shake()
        {
            Shake(0.7f, 0.5f);
        }

        public void Shake(float intensity, float dur)
        {
            shakeAmount = intensity;
            shakeEnd = Time.time + dur;
        }
    }
}