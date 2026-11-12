using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;
    public ParticleSystem smokeFX;

    [HeaderAttribute("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [HeaderAttribute("Jumping")]

    public float jumpPower = 10f;

    [HeaderAttribute("GroundCheck")]

    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [HeaderAttribute("Gravity")]

    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer playerSprite;


    private void Awake()
    {
        isFacingRight = true;

        if (playerSprite != null)
        {
            playerSprite.flipX = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = newVelocity;
        isGrounded();
        Gravity();
        UpdateFacingDirection();

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);

    }

    private void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            if (context.performed)
            {
                animator.SetTrigger("jump");
                smokeFX.Play();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            }
            else if (context.canceled)
            {
                animator.SetTrigger("jump");
                smokeFX.Play();
                //Light tap of jump button half the height
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }

    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }
    private void UpdateFacingDirection()
    {
        // Ignora movimentos muito pequenos do analógico.
        if (Mathf.Abs(horizontalMovement) < 0.1f)
            return;

        // Determina para qual lado o jogador está andando.
        bool shouldFaceRight = horizontalMovement > 0f;

        // Só vira quando a direção realmente muda.
        if (shouldFaceRight != isFacingRight)
        {
            Flip(shouldFaceRight);
        }
    }

    private void Flip(bool faceRight)
    {
        // Atualiza a direção atual do personagem.
        isFacingRight = faceRight;

        // Inverte apenas o sprite do personagem.
        // Não altera a escala do Player nem do AimPivot.
        if (playerSprite != null)
        {
            playerSprite.flipX = !isFacingRight;
        }
    }


}
