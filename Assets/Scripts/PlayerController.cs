using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float normalSpeed;
    public float crouchSpeed;
    public float slideSpeed;
    public Transform sprite;
    public LayerMask groundLayers;
    public float jumpForce;
    public Animator animator;

    private bool facingRight = true;
    private bool grounded = false;
    private bool wallLeft = false;
    private bool wallRight = false;
    private bool canStand = false;

    private bool isCrouching = false;
    private bool isJumpPressed = false;
    private bool doJump = false;
    private bool isSliding = false;

    void Start() {
    }

    void FixedUpdate() {
        Vector2 pos = rigidbody2D.position;
        float halfHeight = 0.9f;// rigidbody2D.collider2D.bounds.size.y / 2;
        float halfWidth = 0.5f;// rigidbody2D.collider2D.bounds.size.x / 2;
        grounded = Physics2D.OverlapArea(pos + new Vector2(-(halfWidth-0.01f), -halfHeight/2), pos + new Vector2(halfWidth-0.01f, -halfHeight-0.01f), groundLayers);
        wallLeft = Physics2D.OverlapArea(pos + new Vector2(-(halfWidth + 0.01f), halfHeight/2), pos + new Vector2(-halfWidth/2, 0), groundLayers);
        wallRight = Physics2D.OverlapArea(pos + new Vector2((halfWidth + 0.01f), halfHeight/2), pos + new Vector2(halfWidth / 2, 0), groundLayers);
        canStand = (Physics2D.OverlapArea(pos + new Vector2(-(halfWidth - 0.01f), 0), pos + new Vector2((halfWidth - 0.01f), halfHeight + 0.01f), groundLayers) == null);

        CheckForHorizontal();
        CheckForJump();
        if (grounded) {
            CheckForCrouch();
        }
        if (isSliding) {
            Vector2 vel = rigidbody2D.velocity;
            vel.y = Mathf.Clamp(vel.y, -slideSpeed, slideSpeed);
            rigidbody2D.velocity = vel;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            doJump = true;
        }
        animator.SetBool("Crouch", isCrouching);
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Moving", rigidbody2D.velocity.x != 0);
        animator.SetFloat("VelocityY", rigidbody2D.velocity.y);
        animator.SetBool("Sliding", isSliding);

    }

    void CheckForCrouch() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            isCrouching = true;
        } else if (canStand) {
            isCrouching = false;
        }
    }


    void CheckForJump() {
        if (doJump) {
            doJump = false;
            if ((grounded || isSliding) && canStand) {
                rigidbody2D.AddForce(new Vector2(0, jumpForce));
                if (isSliding) {
                    rigidbody2D.AddForce(new Vector2((facingRight ? -1 : 1) * jumpForce / 2, 0));
                }
                isJumpPressed = true;
                grounded = false;
            }
        }

        Vector2 vel = rigidbody2D.velocity;
        if (!Input.GetKey(KeyCode.Space) && isJumpPressed && vel.y > 0) {
            vel.y *= 0.4f;
            rigidbody2D.velocity = vel;
            isJumpPressed = false;
        }
    }

    void CheckForHorizontal() {
        float horizontal = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) {
            horizontal = -1;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            horizontal = 1;
        }

        bool isJumpingTowardsWall = !grounded && ((wallLeft && horizontal == -1) || (wallRight && horizontal == 1));
        Vector2 vel = rigidbody2D.velocity;
        if (vel.y < 0 && isJumpingTowardsWall) {
            isSliding = true;
        } else {
            isSliding = false;
        }

        if (horizontal != 0 && !isJumpingTowardsWall) {
            rigidbody2D.AddForce(new Vector2(horizontal * 75, 0));
            float maxSpeed = isCrouching ? crouchSpeed : normalSpeed;
            vel.x = Mathf.Clamp(vel.x, -maxSpeed, maxSpeed);
            rigidbody2D.velocity = vel;

            if ((horizontal > 0 && !facingRight) ||
                (horizontal < 0 && facingRight)) {
                Flip();
            }
        }
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
