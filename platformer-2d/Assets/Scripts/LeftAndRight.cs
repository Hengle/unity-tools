using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftAndRight : MonoBehaviour
{
    public float horizontalSpeed = 100f;

    public float patrolDist = 96f;

    private PlatformerChar2D character;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<PlatformerChar2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float t = patrolDist / horizontalSpeed;
        float dir = -1 * Mathf.Sign((Time.time % (t * 2f)) - t);
        character.move(dir * horizontalSpeed);
    }
}
