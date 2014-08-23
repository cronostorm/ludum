using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public float speed = 1.0f;
  
  private bool facingRight = true;

  public Transform groundCheck;
  public LayerMask groundLayers;
  public float jumpForce;
  private bool grounded = false;
  private float groundRadius = 0.2f;
  

	void Start () {
	}
	
  void FixedUpdate() {
    grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayers);
    CheckForHorizontal();
  }

  void Update() {
    if (grounded) {
      CheckForJump();
    }
  }
  
  void CheckForJump() {
    float vertical = Input.GetAxis("Vertical");
    if (vertical > 0) {
      rigidbody2D.AddForce(new Vector2(0, jumpForce));
      grounded = false;
    }
  }

  void CheckForHorizontal() {
    float horizontal = Input.GetAxis("Horizontal");
    Move(horizontal * speed, rigidbody2D.velocity.y);
    
    if ((horizontal > 0 && !facingRight) || 
        (horizontal < 0 && facingRight)) {
      Flip();
    }
  }

  void Move(float xvel, float yvel) {
    rigidbody2D.velocity = new Vector2(xvel, yvel);
  }

  void Flip() {
    facingRight = !facingRight;
    Vector3 scale = transform.localScale;
    scale.x *= -1;
    transform.localScale = scale;
  }
}
