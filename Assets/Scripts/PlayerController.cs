using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float normalSpeed;
    public float crouchSpeed;
    public Transform sprite;
    public LayerMask groundLayers;
    public float jumpForce;

    private bool facingRight = true;
    private bool grounded = false;
    private bool wallLeft = false;
    private bool wallRight = false;

    private bool isCrouching = false;
    private bool isJumpPressed = false;
    private bool doJump = false;

    void Start() {
    }

    void FixedUpdate() {
        Vector2 pos = rigidbody2D.position;
        grounded = Physics2D.OverlapArea(pos + new Vector2(-0.49f, -0.95f), pos + new Vector2(0.49f, -1.01f), groundLayers);
        wallLeft = Physics2D.OverlapArea(pos + new Vector2(-0.51f, 0.99f), pos + new Vector2(-0.45f, -0.99f), groundLayers);
        wallRight = Physics2D.OverlapArea(pos + new Vector2(0.45f, -0.99f), pos + new Vector2(0.51f, 0.99f), groundLayers);


        CheckForHorizontal();
        CheckForJump();
        if (grounded) {
            CheckForCrouch();
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            doJump = true;
        }
    }

    void CheckForCrouch() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            Crouch();
        } else {
            Stand();
        }
    }

    void Crouch() {
        if (isCrouching) return;

        var boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(1, 1);
        boxCollider.center = new Vector2(0, -0.5f);
        sprite.localScale = new Vector3(1, 1);
        sprite.localPosition = new Vector3(0, -0.5f);
        isCrouching = true;
    }

    void Stand() {
        if (!isCrouching) return;

        Vector2 pos = transform.position;
        bool canStand = !Physics2D.OverlapArea(pos + new Vector2(-0.49f, 0), pos + new Vector2(0.49f, 1.01f), groundLayers);
        if (canStand) {
            var boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(1, 2);
            boxCollider.center = new Vector2(0, 0);
            sprite.localScale = new Vector3(1, 2);
            sprite.localPosition = new Vector3(0, 0);
            isCrouching = false;
        }
    }

    void CheckForJump() {
        if (doJump) {
            doJump = false;
            if (grounded) {
                rigidbody2D.AddForce(new Vector2(0, jumpForce));
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

        rigidbody2D.AddForce(new Vector2(horizontal * 75, 0));
        Vector2 vel = rigidbody2D.velocity;
        float maxSpeed = isCrouching ? crouchSpeed : normalSpeed;
        vel.x = Mathf.Clamp(vel.x, -maxSpeed, maxSpeed);
        rigidbody2D.velocity = vel;

        if ((horizontal > 0 && !facingRight) ||
            (horizontal < 0 && facingRight)) {
            Flip();
        }
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
