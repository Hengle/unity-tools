using UnityEngine;

public class MoveIt : MonoBehaviour
{
    public Vector3 rotation;

    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }
}
