using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// 2D platformer player who looks ahead as to where the character is moving and
/// stop a collision (though repeat looks will need to be add to truly stop
/// all a collision).
/// Rigidbody should be set to Interpolate for smooth camera follow.
/// Physics 2D settings should have "Queries Start In Collider" turned off.
/// </summary>
public class Player : MonoBehaviour
{
    public float horizontalSpeed = 11.21794872f;
    public float maxFallSpeed = 38.8888889f;
    public float fallGravity = 76.0173754f;
    public float jumpSpeed = 22.2222223f;
    public float jumpGravity = 56.98005698f;
    public float groundCheckDist = 0.15f;

    private Rigidbody2D rb;
    private float radius;

    internal float facing = 1;
    internal bool canJump = false;
    internal bool isTouchingGround = false;
    internal Vector2 size;
    internal Vector2 rigidbodyPos
    {
        get
        {
            return rb.position;
        }
    }

    private PlatformerCamera cam;


    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = jumpGravity/Physics2D.gravity.magnitude;

        CircleCollider2D col = GetComponent<CircleCollider2D>();
        radius = Mathf.Max(transform.localScale.x, transform.localScale.y) * col.radius;

        size = new Vector2(2f * radius, 2f * radius);

        cam = Camera.main.GetComponent<PlatformerCamera>();
    }

    void Start()
    {
        StartCoroutine(checkGround());
    }

	// Update is called once per frame
    void Update ()
    {
        //cam.lookDown = (Input.GetAxisRaw("Vertical") == -1);

        updateFacing();

        tryToJump();
    }

    private void updateFacing()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        if (xAxis > 0)
        {
            facing = 1;
        }
        else if (xAxis < 0)
        {
            facing = -1;
        }
    }

    private bool isUpDown = false;

    private void tryToJump()
    {
        float yAxis = Input.GetAxisRaw("Vertical");

        if (yAxis > 0 && isUpDown == false)
        {
            if (canJump)
            {
                Vector2 vel = rb.velocity;
                vel.y += jumpSpeed;
                rb.velocity = vel;
            }

            isUpDown = true;
        }

        if (yAxis <= 0)
        {
            isUpDown = false;
        }
    }

	void FixedUpdate ()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        Vector2 vel = rb.velocity;
        vel.x = xAxis * horizontalSpeed;

        RaycastHit2D hit;
        if (circleCast(rb.position, radius, vel.normalized, vel.magnitude * Time.fixedDeltaTime, out hit))
        {
            /* Move up to whatever we hit */
            rb.position = hit.centroid;

            /* Project our velocity against what we hit so we don't move it. Obviously 
             * we'd need to keep checking the projected velocity if we really care to
             * avoid moving stuff. */
            Vector2 slope;
            slope.x = -hit.normal.y;
            slope.y = hit.normal.x;
            vel = Vector3.Project(vel, slope);
        }

        rb.velocity = vel;

        //Debug.Log(rb.velocity.ToString("f4"));

        Debug.DrawLine(rb.position, rb.position + rb.velocity.normalized, Color.red, 0f, false);
	}

    /* I am updating our ground check bools in WaitForFixedUpdate(), since it will run
     * right after the physics time step (even though WaitForFixedUpdate() is supposed
     * to run right before the time step (another Unity bug :/)). So we have the updated
     * physics location before we check for the ground. Before I had been doing the
     * ground check in FixedUpdate(), but on the first physics frame that jump velocity
     * is applied the player is still on the ground until the internal physics runs so
     * our ground check will still be true. And if a graphics frame is run at this point
     * it will see the char as being slightly above the ground but with grounded bools
     * which are still true. */
    private IEnumerator checkGround()
    {
        yield return new WaitForFixedUpdate();

        RaycastHit2D hit;
        canJump = circleCast(rb.position, radius, Vector2.down, groundCheckDist, out hit);
        /* It seems that any colliders which are less than Physics2D.minPenetrationForPenalty
         * apart are considered to be touching. So any distance less than
         * minPenetrationForPenalty means that we can jump.
         *
         * Though sometimes I see a collider come to rest slightly more than minPenetrationForPenalty
         * distance away from another collider so maybe I'm wrong with the above assumption. Thats
         * why I'm checking for 150% of minPenetrationForPenalty.*/
        isTouchingGround = canJump && hit.distance <= 1.5f * Physics2D.minPenetrationForPenalty;

        StartCoroutine(checkGround());
    }

    private bool circleCast(Vector2 pos, float radius, Vector2 dir, float dist, out RaycastHit2D hit)
    {
        /* Make sure we don't count collider that we start in */
        bool origQueriesStartInColliders = Physics2D.queriesStartInColliders;
        Physics2D.queriesStartInColliders = false;

        /* Move back by min penetration to hit any colliders that we are already touching */
        Vector2 origin = pos - Physics2D.minPenetrationForPenalty * dir;
        float maxDist = Physics2D.minPenetrationForPenalty + dist;

        hit = Physics2D.CircleCast(origin, radius, dir, maxDist);

        Physics2D.queriesStartInColliders = origQueriesStartInColliders;

        if (hit.collider != null)
        {
            /* Remove the min penetration from the distance. */
            hit.distance -= Physics2D.minPenetrationForPenalty;
            return true;
        }
        else
        {
            return false;
        }
    }
}
