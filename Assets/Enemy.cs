
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float chaseSpeed = 2f;
    public float jumpForce = 5f;

    [Header("Detection")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 1f;
    public float obstacleCheckDistance = 1f;
    public float gapCheckDistance = 2f;

    [Header("Sprite")]
    public SpriteRenderer spriteRenderer;
    public bool isFacingRight = true;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    private float direction;


    private void Start()
    {
        // Obtém o Rigidbody2D do próprio inimigo.
        rb = GetComponent<Rigidbody2D>();

        // Verifica se o Player ainda não foi atribuído.
        if (player == null)
        {
            // Procura um objeto com a Tag "Player".
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            // Se encontrou o jogador, guarda seu Transform.
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Player não encontrado na cena!"
                );
            }
        }
    }


    private void Update()
    {
        if (player == null)
            return;

        float distanceX =
            player.position.x - transform.position.x;

        direction = Mathf.Abs(distanceX) > 0.1f
            ? Mathf.Sign(distanceX)
            : 0f;

        Flip();

        isGrounded = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        if (!isGrounded || direction == 0f)
            return;

        // Detecta obstáculos na frente
        RaycastHit2D obstacleAhead = Physics2D.Raycast(
            transform.position,
            Vector2.right * direction,
            obstacleCheckDistance,
            groundLayer
        );

        // Detecta se existe chão à frente
        Vector2 gapOrigin = (Vector2)transform.position
            + Vector2.right * direction;

        RaycastHit2D groundAhead = Physics2D.Raycast(
            gapOrigin,
            Vector2.down,
            gapCheckDistance,
            groundLayer
        );

        // Detecta plataformas acima
        RaycastHit2D platformAbove = Physics2D.Raycast(
            transform.position,
            Vector2.up,
            3f,
            groundLayer
        );

        // Verifica se o jogador está acima
        bool playerAbove =
            player.position.y > transform.position.y + 1f;

        if (obstacleAhead.collider != null ||
            groundAhead.collider == null)
        {
            shouldJump = true;
        }
        else if (playerAbove &&
                 platformAbove.collider != null)
        {
            shouldJump = true;
        }
    }


    private void Flip()
    {
        if (direction < 0f && !isFacingRight)
        {
            isFacingRight = true;
            spriteRenderer.flipX = false;
        }
        else if (direction > 0f && isFacingRight)
        {
            isFacingRight = false;
            spriteRenderer.flipX = true;
        }
    }


    private void FixedUpdate()
    {
        if (player == null)
            return;

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(
                direction * chaseSpeed,
                rb.linearVelocity.y
            );
        }

        if (isGrounded && shouldJump)
        {
            shouldJump = false;

            rb.AddForce(
                new Vector2(
                    direction * jumpForce * 0.5f,
                    jumpForce
                ),
                ForceMode2D.Impulse
            );
        }
    }
}
