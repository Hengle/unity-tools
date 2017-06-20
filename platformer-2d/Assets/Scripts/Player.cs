using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float horizontalSpeed = 100f;

    public float jumpSpeed = 250f;

    private PlatformerChar2D character;

    void Awake()
    {
        character = GetComponent<PlatformerChar2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            character.jump(jumpSpeed);
        }
    }

    void FixedUpdate()
    {
        character.move(Input.GetAxisRaw("Horizontal") * horizontalSpeed);
    }
}
