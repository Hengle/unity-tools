using UnityEngine;

/*
    How To Use for Optimal Results:
        * Set up steps (to match what my platformer 2d project does)
            * Rigidbody2D
                * Frictionless Material
                * Continuous
                * Interpolate
                * Freeze Rotation
            * Platformer Char 2D
                * Call PlatformerChar2D.move() every FixedUpdate() from the main character script to make sure the game object moves like a platformer character (even if you just calling move() with a value of 0).

        * Default values were based on:
            * A character with a circle radius of 8 (height/width of 16) units
            * Gravity of -480
            * The rest of the default values on class propreties.

        * Probably should leave the touchingDist and deltaRadius alone and just make sure they still work for the characters radius and for a relatively steep slope angle. (The larger the character radius is the easier it is for it to be true).
        * Give the character a frictionless physics material
        * Make the character continuous collision detection to avoid bouncing along the wall as it falls
        * Make sure the bottom of the sprite for the character extends past the bottom of the circle collider
        by about touchingDist. This is because the character can be between 0 and touchingDist away from
        the ground so an slight overlap with the ground looks preferable to a slight hover of the character.
        * Work with units greater than 1. Meaning work with circle colliders that are have a radius of 3 not 0.3. (Even better make the circle collider have a radius of greater than 10). This helps with the below touchingDist equation.
        * TODO describe what touchingDist value should be (approximately)
        Warning I haven't proven the following calculation, but it seems right.
        If I select a maxTouchingDist, theta, r, or (R-r) value that does not meet the below formula then you should see the character slide down due to gravity on a steep slope since the smaller ground check circle will land farther away than maxTouchingDist from the circle collider center and thus leave gravity on.

        maxTouchingDist approx = sqrt(r^2 +((R-r) / sin(theta)) - r
        where theta is the angle between a slope and the y axis (the closer theta is to zero the closer the slope is to being vertical)

        So obviously maxTouchingDist grows to infinity the closer it theta gets to zero, but if we assume a worse case theta of 20 degree (or some other steep value) then we can find pretty decent values for R-r and maxTouchingDist.
        So bigger r values and bigger (less steep) theta values help a lot.

    Features:
        Consider allowing for the same logic to be applied as velocity or a force (for percise or accelerated movement)

    Fixed or Mitigated Problems:
        Imperfect following of curve when reaching the top of a hill
            Seems to be migitaged by heavy gravity (which you usually want)

        Moving into wall when falling will stop you from falling
            Make the character frictionless
            Make the character continuous collision detection to avoid (or at least reduce) bouncing along the wall as it falls

        Weird change in motion going from flat to slope
            For some reason a circle cast with a smaller circle than the circle collider causes better surface normal results (not sure why unity physics 2d likes that).

        Too high from the ground
            Added a touching distance that can be pretty small (but not too small) and a lot smaller than the ground follow dist.
            Also make sure your circle collider is slightly smaller than your sprite so the sprite is slightly overlaping the ground rather than hovering slightly above it.

        Falling onto a slope will cause you to land and bounce to the side a bit due to collision resolution
            I would need to watch where I'm going and prevent collision resolution from occuring to prevent this
                Also need to make sure I only prevent if the slope can be considered ground
*/

public class PlatformerChar2D : MonoBehaviour
{
    public float groundCheckDist = 4f;

    public LayerMask mask = Physics2D.DefaultRaycastLayers;

    [Tooltip("How far the circle collider can be from the ground check point and still be considered touching the ground. This number cannot be too small and must take into account that the ground check circle is smaller than the actual circe collider so the hit point will not necessary be touching the real circle collider.")]
    public float touchingDist = 0.1f;

    [Tooltip("How much smaller the ground check circle radius is than the actual circle collider radius. (This helps release changes in ground normal faster for some reason.)")]
    public float deltaRadius = 0.25f;

    private Vector2 groundNormal = Vector2.zero;

    private bool isJumping = false;

    private Rigidbody2D rb;

    private CircleCollider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        col = GetComponent<CircleCollider2D>();
    }

    public void move(float horizontalVelocity)
    {
        rb.gravityScale = 1;

        Vector2 vel = rb.velocity;
        vel.x = horizontalVelocity;

        RaycastHit2D groundCheckHit = circleCast(colPos, col.radius - deltaRadius, Vector2.down, groundCheckDist, mask);

        groundNormal = groundCheckHit.normal;

        if (groundNormal != Vector2.zero)
        {
            /* If we see ground and we are moving down then stop jumping. Or if we see ground and we 
               are moving into it (like a steep wall) then stop jumping. */
            if (rb.velocity.y <= 0 || Vector2.Angle(groundNormal, vel) >= 90f)
            {
                isJumping = false;
            }

            float distToGround = Vector2.Distance(groundCheckHit.point, rb.position) - col.radius;

            if (!isJumping)
            {
                if (distToGround <= touchingDist)
                {
                    rb.gravityScale = 0;
                }

                float y = vel.y;

                Vector2 slope = new Vector2(-groundNormal.y, groundNormal.x);
                vel.y = 0;
                vel = Vector3.Project(vel, slope).normalized * Mathf.Abs(horizontalVelocity);

                if (distToGround > touchingDist)
                {
                    vel.y = Mathf.Min(y, vel.y);
                }

                Debug.DrawLine(colPos, colPos + groundNormal * col.radius * 2f, Color.red, 0f, false);
            }
        }

        rb.velocity = vel;

        RaycastHit2D lookAheadHit = circleCast(colPos, col.radius, rb.velocity.normalized, rb.velocity.magnitude * Time.deltaTime, mask);

        /* If we are going to collider and the normal can be considered ground and we aren't 
         * already too close to it, then move up to collider now and clear the velocity 
         * (using MovePosition() in this case) to prevent the physics collision from displacing
         * the character. */
        if (lookAheadHit.collider != null
            && Vector2.Angle(Vector2.up, lookAheadHit.normal) < 90
            && lookAheadHit.distance > (Physics2D.defaultContactOffset * 2f))
        {
            /* Move up to what we are about to move into with MovePosition. Rigidbody2D.MovePosition() 
             * will calculate the velocity needed to move the object to the position in the next physics 
             * update (neither gravity or linear drag will affect the body). So no need to clear or 
             * project the rigidbody velocity since it is already being overriden by MovePosition(). 
             * Rigidbody 3D I assume does not work this way. */
            rb.MovePosition(lookAheadHit.centroid);
        }
    }

    public void jump(float speed)
    {
        if (!isJumping && groundNormal != Vector2.zero)
        {
            rb.gravityScale = 1;
            isJumping = true;

            Vector3 vel = rb.velocity;
            vel.y = speed;
            rb.velocity = vel;
        }
    }

    /// <summary>
    /// Gets the position of the collider (which can be offset from the transform position).
    /// </summary>
    private Vector2 colPos
    {
        get
        {
            return transform.TransformPoint(col.offset) + (Vector3)rb.position - transform.position;
        }
    }

    private RaycastHit2D circleCast(Vector2 position, float radius, Vector2 direction, float distance, LayerMask mask)
    {
        RaycastHit2D hit;

        bool origQueriesStartInColliders = Physics2D.queriesStartInColliders;

        Physics2D.queriesStartInColliders = false;

        hit = Physics2D.CircleCast(position, radius, direction, distance, mask);

        Physics2D.queriesStartInColliders = origQueriesStartInColliders;

        return hit;
    }

}