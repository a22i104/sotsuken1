using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float groundDrag = 5f;

    [Header("Keys")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;

    Rigidbody rb;
    bool isGrounded;
    float horizontal;
    float vertical;
    public Transform orientation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 地面チェック
        isGrounded = Physics.Raycast(transform.position, Vector3.down,
            playerHeight * 0.5f + 0.2f, whatIsGround);

        // Drag設定
        rb.drag = isGrounded ? groundDrag : 0;

        // 入力取得
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // ジャンプ
        if (isGrounded && Input.GetKey(jumpKey))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 direction = orientation.forward * vertical + orientation.right * horizontal;
        rb.AddForce(direction.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
