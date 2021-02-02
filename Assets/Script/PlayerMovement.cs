using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float climbSpeed;
    public float jumpforce;

    private bool isJumping;
    private bool isGrounded;
    public bool isClimbing;[HideInInspector]

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionsPlayers;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer SpriteRenderer;
    public CapsuleCollider2D PlayerCollider;

    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement;
    private float VerticalMovement;

    public static PlayerMovement instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène");
            return;
        }

        instance = this;
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        VerticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing)
        {
            isJumping = true;
        }

        Flip(rb.velocity.x);

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
        animator.SetBool("isClimbing", isClimbing);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionsPlayers);
        MovePlayer(horizontalMovement, VerticalMovement);
    }

    void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        if (!isClimbing)
        {
            Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

            if (isJumping)
            {
                rb.AddForce(new Vector2(0f, jumpforce));
                isJumping = false;
            }
        } else
        {
           
            Vector3 targetVelocity = new Vector2(0, _verticalMovement);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
        }
        
    }

    void Flip(float _velocity)
    {
        if(_velocity > 0.1f)
        {
            SpriteRenderer.flipX = false;
        }else if(_velocity < -0.1f)
        {
            SpriteRenderer.flipX = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
