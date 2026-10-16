using UnityEngine;

public class MrCupcakeMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpCooldown = 0.3f;
    [SerializeField] private float groundCheckDepth = 0.08f; // How far below the feet the ground is checked
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask objectLayer;

    private Rigidbody2D rb;
    private Collider2D col;
    private bool isGrounded;
    private float nextJumpTime;

    private readonly Collider2D[] results = new Collider2D[8];
    private ContactFilter2D filter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Only detect solid colliders on the ground and object layers
        filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(groundLayer | objectLayer);
    }

    private void FixedUpdate()
    {
        CheckGround();

        // Horizontal movement (vertical velocity is preserved)
        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);

        // Jump only if standing on something, not already rising, and the cooldown has passed
        if (isGrounded && rb.linearVelocity.y <= 0.1f && Time.time >= nextJumpTime)
        {
            // Set the vertical velocity instead of adding to it, so every jump is identical
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            nextJumpTime = Time.time + jumpCooldown;
        }
    }

    private void CheckGround()
    {
        Bounds b = col.bounds;

        // Thin box right below the feet, slightly narrower than the collider
        Vector2 center = new Vector2(b.center.x, b.min.y - groundCheckDepth * 0.5f);
        Vector2 size = new Vector2(b.size.x * 0.8f, groundCheckDepth);

        int count = Physics2D.OverlapBox(center, size, 0f, filter, results);

        isGrounded = false;
        for (int i = 0; i < count; i++)
        {
            if (results[i] != col) // Ignore its own collider
            {
                isGrounded = true;
                break;
            }
        }
    }

    // Draws the ground check box when the object is selected (green = grounded, red = in the air)
    private void OnDrawGizmosSelected()
    {
        Collider2D c = col != null ? col : GetComponent<Collider2D>();
        if (c == null) return;

        Bounds b = c.bounds;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(
            new Vector3(b.center.x, b.min.y - groundCheckDepth * 0.5f, 0),
            new Vector3(b.size.x * 0.8f, groundCheckDepth, 0));
    }
}