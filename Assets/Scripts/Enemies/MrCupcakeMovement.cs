using UnityEngine;

public class MrCupcakeMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float raycastDistance = 1.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask objectLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Movimiento hacia la izquierda
        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);

        EnemyJump();
    }

    private void EnemyJump()
    {
        // Combinar las capas del suelo y objetos
        LayerMask groundMask = groundLayer | objectLayer;

        // Comprobar si hay suelo debajo del enemigo
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            raycastDistance,
            groundMask
        );

        isGrounded = hit.collider != null;

        // Debug: verde = tocando suelo, rojo = en el aire
        Debug.DrawRay(
            transform.position,
            Vector2.down * raycastDistance,
            isGrounded ? Color.green : Color.red
        );

        // Si está en el suelo, saltar automáticamente
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }


}