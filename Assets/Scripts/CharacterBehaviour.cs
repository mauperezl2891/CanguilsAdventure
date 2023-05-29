using UnityEngine;
using UnityEngine.Events;

public class CharacterBehaviour : MonoBehaviour
{
    public Sprite invertedSprite;

    private float horizontal;
    public float speed = 4f;
    private float jumpingForce = 9f;
    public bool isInverted = false;
    public Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public bool invertHorizontal = false;
    private float invertedHorizontal = 1f;
    private Sprite originalSprite;
    private bool isGoingBackwards = false;
    private bool isGrounded = false;
    private SpriteRenderer spriteRenderer;
    EventController eventController;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

    }

    private void Start()
    {
        eventController = FindObjectOfType<EventController>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        bool wasGrounded = isGrounded;
        isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, .2f, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    private void Update()
    {
        invertedHorizontal = invertHorizontal ? -1f : 1f;

        horizontal = invertedHorizontal * Input.GetAxisRaw("Horizontal");

        animator.SetFloat("speed", Mathf.Abs(horizontal));
        animator.SetBool("inverted", isInverted);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpingForce);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            horizontal = 0;
            InvertControllers();
        }

        if (horizontal < 0f) //white canguil goes to the right
        {
            isGoingBackwards = true;
        }
        else
        {
            isGoingBackwards = false;
        }
        animator.SetBool("back", isGoingBackwards);

        if (!IsGrounded() && Input.GetKeyDown(KeyCode.V))
        {
            horizontal = 0;
        }

    }

    public void InvertControllers()
    {
        invertHorizontal = !invertHorizontal;
        isInverted = !isInverted;

        if (isInverted)
        {
            spriteRenderer.sprite = invertedSprite;
        }
        else
        {
            spriteRenderer.sprite = originalSprite;
        }

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (eventController != null)
        {
            eventController.CharacterEnteredCollider(collision);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (eventController != null)
        {
            eventController.CharacterEnteredCollider(collision);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (eventController != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Info"))
            {
                eventController.HideInfo();
            }
        }
    }
}
